using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : MonoBehaviour
{
    public List<GameObject> craftables;
    public List<Button> craftingButtons;
    public Transform spawmPoint;

    private int slide = 0;
    private MetalChest chest;

    // Start is called before the first frame update
    void Start()
    {
        chest = GetComponent<MetalChest>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //steel
        if (chest.GetOres(0) >= 1)
        {
            if(chest.GetOres(1) >= 1)
            {
                craftingButtons[0].interactable = true;
            }
            else
            {
                craftingButtons[0].interactable = false;
            }
        }
        else
        {
            craftingButtons[0].interactable = false;
        }

        //Iron
        if(chest.GetOres(1) >= 2)
        {
            craftingButtons[1].interactable = true;
        }
        else
        {
            craftingButtons[1].interactable = false;
        }

        //Tin
        if(chest.GetOres(2) >= 2)
        {
            craftingButtons[2].interactable = true;
        }
        else
        {
            craftingButtons[2].interactable = false;
        }

        //Pewter
        if(chest.GetOres(2) >= 1)
        {
            if(chest.GetOres(3) >= 1)
            {
                craftingButtons[3].interactable = true;
            }
            else
            {
                craftingButtons[3].interactable = false;
            }
        }
        else
        {
            craftingButtons[3].interactable = false;
        }
        
        //Zinc
        if(chest.GetOres(4) >= 2)
        {
            craftingButtons[4].interactable = true;
        }
        else
        {
            craftingButtons[4].interactable = false;
        }

        //brass
        if(chest.GetOres(4) >= 1)
        {
            if(chest.GetOres(5) >= 1)
            {
                craftingButtons[5].interactable = true;
            }
            else
            {
                craftingButtons[5].interactable = false;
            }
        }
        else
        {
            craftingButtons[5].interactable = false;
        }

        //mixed
        bool enough = true;

        for(int i = 0; i <= 5; i++)
        {
            if(chest.GetOres(i) < 2)
            {
                enough = false;
            }
        }

        if (enough)
        {
            craftingButtons[6].interactable = true;
        }
        else
        {
            craftingButtons[6].interactable = false;
        }

        //coin
        if(chest.GetOres(2) >= 1)
        {
            if(chest.GetOres(5) >= 1)
            {
                craftingButtons[7].interactable = true;
            }
            else
            {
                craftingButtons[7].interactable = false;
            }
        }
        else
        {
            craftingButtons[7].interactable = false;
        }

    }


    public void craft(int index)
    {
        switch (index)
        {
            //Steel
            case 0:
                chest.SubstractOres(1, 0);
                chest.SubstractOres(1, 1);
                break;
            //Iron
            case 1:
                chest.SubstractOres(2, 1);
                break;
            //Tin
            case 2:
                chest.SubstractOres(2, 2);
                break;
            //Pewter
            case 3:
                chest.SubstractOres(1, 2);
                chest.SubstractOres(1, 3);
                break;
            //Zinc
            case 4:
                chest.SubstractOres(2, 4);
                break;
            //Brass
            case 5:
                chest.SubstractOres(1, 4);
                chest.SubstractOres(1, 5);
                break;
            //mixed
            case 6:
                for(int i = 0; i <= 5; i++)
                {
                    chest.SubstractOres(2, i);
                }
                break;
            //coin
            case 7:
                chest.SubstractOres(1, 2);
                chest.SubstractOres(1, 5);
                break;
            
        }

        Instantiate(craftables[index], spawmPoint.transform.position, spawmPoint.transform.rotation);
    }

    public void changeSlides(bool next)
    {
        if (next)
        {
            if(slide < 3)
            {
                slide++;

                foreach(Button current in craftingButtons)
                {
                    current.gameObject.SetActive(false);
                }

                craftingButtons[slide*2].gameObject.SetActive(true);
                craftingButtons[(slide*2) + 1].gameObject.SetActive(true);
            }
        }
        else
        {
            if (slide > 0)
            {
                slide--;

                foreach (Button current in craftingButtons)
                {
                    current.gameObject.SetActive(false);
                }

                craftingButtons[slide*2].gameObject.SetActive(true);
                craftingButtons[(slide*2) + 1].gameObject.SetActive(true);
            }
        }
    }

}
