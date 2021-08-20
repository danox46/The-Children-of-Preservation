using UnityEngine;
using TigerForge;

// Class for the Coins.

public class Coin : MonoBehaviour
{

    // Every time that the Coin receives a collision with the Player...
    void OnTriggerEnter(Collider other)
    {
        // ...it emits the "ON_COIN_TAKEN" event, passing a value of 10.
        EEventManger.EmitEvent("ON_COIN_TAKEN", 10);

        // Eventually the Coin is destroyed.
        Destroy(gameObject);
    }

}
