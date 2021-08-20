using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchableItem : MonoBehaviour
{
    public GameManager manager;
    public int storyTrigger;

    private bool dropped;
    

    // Start is called before the first frame update
    void Start()
    {
        dropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Searching()
    {
        if (!dropped)
        {
            if(storyTrigger > -1)
            {
                manager.NewStoryMessage(storyTrigger);
            }

            GetComponent<Animator>().SetTrigger("Searched");
            GetComponent<DropList>().FixedDrop();
            dropped = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public bool GetDropped()
    {
        return dropped;
    }
}
