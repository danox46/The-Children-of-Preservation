using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleState : IkolosState
{
    private Kolos kolo;
    private WeaponInfo currentWeapon;
    private float attackTime = 2f;
    private float attackCounter;
    private bool punched;

    public void Enter(Kolos kolos)
    {
        this.kolo = kolos;
        currentWeapon = kolo.GetComponentInChildren<WeaponInfo>();
        kolo.stateName = "Melee";

        //kolo.Move(0, false, false);

        attackCounter = 0;
        punched = false;
    }

    public void Execute()
    {
        if (!punched)
        {
            
            punched = Punch();
            //kolo.StartCoroutine(kolo.AtiumAction());
        }

        if (kolo.target != null)
        {

            if ((Mathf.Abs(kolo.target.transform.position.x - kolo.transform.position.x) > kolo.meleeRange))
            {
                //Debug.Log("Kolo is on Meele range");
                kolo.ChangeState(new ChaseState());
            }

            if ((!kolo.attaking) && punched)
            {
                //Debug.Log("Kolo attacking? " + kolo.attaking + "punched " + punched);
                if (kolo.target != null)
                    kolo.ChangeState(new ChaseState());
                else
                    kolo.ChangeState(new IdleState());
            }
        }
        else
        {
            kolo.ChangeState(new IdleState());
        }

    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    private bool Punch()
    {
        if (kolo.meeleTimer >= kolo.meeleCD)
        {
            kolo.meeleTimer = 0;
            //kolo.attaking = true;
            currentWeapon.Attack1();
            return true;
        }
        else
        {
            return false;
        }
    }

}
