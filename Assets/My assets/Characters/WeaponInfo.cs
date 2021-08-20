using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class WeaponInfo : MonoBehaviour
{
    public float extraAttack;
    public float weaponPush;
    [Range(0, 1)] [SerializeField] float critChance;

    [SerializeField] protected PlatformerCharacter2D weaponUser;
    protected PlatformerCharacter2D hitted;
    protected List<Collider2D> alreadyHitted;

    public virtual void Start()
    {

        //get the user's information
        weaponUser = GetComponentInParent<PlatformerCharacter2D>();

        alreadyHitted = new List<Collider2D>();
    }



    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent != null)
        {
            //no friendly fire
            if (transform.parent.tag == "Player")
            {
                if (collision.tag == "Enemy")
                {
                    if (!alreadyHitted.Contains(collision))
                    {
                        alreadyHitted.Add(collision);

                        //get the info for the hitted enemy
                        hitted = collision.GetComponent<PlatformerCharacter2D>();

                        //send damage to the hitted enemy
                        bool hit = hitted.ReciveDamage(weaponUser.baseAttack + extraAttack, GetComponentInParent<Transform>().gameObject, critChance);

                        //if the enemy took damage from the hit
                        if (hit)
                        {
                            //reference the rigidbodies and add force acording to the differece in velocity and the base weapon push
                            Rigidbody2D userRB = this.GetComponentInParent<Rigidbody2D>();
                            Rigidbody2D hittedRB = collision.GetComponent<Rigidbody2D>();
                            //Debug.Log("difference in velocity" + (userRB.velocity.magnitude - hittedRB.velocity.magnitude));
                            if (weaponUser.GetFacingRight())
                            {
                                hittedRB.AddForce(Vector2.right * (weaponPush), ForceMode2D.Impulse);
                            }
                            else
                            {
                                hittedRB.AddForce(Vector2.left * (weaponPush), ForceMode2D.Impulse);
                            }

                        }
                        else
                        {
                            Debug.Log("the attack missed");
                        }
                    }
                }
            }

            if (transform.parent.tag == "Enemy")
            {
                if (!alreadyHitted.Contains(collision))
                {
                    if (collision.gameObject.layer == 18 ||collision.tag == "Enraged")
                    {
                        if(collision.gameObject.layer == 18)
                            hitted = collision.GetComponentInParent<PlatformerCharacter2D>();
                        else
                        //get the info for the hitted enemy
                        hitted = collision.GetComponent<PlatformerCharacter2D>();

                        //send damage to the hitted enemy
                        bool hit = hitted.ReciveDamage(weaponUser.baseAttack + extraAttack, GetComponentInParent<Transform>().gameObject, critChance);

                        //if the enemy took damage from the hit
                        if (hit)
                        {
                            //reference the rigidbodies and add force acording to the differece in velocity and the base weapon push
                            Rigidbody2D userRB = this.GetComponentInParent<Rigidbody2D>();
                            Rigidbody2D hittedRB = hitted.GetComponent<Rigidbody2D>();

                            if (weaponUser.GetFacingRight())
                            {
                                Debug.Log(hittedRB.tag);
                                hittedRB.AddForce(new Vector2(1f,0.5f) * (weaponPush), ForceMode2D.Impulse);
                            }
                            else
                            {
                                hittedRB.AddForce(new Vector2(-1f, 0.5f) * (weaponPush), ForceMode2D.Impulse);
                            }

                            
                        }
                        else
                        {
                            Debug.Log("the attack missed");
                        }
                    }
                }
            }

            if(transform.parent.tag == "Enraged")
            {
                if (!alreadyHitted.Contains(collision))
                {
                    if (collision.tag == "Enemy")
                    {
                        //get the info for the hitted enemy
                        hitted = collision.GetComponent<PlatformerCharacter2D>();

                        //send damage to the hitted enemy
                        bool hit = hitted.ReciveDamage(weaponUser.baseAttack + extraAttack, GetComponentInParent<Transform>().gameObject, critChance);

                        //if the enemy took damage from the hit
                        if (hit)
                        {
                            //reference the rigidbodies and add force acording to the differece in velocity and the base weapon push
                            Rigidbody2D userRB = this.GetComponentInParent<Rigidbody2D>();
                            Rigidbody2D hittedRB = collision.GetComponent<Rigidbody2D>();
                            if (weaponUser.GetFacingRight())
                            {
                                hittedRB.AddForce(Vector2.right * (weaponPush), ForceMode2D.Impulse);
                            }
                            else
                            {
                                hittedRB.AddForce(Vector2.left * (weaponPush), ForceMode2D.Impulse);
                            }

                        }
                        else
                        {
                            Debug.Log("the attack missed");
                        }
                    }
                }
            }

            if (collision.tag == "Breakable")
            {
                if (!alreadyHitted.Contains(collision))
                {
                    alreadyHitted.Add(collision);
                    Debug.Log("Hit an item");
                    BreakableItem item = collision.GetComponent<BreakableItem>();
                    item.ReciveDamage(weaponUser.baseAttack + extraAttack, transform.name);

                }

            }
        }
    }

    public virtual void ClearAlreadyHitted()
    {
        alreadyHitted.Clear();
    }

    public virtual void Attack1()
    {
        if (weaponUser.alive && !weaponUser.GetStunned())
        {
            if (weaponUser.attaking == false)
            {
                weaponUser.TriggerAttacking();
            }
        }
        /*else
        {
            if (weaponUser.IsComboTime() && !weaponUser.m_Anim.GetBool("ComboAttack"))
            {
                //Debug.Log("Calling combo");
                weaponUser.m_Anim.SetTrigger("Combo");
            }
        }*/

    }

    

        //basic attack function
    /*public virtual IEnumerator Attack1()
    {
        if (weaponUser.alive)
        {
            //looks the user on attack routine
            attaking = true;
            weaponUser.attaking = true;
            float sizeUnit = range / resizeRate;
            weaponUser.m_Anim.SetTrigger("Attacking");
            yield return new WaitForSeconds(attSpd);
            attaking = false;
            weaponUser.attaking = false;
        }
    }*/


}


