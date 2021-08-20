using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField]
    private Kolos kolo;

    private void Start()
    {
        kolo = GetComponentInParent<Kolos>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (kolo.tag == "Enemy")
        {
            if (collision.transform.tag == "Player")
            {
                kolo.target = collision.gameObject;
                GetComponent<PolygonCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = true;
            }
        }

        if(kolo.tag == "Enraged")
        {
            if(collision.transform.tag == "Enemy")
            {
                kolo.target = collision.gameObject;
                GetComponent<PolygonCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = true;
            }
        }

        /*/if (kolo.stateName == "Patrol" && collision.transform.tag == "Enemy")
        {
            kolo.pathBlocked = false;

            if(kolo.m_FacingRight && kolo.transform.position.x < collision.transform.position.x && Vector2.Distance(kolo.transform.position, collision.transform.position) < pathTolrance)
                kolo.pathBlocked = true;

            if (!kolo.m_FacingRight && kolo.transform.position.x > collision.transform.position.x && Vector2.Distance(kolo.transform.position, collision.transform.position) < pathTolrance)

                Debug.LogWarning("Distance" + Vector2.Distance(kolo.transform.position, collision.transform.position));
     
        }/*/

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (kolo.alive)
        {
            if (collision.transform.tag == "Player")
            {
                kolo.target = null;
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = false;
                //Debug.Log("target lost");
            }

            if (kolo.tag == "Enraged")
            {
                if (collision.transform.tag == "Enemy")
                {
                    kolo.target = null;
                    GetComponent<PolygonCollider2D>().enabled = true;
                    GetComponent<CircleCollider2D>().enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if (kolo.target != null)
        {
            if ((kolo.target.transform.position.x < kolo.transform.position.x) & kolo.GetFacingRight())
            {
                kolo.Flip();
            }

            if ((kolo.target.transform.position.x > kolo.transform.position.x) & !kolo.GetFacingRight())
            {
                kolo.Flip();
            }
        }
    }
}
