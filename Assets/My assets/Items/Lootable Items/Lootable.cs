using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable 
{

    public int lootType;
    public float effectValue;
    public Sprite icon;


    public Lootable(Lootable lootable)
    {
        lootType = lootable.lootType;
        effectValue = lootable.effectValue;
        icon = lootable.icon;
    }

    public Lootable(LootBehaviour Loot)
    {
        lootType = Loot.lootType;
        effectValue = Loot.effectValue;
        icon = Loot.GetComponent<SpriteRenderer>().sprite;
    }

    public void LootableEXT(Lootable lootable)
    {
        lootType = lootable.lootType;
        effectValue = lootable.effectValue;
        icon = lootable.icon;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
