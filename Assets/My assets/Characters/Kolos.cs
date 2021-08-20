using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Kolos : PlatformerCharacter2D
{
    public Rigidbody2D rockPF;
    //EnemyStates
    private IkolosState currentState;
    public string stateName;
    public Transform patrolBase;
    public bool home;
    public int baseSize;
    public bool pathBlocked;

    public GameObject model;
    public GameObject sight;
    public GameObject weapon;

    public float meeleCD;
    public float meeleTimer;

    //Atium Shadow
    //public KolosAS atiumShadow;
    //Need to find this on the game manager
    public bool atiumOn;

    //targeting
    public GameObject target;
    public float rangedRange;
    public float meleeRange;
    private bool shooting = false;
    public int baseForce;
    public Waypoint waypoint;

    private bool dropped;

    public int rocks;

    public float rage = 0;
    //public bool enraged;

    public override void Awake()
    {
        base.Awake();
        //get the range of the current weapon
        //meleeRange = 2;
        //meeleCD = 3;
        meeleTimer = 0;
        waypoint = null;
        //enraged = false;

    }

    public void Start()
    {
        //start in idle
        ChangeState(new IdleState());
        //AtiumMode();

    }

    public override void Update()
    {
        base.Update();

        //if alive and not stunned run the current state
        if (alive && !GetStunned())
            currentState.Execute();

        if (transform.position.x < patrolBase.position.x)
        {
            if (Mathf.Abs(patrolBase.position.x) - Mathf.Abs(transform.position.x) < baseSize)
                home = true;
            else
                home = false;
        }
        else
        {
            if (Mathf.Abs(transform.position.x) - Mathf.Abs(patrolBase.position.x) < baseSize)
                home = true;
            else
                home = false;
        }

        if(meeleTimer < meeleCD)
        {
            meeleTimer += Time.deltaTime;
        }

        if(rage >= 100)
        {
            tag = "Enraged";
            gameObject.layer = 20;
            weapon.layer = 20;
            sight.GetComponent<PolygonCollider2D>().enabled = false;
            sight.GetComponent<PolygonCollider2D>().enabled = true;
            sight.layer = 20;
            GetComponentInChildren<EmotionalUI>().Deactivate();
        }

        /*if (rage <= 0)
            tag = "Enemy";*/

        if(rage > 0)
            rage -= Time.deltaTime*5;

    }

    // Update is called once per frame
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
        if (collision.tag == "Waypoint")
            waypoint = collision.GetComponent<Waypoint>();


    }

    public override bool ReciveDamage(float dmg, GameObject source, float crtChance)
    {
        if(source.tag == "Weapon")
        {
            target = source;
        }

        return base.ReciveDamage(dmg, source, crtChance);

    }

    public override void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.


        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;


        /*Vector3 theScale = model.transform.localScale;
        theScale.x *= -1;
        model.transform.localScale = theScale;

        theScale = sight.transform.localScale;
        theScale.x *= -1;
        sight.transform.localScale = theScale;

        theScale = weapon.transform.localScale;
        theScale.x *= -1;
        weapon.transform.localScale = theScale;*/

    }

    //changing state function
    public void ChangeState(IkolosState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public override void Die()
    {
        base.Die();
        sight.SetActive(false);
        target = null;
        alive = false;

        if (!dropped)
        {
            GetComponent<DropList>().RamdomDrop();
            dropped = true;
        }
        //and set for destruction
        Destroy(gameObject, 3f);
    }

    //need to change this to sync with Anim and fix the rock speed
    public IEnumerator shootRock(Vector3 targetPos)
    {
        if (!shooting && (rocks > 0))
        {
            shooting = true;
            m_Anim.SetTrigger("Shooting");
            yield return new WaitForSeconds(0.2f);
            Rigidbody2D rock = Instantiate(rockPF);
            if (m_FacingRight)
            {
                rock.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, 0);
                rock.AddForce(targetPos * baseForce, ForceMode2D.Impulse);
            }
            else
            {
                rock.transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y, 0);
                targetPos = new Vector3(-targetPos.x, targetPos.y, 0);
                rock.AddForce(targetPos * baseForce, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.7f);
            shooting = false;
            rocks--;
            //force add should depend on base force
        }
    }

    /*public void AtiumMode()
    {
        Transform[] children;

        gameObject.layer = 15;
        Debug.Log("start");
        children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.tag == "Weapon")
            {
                child.gameObject.layer = 15;
                Debug.Log("end");
            }

        }



        //atiumShadow.enabled = true;
        atiumShadow.rocks = rocks;
        atiumShadow.hp = hp;
    }

    public IEnumerator AtiumAction(float m, bool c, bool j)
    {
        yield return new WaitForSeconds(2);
        atiumShadow.Move(m, c, j);
    }

    public IEnumerator AtiumAction(Vector3 target)
    {
        yield return new WaitForSeconds(2);
        atiumShadow.StartCoroutine(atiumShadow.shootRock(target));
    }

    public IEnumerator AtiumAction()
    {
        WeaponInfo currentWeapon = atiumShadow.GetComponentInChildren<WeaponInfo>();
        yield return new WaitForSeconds(2);
        currentWeapon.StartCoroutine(currentWeapon.Attack1());
    }

    public IEnumerator AtiumAction(int flip)
    {
        yield return new WaitForSeconds(2);
        atiumShadow.Flip();
    }*/
}
