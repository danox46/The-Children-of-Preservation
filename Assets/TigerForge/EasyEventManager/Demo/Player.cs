using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

// Class for the Player.

public class Player : MonoBehaviour
{

    private int coins = 0;

    // On game start...
    void Start()
    {
        // ...I register the listening of an Event with name "ON_COIN_TAKE".
        // If this Event is intercepted (it will be emitted by the Coins), the Event Manager will execute the OnCoinTaken() function.
        EEventManger.StartListening("ON_COIN_TAKEN", OnCoinTaken);
    }

    // Every time the OnCoinTaken is executed by the Event Manager...
    void OnCoinTaken()
    {
        // ...I increment the coins variable with the integer value received by the EmitEvent() method called by each Coin.
        coins += EEventManger.GetInteger("ON_COIN_TAKEN");
        
        // I show 'coins' value on Debug console.
        Debug.Log("Now you've got " + coins + " coins!");
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player forwards.
        transform.Translate(Vector3.forward * 3 * Time.deltaTime);
    }

    // If this Player is destroyed...
    void OnDestroy()
    {
        // ...I stop the listening of "ON_COIN_TAKEN" Event because it's no longer necessary (or to prevent an unneeded use of this Event).
        EEventManger.StopListening("ON_COIN_TAKEN", OnCoinTaken);
    }
}
