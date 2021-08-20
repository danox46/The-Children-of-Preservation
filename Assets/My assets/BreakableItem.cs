using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    private float hp;
    private float maxHp;



    // Start is called before the first frame update
    void Start()
    {
        maxHp = 10;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ReciveDamage(float dmg, string source)
    {
            //decrease hp
            hp -= (dmg);
            Debug.Log(source + " made: " + (dmg) + " damage points to: " + transform.name);

        if (hp <= 0)
        {

        }
            return true;

    }

    public void Broke()
    {

        Destroy(gameObject, 5f);
    }

}
