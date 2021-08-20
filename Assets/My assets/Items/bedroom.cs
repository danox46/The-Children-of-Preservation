using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class bedroom : MonoBehaviour
{
    public Platformer2DUserControl control;
    public DoorBehaviour door;
    private Animator animator;
    public GameObject restUI;
    public bool won = false;

    private bool sleeping = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        door.OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        if (sleeping)
        {
            control.GetComponent<Mistborn>().CureHp(Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            restUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            restUI.SetActive(false);
        }
    }

    public void Sleep()
    {
        if (sleeping)
        {
            sleeping = false;
            control.enabled = true;
            door.OpenDoor();
            animator.SetTrigger("LightsOn");
        }
        else
        {
            if (won)
            {
                control.enabled = false;
                sleeping = true;
                door.OpenDoor();
                animator.SetTrigger("LightsOff");
            }
            else
            {
                List<string> message = new List<string>();

                message.Add("Not time for sleep \n I need to deal with the Kolos");

                control.NewMessage(message);
            }
        }

    }
}
