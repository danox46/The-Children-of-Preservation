using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class MetalChest : MonoBehaviour
{
    public GameObject chestDisplay;
    public Platformer2DUserControl control;
    private bool inRage;
    private Animator animator;

    public Text steelOre;
    public Text ironOre;
    public Text tinOre;
    public Text pewterlOre;
    public Text ZincOre;
    public Text BrassOre;

    [SerializeField] private List<int> ores = new List<int>();

    public GameObject craftingUI;

    // Start is called before the first frame update
    void Start()
    {
        inRage = false;
        ores.Add(0);
        ores.Add(0);
        ores.Add(0);
        ores.Add(0);
        ores.Add(0);
        ores.Add(0);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            chestDisplay.SetActive(true);
            inRage = true;
            control = collision.GetComponent<Platformer2DUserControl>();
            animator.SetTrigger("Open");

            if (GetComponent<CraftingTable>().enabled)
            {
                craftingUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            chestDisplay.SetActive(false);
            craftingUI.SetActive(false);
            animator.SetTrigger("Close");
            inRage = false;
            control = null;
        }
    }

    public int GetOres(int index)
    {
        return ores[index];
    }

    public void SubstractOres(int amount, int index)
    {
        ores[index] -= amount;
        steelOre.text = ores[0].ToString();
        ironOre.text = ores[1].ToString();
        tinOre.text = ores[2].ToString();
        pewterlOre.text = ores[3].ToString();
        ZincOre.text = ores[4].ToString();
        BrassOre.text = ores[5].ToString();
    }

    public void StoreMetals()
    {
        if (inRage)
        {
            if (control.CheckInvIndex() > 0)
            {
                for (int i = 0; i <= 7; i++)
                {
                    if (control.CheckInvItem(i) != null)
                    {
                        switch (control.CheckInvItem(i).lootType)
                        {
                            case 10:
                                ores[0] += 1;
                                control.RemoveInvItem(i);
                                steelOre.text = ores[0].ToString();
                                break;

                            case 11:
                                ores[1] += 1;
                                control.RemoveInvItem(i);
                                ironOre.text = ores[1].ToString();
                                break;

                            case 12:
                                ores[2] += 1;
                                control.RemoveInvItem(i);
                                tinOre.text = ores[2].ToString();
                                break;

                            case 13:
                                ores[3] += 1;
                                control.RemoveInvItem(i);
                                pewterlOre.text = ores[3].ToString();
                                break;

                            case 14:
                                ores[4] += 1;
                                control.RemoveInvItem(i);
                                ZincOre.text = ores[4].ToString();
                                break;

                            case 15:
                                ores[5] += 1;
                                control.RemoveInvItem(i);
                                BrassOre.text = ores[5].ToString();
                                break;
                        }
                        
                    }
                }
            }
        }
    }


}
