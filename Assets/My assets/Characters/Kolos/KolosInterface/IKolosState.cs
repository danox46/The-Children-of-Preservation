using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public interface IkolosState
{

    void Execute();
    void Enter(Kolos kolos);
    void Exit();
    void OnTriggerEnter2D(Collider2D other);


}
