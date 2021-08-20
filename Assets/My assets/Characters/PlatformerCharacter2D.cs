using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] protected float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] protected float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] protected float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] protected bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] protected LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        protected Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        protected const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        protected bool m_Grounded;            // Whether or not the player is grounded.
        protected Transform m_CeilingCheck;   // A position marking where to check for ceilings
        protected const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        protected Animator m_Anim;            // Reference to the player's animator component.
        protected Rigidbody2D m_Rigidbody2D;
        protected bool m_FacingRight = true;  // For determining which way the player is currently facing.

        // Char Stats
        public float baseAttack;
        [SerializeField] protected float defense;
        [SerializeField] protected float maxHp;
        [SerializeField] protected float hp;
        //Need to change the speed of the animation depending on AttSpd
        //public float attSpd;
        public float stunTime = 0.5f;
        [SerializeField] private bool comboWindow;

        //this are routine keys, to make sure the character can't perform any actions while attacking or after death
        public bool attaking = false;
        public bool alive;
        private bool stunned;

        public virtual void Awake()
        {
            //maxHp = 1000;
            hp = maxHp;
            //defense = 20;
            alive = true;
            comboWindow = false;
            stunned = false;

            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            m_Anim.SetFloat("AttackSpeed", 1);
        }

        public virtual void Update()
        {
            //check if the char died
            if (hp <= 0)
            {
                Die();
            }
        }

        public virtual void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

            /*if (!m_Anim.GetBool("Ground")) 
                Debug.Log("vSpeed: " + m_Anim.GetFloat("vSpeed"));*/
        }

        //changed to punlic virtual for inheritance
        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            //any collisions made to the player will trigger the recive damage function
            //Need to make damage grow sponential and relative to the mass of the object
            if (!(collision.collider.isTrigger))
                ReciveDamage((collision.relativeVelocity.magnitude* collision.relativeVelocity.magnitude)/2, collision.gameObject, 0); // Update Recive Damage function
        }


        public virtual void Move(float move, bool crouch, bool jump)
        {
            if (!m_Anim.GetBool("Stunned") && alive && !attaking)
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

                //adding this provisionally
                
                if(m_Rigidbody2D.velocity.magnitude < 0.01)
                {
                    m_AirControl = true;
                }
                else
                {
                    m_AirControl = false;
                }

                //only control the player if grounded or airControl is turned on
                if ((m_Grounded || m_AirControl) && !(attaking))
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    move = (crouch ? move * m_CrouchSpeed : move);

                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

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


        public virtual void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        
        }


        //This function needs to be updated for doge
        public virtual bool ReciveDamage(float dmg, GameObject source, float crtChance)
        {
            if (dmg > defense)
            {

                float drawDmg = UnityEngine.Random.value;

                if (drawDmg < 1 - crtChance)
                {
                    hp -= (dmg + (dmg * (drawDmg / 3)) - defense);
                    //Debug.Log("Hitted: " + (dmg * (drawDmg / 3)) + "from: " + source.name + " your hp is: " + hp);
                    if (hp > 0)
                    {
                        m_Anim.SetFloat("StunTime", stunTime);
                    }
                    //remember to update HP Bar

                }
                else
                {
                    hp -= (dmg + (dmg * 2)) - defense;
                    Debug.Log("Critical Hit! " + (dmg * 2) + "from: " + source);
                    if(hp > 0)
                        m_Anim.SetFloat("StunTime", stunTime);

                }
                return true;


            }
            else
            {
                return false;
            }

        }

        public void CureHp(float amount)
        {
            if((hp+amount) < maxHp)
            {
                hp += amount;
            }
            else
            {
                hp = maxHp;
            }
        }

        public virtual void Die()
        {
            //what is dead can never die
            if (alive)
            {
                //take the object to a different collision layer
                gameObject.layer = 8;
                m_Anim.SetFloat("Speed", 0);
                m_Anim.SetTrigger("Dead");

                /*Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (Collider2D col in colliders)
                {
                    if (col.tag == "Weapon")
                    {
                        if (col.name == "Unarmed")
                            Destroy(col);
                        else
                        {
                            col.GetComponent<SpriteRenderer>().enabled = false;
                            Destroy(col);
                        }
                    }

                }*/


                alive = false;

            }

        }

        //This function needs to be synced with the animator
        public IEnumerator Stun()
        {
            stunned = true;
            //Debug.Log("stunned 1");
            yield return new WaitForSeconds(stunTime);
            stunned = false;
        }

        public void ComboTime()
        {
            comboWindow = !comboWindow;
            //Debug.Log("Combo Time = " + comboWindow);
        }

        public bool IsComboTime()
        {
            return comboWindow;
        }

        public bool GetFacingRight()
        {
            return m_FacingRight;
        }

        public bool GetStunned()
        {
            return m_Anim.GetBool("Stunned");
        }

        public void TriggerAttacking()
        {
            //Debug.Log("Kolo triggers the attack");
            m_Anim.SetTrigger("Attacking");
        }

        public float GetHp()
        {
            return hp;
        }

        public float GetMaxHp()
        {
            return maxHp;
        }
    }
}
