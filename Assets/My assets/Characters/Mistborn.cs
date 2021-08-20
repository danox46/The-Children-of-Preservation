using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;

public class Mistborn : PlatformerCharacter2D
{
    public Transform chest;
    //Attributes
    [Range(0, 1)] [SerializeField] float evasion;

    [SerializeField] private bool doging;
    [SerializeField] private float pewterMultiplier;

    public Platformer2DUserControl control;
    public bool won = false;

    public List<LootBehaviour> closeLoot;
    public List<SearchableItem> closeSearch;
    public List<DoorBehaviour> closeDoors;

    public GameObject deadMenu;
    public GameObject resetart;
    public GameObject respawn;

    private bool stopped;
    

    public override void Awake()
    {

        doging = false;
        evasion = 0.3f;
        stopped = false;

        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // check if lootable
        if(collision.gameObject.layer == 13)
        {
            if(!closeLoot.Contains(collision.GetComponentInParent<LootBehaviour>()))
                closeLoot.Add(collision.GetComponentInParent<LootBehaviour>());
        }

        if(collision.gameObject.layer == 16)
        {
            if(!closeSearch.Contains(collision.GetComponent<SearchableItem>()))
                closeSearch.Add(collision.GetComponent<SearchableItem>());
        }

        if(collision.gameObject.tag == "Door")
        {
            if (!closeDoors.Contains(collision.GetComponent<DoorBehaviour>()))
            {
                closeDoors.Add(collision.GetComponent<DoorBehaviour>());
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            closeLoot.Remove(collision.GetComponentInParent<LootBehaviour>());
        }

        if (collision.gameObject.layer == 16)
        {

            closeSearch.Remove(collision.GetComponent<SearchableItem>());
        }

        if (collision.gameObject.tag == "Door")
        {
            closeDoors.Remove(collision.GetComponent<DoorBehaviour>());
        }
    }

    public override void Flip()
    {
        base.Flip();

        //And let know all allomantic lines attached to the character that it flipped
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.tag == "Line")
            {
                ALControler childController = child.GetComponent<ALControler>();
                childController.facingRight = m_FacingRight;
            }

        }
    }

    public override bool ReciveDamage(float dmg, GameObject source, float crtChance)
    {
        if (dmg > defense && !doging)
        {
            if (Random.value > evasion)
            {
                float drawDmg = Random.value;

                attaking = false;

                if (drawDmg < 1 - crtChance)
                {
                    hp -= (dmg + (dmg*(drawDmg/3)) - defense);
                    //Debug.Log("Hitted: " + (dmg * (drawDmg / 3)) + "from: " + source);
                    if(hp > 0)
                        m_Anim.SetFloat("StunTime", stunTime);
                    //remember to update HP Bar
                    
                }
                else
                {
                    hp -= (dmg + (dmg * 2)) - defense;
                    Debug.Log("Critical Hit! " + (dmg * 2) + "from: " + source);
                    if(hp > 0)
                        m_Anim.SetFloat("StunTime", stunTime*2);

                }
                return true;

            }
            else
            {
                m_Anim.SetTrigger("Doging");
                return false;
            }
        }
        else
            return false;


    }

    public override void Move(float move, bool crouch, bool jump)
    {
        if (!m_Anim.GetBool("Stunned") && alive && !attaking && !doging)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    //Debug.Log("Forcing a crowch");
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if ((m_Grounded || m_AirControl) && !(attaking))
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                if (Mathf.Abs(move) > 0)
                {
                    stopped = false;
                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    if (!stopped)
                    {
                        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                        m_Anim.SetFloat("Speed", 0);
                        stopped = true;
                    }
                }

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
        else
        {
            m_Anim.SetFloat("Speed", 0);
            //m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        }
    }


    public override void Die()
    {
        base.Die();
        transform.Find("Chest").gameObject.layer = 8;
        GetComponentInChildren<ALSpawmer>().MetalsOff();
        

        if (won)
        {
            deadMenu.SetActive(true);
            resetart.SetActive(false);
            respawn.SetActive(true);
        }
        else
        {
            deadMenu.SetActive(true);
            resetart.SetActive(true);
            respawn.SetActive(false);
        }

        //if()
        //SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    public void Shotting()
    {
        GetComponentInChildren<ALSpawmer>().Shoot();
    }

    public void SetDoging(bool newValue)
    {
        attaking = false;
        doging = newValue;
    }
    
    public bool GetDoging()
    {
        return doging;
    }

    public void Pewter(bool on)
    {
        if (on)
        {
            //float dif = maxHp * pewterMultiplier - maxHp;

            maxHp = maxHp * pewterMultiplier;
            hp = hp * pewterMultiplier;
            baseAttack = baseAttack * (pewterMultiplier - 0.3f);
            m_Anim.SetFloat("AttackSpeed", m_Anim.GetFloat("AttackSpeed") * pewterMultiplier);
            defense = defense * (pewterMultiplier - 0.3f);
            m_MaxSpeed = m_MaxSpeed * pewterMultiplier;
            m_JumpForce = m_JumpForce * pewterMultiplier;
        }
        else
        {
            maxHp = maxHp / pewterMultiplier;

            hp = hp / pewterMultiplier;

            baseAttack = baseAttack / (pewterMultiplier - 0.3f);
            m_Anim.SetFloat("AttackSpeed", m_Anim.GetFloat("AttackSpeed") / pewterMultiplier);
            defense = defense / (pewterMultiplier - 0.3f);
            m_MaxSpeed = m_MaxSpeed / pewterMultiplier;
            m_JumpForce = m_JumpForce / pewterMultiplier;
        }

    }

    public bool GetShooting()
    {
        return m_Anim.GetBool("Shooting");
    }

    public void SetShooting(bool on)
    {
        m_Anim.SetBool("Shooting", on);
    }

    public bool GetCombo()
    {
        return m_Anim.GetBool("ComboAttack");
    }

    public void TriggerCombo()
    {
        m_Anim.SetTrigger("Combo"); 
    }

    public void TriggerRespawn()
    {
        hp = maxHp;
        alive = true;

        gameObject.layer = 11;
        transform.Find("Chest").gameObject.layer = 18;

        m_Anim.SetTrigger("Respawn");
    }

}
