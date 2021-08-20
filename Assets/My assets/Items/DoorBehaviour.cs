using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool opened;
    public bool locked;
    public int keyType;
    public MessageBox messageBox;
    public int storyTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (!opened)
        {
            GetComponent<Animator>().SetTrigger("Open");
            opened = true;
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Close");
            opened = false;
        }
    }


}
