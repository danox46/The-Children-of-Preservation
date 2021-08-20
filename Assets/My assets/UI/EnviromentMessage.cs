using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class EnviromentMessage : MonoBehaviour
{
    [SerializeField] private List<string> message;
    private bool given;
    private float givenTimer;


    // Start is called before the first frame update
    void Start()
    {
        given = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (given)
        {
            givenTimer += Time.deltaTime;

            if(givenTimer >= 15)
            {
                given = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!given)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Platformer2DUserControl>().NewMessage(message);
                given = true;
            }
        }
    }
}
