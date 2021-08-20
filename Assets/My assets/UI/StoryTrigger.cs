using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    public GameManager manager;
    public int storyIndex;

    private bool given = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 18)
        {
            if (!given)
            {
                manager.NewStoryMessage(storyIndex);
                given = true;
            }
        }
    }
}
