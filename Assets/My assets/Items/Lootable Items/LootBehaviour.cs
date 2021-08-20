using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehaviour : MonoBehaviour
{
    public int lootType;
    public float effectValue;
    public bool looted = false;

    public void Looting()
    {
        GetComponent<Animator>().SetTrigger("Looted");
    }

    public void Looted()
    {
        Destroy(gameObject);
    }
}
