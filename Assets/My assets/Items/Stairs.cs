using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Stairs : MonoBehaviour
{
    Transform stair;

    // Use this for initialization
    void Start()
    {
        stair = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] steps;

        if (collision.gameObject.layer == 18 || collision.gameObject.layer == 15)
        {
            steps = stair.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D step in steps)
            {
                step.isTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        Collider2D[] steps;

        if (collision.gameObject.layer == 18 || collision.gameObject.layer == 15)
        {
            steps = stair.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D step in steps)
            {
                if (!(step.tag == "Stair"))
                    step.isTrigger = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Horizontal: " + CrossPlatformInputManager.GetAxis("Vertical"));
        if (CrossPlatformInputManager.GetAxis("Vertical") < 0)
        {
            Collider2D[] steps;

            steps = stair.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D step in steps)
            {
                step.isTrigger = true;
            }

        }
    }
}
