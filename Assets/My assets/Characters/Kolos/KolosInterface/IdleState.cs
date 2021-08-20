using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IkolosState
{
    private float idleDuration = 2.5f;
    private float idleTimer;

    private Kolos kolo;

    public void Enter(Kolos kolos)
    {
        this.kolo = kolos;

        kolo.stateName = "Idle";

        kolo.Move(0, false, false);
        //kolo.StartCoroutine(kolo.AtiumAction(0, false, false));
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime;
        Idle();
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Idle()
    {

        if (idleTimer >= idleDuration)
        {
            kolo.ChangeState(new PatrolState());
        }

        if (kolo.target != null)
        {
            kolo.ChangeState(new ChaseState());
        }
    }
}
