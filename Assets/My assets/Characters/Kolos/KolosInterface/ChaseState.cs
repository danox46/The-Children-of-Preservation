using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IkolosState
{
    private Kolos kolo;

    public void Enter(Kolos kolos)
    {
        this.kolo = kolos;
        kolo.stateName = "Chase";
    }

    public void Execute()
    {
        Chase();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    private void Chase()
    {
        //if there is no target go to iddle
        if (kolo.target == null)
        {
            kolo.ChangeState(new IdleState());
        }
        else
        {
            //check is the target is withing melee range
            if ((Mathf.Abs(kolo.target.transform.position.x - kolo.transform.position.x) < kolo.meleeRange))
            {
                //Debug.Log("Kolo is on Meele range");
                kolo.ChangeState(new MeeleState());
            }
            //if the character is not on melee range but on ranged range go to rangestate
            else if ((Mathf.Abs(kolo.target.transform.position.x - kolo.transform.position.x) < kolo.rangedRange) && kolo.rocks > 0)
            {
                //Debug.Log("Kolo is on Ranged range");
                //kolo.ChangeState(new RangeState());
            }

            if ((Mathf.Abs(kolo.target.transform.position.x - kolo.transform.position.x) < 5))
            {
                kolo.Move(0, false, false);
            }
            else
            {
                //if not in range to attack move towards the target
                if (kolo.target.transform.position.x < kolo.transform.position.x)
                {
                    kolo.Move(-1, false, false);
                    //kolo.StartCoroutine(kolo.AtiumAction(-1, false, false));
                }
                else
                {
                    kolo.Move(1, false, false);
                    //kolo.StartCoroutine(kolo.AtiumAction(-1, false, false));
                }

            }

        }


    }
}
