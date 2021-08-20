using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBlock : MonoBehaviour
{
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponentInChildren<ALSpawmer>().burnPewter)
            {
                body.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                body.bodyType = RigidbodyType2D.Static;
            }
        }
    }



}
