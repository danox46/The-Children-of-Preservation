using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IkolosState
{

    private Kolos kolo;

    public void Enter(Kolos kolos)
    {
        this.kolo = kolos;

        kolo.stateName = "Return";
    }

    public void Execute()
    {
        if (kolo.target == null)
        {
            if (!kolo.home)
                GoHome();
            else
                kolo.ChangeState(new IdleState());
        }
        else
            kolo.ChangeState(new ChaseState());
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //throw new System.NotImplementedException();
    }

    private void GoHome()
    {
        if (kolo.patrolBase.position.x < kolo.transform.position.x)
        {
            kolo.Move(-1, false, false);
            //kolo.StartCoroutine(kolo.AtiumAction(-1, false, false));
        }

        else
        {
            kolo.Move(1, false, false);
            //kolo.StartCoroutine(kolo.AtiumAction(1, false, false));
        }


    }
}
