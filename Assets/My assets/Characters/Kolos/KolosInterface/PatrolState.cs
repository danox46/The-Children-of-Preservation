using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IkolosState
{
    private Kolos kolo;

    private float lookArround;
    private float lookingArround;

    //when patroling, take breaks to idle after patrolDuration
    private float patrolDuration = 10;
    private float patrolTimer;

    public void Enter(Kolos kolos)
    {
        this.kolo = kolos;

        kolo.stateName = "Patrol";

        patrolTimer = 0;
        lookArround = 3;
        lookingArround = 0;
    }

    public void Execute()
    {
        if (kolo.home)
            Patrol();
        else
            kolo.ChangeState(new ReturnState());
        patrolTimer += Time.deltaTime;
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    private void Patrol()
    {
        if(kolo.waypoint != null)
        {
            switch (kolo.waypoint.waypointType)
            {
                case 1:
                    kolo.Flip();
                    kolo.waypoint = null;
                        break;
            }
        }

        if (patrolTimer >= lookArround)
        {
            if (lookingArround == 0)
            {
                kolo.Move(0, false, false);
                kolo.Flip();
                lookingArround = patrolTimer;
            }
            else
            {
                if (patrolTimer - lookingArround >= 1)
                {
                    kolo.Flip();
                    lookArround = lookArround + patrolTimer;
                    lookingArround = 0;
                }
            }
        }
        else
        {
            //the localscale will tell the kolo to move fordward regardless of where he's facing
            kolo.Move(kolo.transform.localScale.x / 4, false, false);
            /*if (kolo.atiumOn)
            {
                kolo.StartCoroutine(kolo.AtiumAction(kolo.transform.localScale.x / 4, false, false));
            }*/
        }

        if (patrolTimer >= patrolDuration)
        {
            kolo.ChangeState(new IdleState());
        }

        //if a target is found chase it
        if (kolo.target != null)
        {
            kolo.ChangeState(new ChaseState());
        }
    }
}
