using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistbornWeapon : WeaponInfo
{

    public override void Start()
    {
        base.Start();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void ClearAlreadyHitted()
    {
        base.ClearAlreadyHitted();
    }

    public override void Attack1()
    {
        //Debug.Log("Calling Attack1 from weapon, Alive: " + weaponUser.alive + " Stunned: " + weaponUser.m_Anim.GetBool("Stunned") + " Doging: " + weaponUser.GetComponent<Mistborn>().GetDoging());
        if (weaponUser.alive && !weaponUser.GetStunned() && !weaponUser.GetComponent<Mistborn>().GetDoging())
        {
            if (weaponUser.attaking == false)
            {
                weaponUser.TriggerAttacking();
            }
            else
            {
                if (weaponUser.IsComboTime() && !weaponUser.GetComponent<Mistborn>().GetCombo())
                {
                    //Debug.Log("Calling combo");
                    weaponUser.GetComponent<Mistborn>().TriggerCombo();
                }
            }
        }

    }
}
