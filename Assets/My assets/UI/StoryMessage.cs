using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class StoryMessage : MessageBox
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DisplayMessage(List<string> message)
    {
        player.GetComponent<PlatformerCharacter2D>().Move(0, false, false);
        player.GetComponentInChildren<ALSpawmer>().MetalsOff();
        player.GetComponent<Platformer2DUserControl>().enabled = false;
        base.DisplayMessage(message);
    }

    

    public override void Deactivate()
    {
        player.GetComponent<Platformer2DUserControl>().enabled = true;
        base.Deactivate();
    }

}
