using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropList : MonoBehaviour
{

    public List<GameObject> PosibleDrops;
    public Transform dropPoint;

    private bool droped = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RamdomDrop()
    {
        if (!droped)
        {
            int dropAmount = Random.Range(1, 3);

            for (int i = 0; i < dropAmount; i++)
            {
                int draw = Random.Range(0, PosibleDrops.Count - 1);
                Instantiate(PosibleDrops[draw], dropPoint.transform.position, dropPoint.transform.rotation);
            }

            droped = true;
        }
        
    }

    public void FixedDrop()
    {
        //int y= 0;

        if (!droped)
        {
            foreach (GameObject current in PosibleDrops)
            {

                Instantiate(current, dropPoint.transform.position, dropPoint.transform.rotation);
                //y++;
            }

            droped = true;
        }

        
    }


}
