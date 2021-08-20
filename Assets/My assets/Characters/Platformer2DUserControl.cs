using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        RaycastHit2D[] mouseRayHits;
        Ray mouseRay;

        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private WeaponInfo currentWeapon;

        private Lootable[] inventory;
        private int invIndex;

        private Lootable[] equipment;
        private int equIndex;
        private int selectedEquipment = -1;

        public List<Image> slotIcons;

        public List<GameObject> metalTimers;

        //UI
        private Image healthBar;
        private Vector3 healthScale;

        public GameObject messageBox;

        //public Text hpDisplay;
        public Transform hpBar;

        [SerializeField] private bool coinShot;
        [SerializeField] private bool lurcher;
        [SerializeField] private bool tinEye;
        [SerializeField] private bool pewterArm;
        [SerializeField] private bool rioter;
        [SerializeField] private bool soother;


        public List<Image> slots = new List<Image>();

        

        private void Awake()
        {
            healthBar = hpBar.GetComponent<Image>();
            healthScale = hpBar.transform.localScale;
            currentWeapon = GetComponentInChildren<WeaponInfo>();
            m_Character = GetComponent<PlatformerCharacter2D>();
            //Debug.Log("Name: " + m_Character.name);

            inventory = new Lootable[8];
            equipment = new Lootable[3];
            invIndex = 0;
        }


        private void Update()
        {
            //hpDisplay.text = Mathf.RoundToInt(m_Character.GetHp()).ToString();
            UpdateHP();

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            GameObject hitted;

            Transform[] children = GetComponentsInChildren<Transform>();
            bool found;
            int innerCount;



            //Store the position of the mouse on the camera as a ray
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Launch a raycastall on the mouse position
            mouseRayHits = Physics2D.RaycastAll(mouseRay.origin, mouseRay.direction);

            if (mouseRayHits.Length > 0)
            {
                //For each child of the player
                foreach (Transform child in children)
                {

                    //All lines 
                    if (child.tag == "Line")
                    {
                        //Debug.Log("Found a line");
                        //Reference the line controler script
                        ALControler childController = child.GetComponent<ALControler>();
                        found = false;
                        innerCount = 0;

                        //Look for this child in mouseRayHits 
                        do
                        {

                            RaycastHit2D hit = mouseRayHits[innerCount];

                            //Get the gameObject attached to the collider hitted by the ray
                            hitted = hit.collider.gameObject;

                            //If the gameobject hitted is meant for UI
                            if (hitted.layer == 5)
                            {
                                found = hitted.name == child.name;
                            }
                            innerCount++;
                            if (found)
                                break;
                        } while (innerCount < (mouseRayHits.Length));

                        //if none of this child is not in the list, the do while breaks because the innerCount and sends false;

                        childController.mouseOver = found;

                        if (m_Character.alive && (gameObject.GetComponentInChildren<ALSpawmer>().burnSteel || gameObject.GetComponentInChildren<ALSpawmer>().burnIron))
                        {

                            //this whole section needs to move to the plataformer user control script

                            if (CrossPlatformInputManager.GetButtonDown("Fire1") && gameObject.GetComponentInChildren<ALSpawmer>().burnSteel)
                            {
                                childController.pushing = true;
                            }

                            if (CrossPlatformInputManager.GetButtonUp("Fire1"))
                                childController.pushing = false;

                            if (CrossPlatformInputManager.GetButtonDown("Fire2") && gameObject.GetComponentInChildren<ALSpawmer>().burnIron)
                                childController.pulling = true;


                            if (CrossPlatformInputManager.GetButtonUp("Fire2"))
                                childController.pulling = false;

                        }


                    }

                    /*if(child.tag == "Enemy")
                    {
                        //Debug.Log("OverEnemy");
                        if(CrossPlatformInputManager.GetButtonDown("Fire1") && gameObject.GetComponentInChildren<ALSpawmer>().burnZinc)
                        {
                            float draw = UnityEngine.Random.Range(0, 1);
                            Debug.Log("Drawing " + draw);

                            if(draw < 0.8)
                            {
                                child.GetComponent<Kolos>().rage += 20;
                                Debug.Log("Enraging Kolos");
                            }
                        }
                    }*/
                }

                if (CrossPlatformInputManager.GetButtonDown("Fire2") && gameObject.GetComponentInChildren<ALSpawmer>().burnZinc)
                {
                    float draw = UnityEngine.Random.Range(0f, 1f);
                    //Debug.Log("Drawing " + draw);


                    found = false;
                    innerCount = 0;

                    //Look for this child in mouseRayHits 
                    do
                    {

                        RaycastHit2D hit = mouseRayHits[innerCount];

                        //Get the gameObject attached to the collider hitted by the ray
                        hitted = hit.collider.gameObject;

                        //If the gameobject hitted is meant for UI
                        if (hitted.layer == 17)
                        {
                            if (draw < 0.8)
                            {
                                hitted.GetComponent<EmotionalUI>().EnrageKolo(true);
                            }
                        }
                        innerCount++;
                        if (found)
                            break;
                    } while (innerCount < (mouseRayHits.Length));
                }

                if (CrossPlatformInputManager.GetButtonDown("Fire1") && gameObject.GetComponentInChildren<ALSpawmer>().burnBrass)
                {
                    float draw = UnityEngine.Random.Range(0f, 1f);
                    //Debug.Log("Drawing " + draw);


                    found = false;
                    innerCount = 0;

                    //Look for this child in mouseRayHits 
                    do
                    {

                        RaycastHit2D hit = mouseRayHits[innerCount];

                        //Get the gameObject attached to the collider hitted by the ray
                        hitted = hit.collider.gameObject;

                        //If the gameobject hitted is meant for UI
                        if (hitted.layer == 17)
                        {
                            if (draw < 0.8)
                            {
                                hitted.GetComponent<EmotionalUI>().EnrageKolo(false);
                            }
                        }
                        innerCount++;
                        if (found)
                            break;
                    } while (innerCount < (mouseRayHits.Length));
                }
            }

            if (CrossPlatformInputManager.GetButtonDown("Attack"))
            {
                currentWeapon.Attack1();
            }

            if (CrossPlatformInputManager.GetButton("Aim"))
            {
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                    GetComponentInChildren<ALSpawmer>().GetComponentInChildren<ALSpawmer>().Aim((Camera.main.ScreenPointToRay(Input.mousePosition).origin) - this.transform.position);
                }

            }

            if (coinShot)
            {
                if (CrossPlatformInputManager.GetButtonDown("Steel"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningSteel();
                }
            }

            if (lurcher)
            {
                if (CrossPlatformInputManager.GetButtonDown("Iron"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningIron();
                }
            }

            if (pewterArm)
            {
                if (CrossPlatformInputManager.GetButtonDown("Pewter"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningPewter();
                }
            }

            if (tinEye)
            {
                if (CrossPlatformInputManager.GetButtonDown("Tin"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningTin();
                }
            }

            if (rioter)
            {
                if (CrossPlatformInputManager.GetButtonDown("Zinc"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningZinc();
                }
            }

            if (soother)
            {
                if (CrossPlatformInputManager.GetButtonDown("Brass"))
                {
                    GetComponentInChildren<ALSpawmer>().BurningBrass();
                }
            }


            if (CrossPlatformInputManager.GetButtonDown("Loot"))
            {
                List<int> itemIndexes = new List<int>();

                foreach (LootBehaviour current in m_Character.GetComponent<Mistborn>().closeLoot)
                {
                    if (current.looted)
                    {
                        itemIndexes.Add(m_Character.GetComponent<Mistborn>().closeLoot.IndexOf(current));
                    }
                }

                foreach (int index in itemIndexes)
                {
                    m_Character.GetComponent<Mistborn>().closeLoot.RemoveAt(index);
                }

                if (m_Character.GetComponent<Mistborn>().closeLoot.Count > 0)
                {
                    
                    if (m_Character.GetComponent<Mistborn>().closeLoot[0].lootType == 0)
                    {

                        m_Character.GetComponentInChildren<ALSpawmer>().coinPouch += Mathf.FloorToInt(m_Character.GetComponent<Mistborn>().closeLoot[0].effectValue);
                        m_Character.GetComponent<Mistborn>().closeLoot[0].Looting();
                        m_Character.GetComponent<Mistborn>().closeLoot[0].looted = true;
                        m_Character.GetComponent<Mistborn>().closeLoot.RemoveAt(0);

                    }
                    else
                    {
                        /*if (invIndex <= 7)
                        {*/

                        bool looted = false;
                        //int i = 0;

                        looted = AddInvItem(m_Character.GetComponent<Mistborn>().closeLoot[0]);

                        if (looted)
                        {
                            m_Character.GetComponent<Mistborn>().closeLoot[0].Looting();
                            m_Character.GetComponent<Mistborn>().closeLoot.RemoveAt(0);
                        }
                        else
                        {
                            Debug.Log("Inventory full");
                        }

                        /*while (!looted && (i <= 7))
                        {
                            if (inventory[i] == null)
                            {
                                LootBehaviour currentLoot = ;
                                Lootable newInvItem = new Lootable(currentLoot);

                                inventory[i] = newInvItem;
                                slots[i].sprite = m_Character.GetComponent<Mistborn>().closeLoot[0].GetComponent<SpriteRenderer>().sprite;
                                slots[i].enabled = true;


                                looted = true;
                                invIndex++;

                            }

                            i++;
                        }


                    }
                    else
                        Debug.Log("Inventory full");*/
                    }
                }

                if(m_Character.GetComponent<Mistborn>().closeSearch.Count > 0)
                {
                    if (!m_Character.GetComponent<Mistborn>().closeSearch[0].GetDropped())
                    {
                        m_Character.GetComponent<Mistborn>().closeSearch[0].Searching();
                        //m_Character.GetComponent<Mistborn>().closeSearch.RemoveAt(0);
                    }
                }

                if(m_Character.GetComponent<Mistborn>().closeDoors.Count > 0)
                {
                    if (m_Character.GetComponent<Mistborn>().closeDoors[0].locked)
                    {
                        bool key = false;
                        int pos = 0;

                        for(int i = 0; i <= 7; i++)
                        {
                            if (inventory[i] != null)
                            {
                                if (inventory[i].lootType == m_Character.GetComponent<Mistborn>().closeDoors[0].keyType)
                                {
                                    key = true;
                                    pos = i;
                                }
                            }
                        }

                        if (key)
                        {
                            m_Character.GetComponent<Mistborn>().closeDoors[0].locked = false;
                            RemoveInvItem(pos);

                            List<string> doorUMessage = new List<string>();

                            doorUMessage.Add("The door is unlocked");
                            NewMessage(doorUMessage);

                            Debug.Log("The door is now unlocked");
                        }
                        else
                        {
                            List<string> doorLMessage = new List<string>();

                            doorLMessage.Add("The door is locked");
                            NewMessage(doorLMessage);

                            Debug.Log("The door is locked");
                        }

                    }
                    else
                    {
                        m_Character.GetComponent<Mistborn>().closeDoors[0].OpenDoor();
                    }
                }
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv1"))
            {
                UseItem(1);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv2"))
            {
                UseItem(2);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv3"))
            {
                UseItem(3);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv4"))
            {
                UseItem(4);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv5"))
            {
                UseItem(5);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv6"))
            {
                UseItem(6);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv7"))
            {
                UseItem(7);
            }

            if (CrossPlatformInputManager.GetButtonDown("Inv8"))
            {
                UseItem(8);
            }


            
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //Debug.Log("Horizontal: " + CrossPlatformInputManager.GetAxis("Horizontal"));
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }

        public void UseItem(int index)
        {
            if (inventory[index - 1] != null)
            {
                switch (inventory[index - 1].lootType)
                {
                    //Steel Vial
                    case 1:
                        m_Character.GetComponentInChildren<ALSpawmer>().steelRes += inventory[index - 1].effectValue;

                        List<string> steelVMessage = new List<string>();

                        steelVMessage.Add("Gulp, gulp \n Steel reserve increased");
                        NewMessage(steelVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 2:
                        m_Character.GetComponentInChildren<ALSpawmer>().ironRes += inventory[index - 1].effectValue;

                        List<string> ironVMessage = new List<string>();

                        ironVMessage.Add("Gulp, gulp \n Iron reserve increased");
                        NewMessage(ironVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    //HP Vial
                    case 3:
                        m_Character.CureHp(inventory[index - 1].effectValue);

                        List<string> hpVMessage = new List<string>();

                        hpVMessage.Add("Gulp, gulp \n Recovered health points");
                        NewMessage(hpVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 4:
                        //Debug.Log("I should use this to open a door");
                        List<string> keyMessage = new List<string>();
                        keyMessage.Add("I should use this \n to open a door");
                        NewMessage(keyMessage);
                        break;

                    case 5:
                        if (selectedEquipment != index - 1)
                        {
                            selectedEquipment = index - 1;
                            //Debug.Log("Iron Spike Selected");

                            List<string> ironSMessage = new List<string>();

                            ironSMessage.Add("If I spike myself with this \n I'll be a lurcher");
                            ironSMessage.Add("Click a slot on \n the hemalurgic map");
                            NewMessage(ironSMessage);
                        }
                        else
                        {
                            selectedEquipment = -1;
                            Debug.Log("Iron Deselected");
                        }
                        break;

                    case 6:
                        if (selectedEquipment != index - 1)
                        {
                            selectedEquipment = index - 1;
                            //Debug.Log("Pewter Spike Selected");

                            List<string> pewterSMessage = new List<string>();

                            pewterSMessage.Add("If I spike myself with this \n I'll be a Pewter Arm");
                            pewterSMessage.Add("Click a slot on \n the hemalurgic map");
                            NewMessage(pewterSMessage);
                        }
                        else
                        {
                            selectedEquipment = -1;
                            Debug.Log("pewter  Deselected");
                        }
                        break;

                    case 7:
                        if (selectedEquipment != index - 1)
                        {
                            selectedEquipment = index - 1;
                            //Debug.Log("Tin Spike Selected");

                            List<string> tinSMessage = new List<string>();

                            tinSMessage.Add("If I spike myself with this \n I'll be a Tin Eye");
                            tinSMessage.Add("Click a slot on \n the hemalurgic map");
                            NewMessage(tinSMessage);
                        }
                        else
                        {
                            selectedEquipment = -1;
                            Debug.Log("Tin  Deselected");
                        }
                        break;

                    case 8:
                        if (selectedEquipment != index - 1)
                        {
                            selectedEquipment = index - 1;
                            //Debug.Log("Zinc Spike Selected");

                            List<string> zincSMessage = new List<string>();

                            zincSMessage.Add("If I spike myself with this \n I'll be a Rioter");
                            zincSMessage.Add("Click a slot on \n the hemalurgic map");
                            NewMessage(zincSMessage);
                        }
                        else
                        {
                            selectedEquipment = -1;
                            Debug.Log("Zinc  Deselected");
                        }
                        break;

                    case 9:
                        if (selectedEquipment != index - 1)
                        {
                            selectedEquipment = index - 1;
                            //Debug.Log("Brass Spike Selected");

                            List<string> brassSMessage = new List<string>();

                            brassSMessage.Add("If I spike myself with this \n I'll be a Soother");
                            brassSMessage.Add("Click a slot on \n the hemalurgic map");
                            NewMessage(brassSMessage);
                        }
                        else
                        {
                            selectedEquipment = -1;
                            Debug.Log("Brass Deselected");
                        }
                        break;

                    case 10:
                        List<string> coalOMessage = new List<string>();

                        coalOMessage.Add("I can use this coal ore \n to make steel flakes");
                        coalOMessage.Add("I should find a place to store it");
                        NewMessage(coalOMessage);

                        break;

                    case 11:
                        List<string> ironOMessage = new List<string>();

                        ironOMessage.Add("I can use this iron ore \n to make iron flakes");
                        ironOMessage.Add("I should find a place to store it");
                        NewMessage(ironOMessage);

                        break;

                    case 12:
                        List<string> tinOMessage = new List<string>();

                        tinOMessage.Add("I can use this tin ore \n to make tin flakes");
                        tinOMessage.Add("I should find a place to store it");
                        NewMessage(tinOMessage);

                        break;

                    case 13:
                        List<string> leadOMessage = new List<string>();

                        leadOMessage.Add("I can use this lead ore \n to make pewter flakes");
                        leadOMessage.Add("I should find a place to store it");
                        NewMessage(leadOMessage);

                        break;

                    case 14:
                        List<string> zincOMessage = new List<string>();

                        zincOMessage.Add("I can use this zinc ore \n to make zinc flakes");
                        zincOMessage.Add("I should find a place to store it");
                        NewMessage(zincOMessage);

                        break;

                    case 15:
                        List<string> copperOMessage = new List<string>();

                        copperOMessage.Add("I can use this copper ore \n to make brass flakes");
                        copperOMessage.Add("I should find a place to store it");
                        NewMessage(copperOMessage);

                        break;

                    case 16:
                        m_Character.GetComponentInChildren<ALSpawmer>().tinRes += inventory[index - 1].effectValue;

                        List<string> tinVMessage = new List<string>();

                        tinVMessage.Add("Gulp, gulp \n Tin reserve increased");
                        NewMessage(tinVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 17:
                        m_Character.GetComponentInChildren<ALSpawmer>().pewterRes += inventory[index - 1].effectValue;

                        List<string> pewterVMessage = new List<string>();

                        pewterVMessage.Add("Gulp, gulp \n pewter reserve increased");
                        NewMessage(pewterVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 18:
                        m_Character.GetComponentInChildren<ALSpawmer>().zincRes += inventory[index - 1].effectValue;

                        List<string> zincVMessage = new List<string>();

                        zincVMessage.Add("Gulp, gulp \n Zinc reserve increased");
                        NewMessage(zincVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 19:
                        m_Character.GetComponentInChildren<ALSpawmer>().brassRes += inventory[index - 1].effectValue;

                        List<string> brassVMessage = new List<string>();

                        brassVMessage.Add("Gulp, gulp \n Brass reserve increased");
                        NewMessage(brassVMessage);

                        RemoveInvItem(index - 1);

                        break;

                    case 20:
                        m_Character.GetComponentInChildren<ALSpawmer>().brassRes += inventory[index - 1].effectValue;
                        m_Character.GetComponentInChildren<ALSpawmer>().zincRes += inventory[index - 1].effectValue;
                        m_Character.GetComponentInChildren<ALSpawmer>().tinRes += inventory[index - 1].effectValue;
                        m_Character.GetComponentInChildren<ALSpawmer>().pewterRes += inventory[index - 1].effectValue;
                        m_Character.GetComponentInChildren<ALSpawmer>().ironRes += inventory[index - 1].effectValue;
                        m_Character.GetComponentInChildren<ALSpawmer>().steelRes += inventory[index - 1].effectValue;

                        List<string> mixedVMessage = new List<string>();

                        mixedVMessage.Add("Gulp, gulp \n All metal reserves increased");
                        NewMessage(mixedVMessage);

                        RemoveInvItem(index - 1);

                        break;


                }
            }
            //else
                //Debug.Log("Nothing there");
        }

        public bool AddInvItem(LootBehaviour item)
        {
            bool looted = false;
            int i = 0;
            while (!looted && (i <= 7))
            {
                if (inventory[i] == null)
                {
                    Lootable newInvItem = new Lootable(item);

                    inventory[i] = newInvItem;
                    slots[i].sprite = m_Character.GetComponent<Mistborn>().closeLoot[0].GetComponent<SpriteRenderer>().sprite;
                    slots[i].enabled = true;
                    looted = true;
                    invIndex++;

                }

                i++;
            }
            return looted;
        }

        public Lootable CheckInvItem(int index)
        {

            return inventory[index];
        }

        public int CheckInvIndex()
        {
            return invIndex;
        }

        public bool AddInvItem(Lootable item)
        {
            bool looted = false;
            int i = 0;
            while (!looted && (i <= 7))
            {
                if (inventory[i] == null)
                {
                    Lootable newInvItem = new Lootable(item);

                    inventory[i] = newInvItem;
                    slots[i].sprite = item.icon;
                    slots[i].enabled = true;
                    looted = true;
                    invIndex++;

                }

                i++;
            }
            return looted;
        }

        public void RemoveInvItem(int index)
        {
            slots[index].enabled = false;
            inventory[index] = null;
            invIndex--;
        }

        public void UpdateHP()
        {
            if (m_Character.GetHp() > 0)
            {
                healthBar.transform.localScale = new Vector3(healthScale.x * (m_Character.GetHp() / m_Character.GetMaxHp()), 1, 1);
            }
            else
            {
                healthBar.transform.localScale = new Vector3(0, 1, 1);
            }
        }

        public void EquipItem(int slot)
        {
            if (selectedEquipment >= 0)
            {
                if (equipment[slot] != null)
                {
                    Lootable selected = inventory[selectedEquipment];

                    RemoveInvItem(selectedEquipment);
                    AddInvItem(equipment[slot]);
                    //inventory[selectedEquipment] = equipment[slot];

                    switch (equipment[slot].lootType)
                    {
                        case 5:
                            if (m_Character.GetComponentInChildren<ALSpawmer>().burnIron)
                            {
                                m_Character.GetComponentInChildren<ALSpawmer>().BurningIron();
                            }

                            lurcher = false;
                            metalTimers[1].SetActive(false);
                            break;

                        case 6:
                            if (m_Character.GetComponentInChildren<ALSpawmer>().burnPewter)
                            {
                                m_Character.GetComponentInChildren<ALSpawmer>().BurningPewter();
                            }

                            pewterArm = false;
                            metalTimers[2].SetActive(false);
                            break;

                        case 7:
                            if (m_Character.GetComponentInChildren<ALSpawmer>().burnTin)
                            {
                                m_Character.GetComponentInChildren<ALSpawmer>().BurningTin();
                            }

                            tinEye = false;
                            metalTimers[3].SetActive(false);
                            break;

                        case 8:
                            if (m_Character.GetComponentInChildren<ALSpawmer>().burnZinc)
                            {
                                m_Character.GetComponentInChildren<ALSpawmer>().BurningZinc();
                            }

                            rioter = false;
                            metalTimers[4].SetActive(false);
                            break;

                        case 9:
                            if (m_Character.GetComponentInChildren<ALSpawmer>().burnBrass)
                            {
                                m_Character.GetComponentInChildren<ALSpawmer>().BurningBrass();
                            }

                            soother = false;
                            metalTimers[5].SetActive(false);
                            break;
                    }

                    //add the visuals here
                    
                    equipment[slot] = selected;
                    slotIcons[slot].sprite = equipment[slot].icon;

                    switch (equipment[slot].lootType)
                    {
                        case 5:
                            lurcher = true;
                            metalTimers[1].SetActive(true);
                            break;

                        case 6:
                            pewterArm = true;
                            metalTimers[2].SetActive(true);
                            break;

                        case 7:
                            tinEye = true;
                            metalTimers[3].SetActive(true);
                            break;

                        case 8:
                            rioter = true;
                            metalTimers[4].SetActive(true);
                            break;

                        case 9:
                            soother = true;
                            metalTimers[5].SetActive(true);
                            break;
                    }

                    selectedEquipment = -1;
                }
                else
                {
                    Lootable selected = inventory[selectedEquipment];
                    RemoveInvItem(selectedEquipment);
                    equipment[slot] = selected;
                    slotIcons[slot].enabled = true;
                    slotIcons[slot].sprite = equipment[slot].icon;

                    switch (equipment[slot].lootType)
                    {
                        case 5:
                            lurcher = true;
                            metalTimers[1].SetActive(true);
                            break;

                        case 6:
                            pewterArm = true;
                            metalTimers[2].SetActive(true);
                            break;

                        case 7:
                            tinEye = true;
                            metalTimers[3].SetActive(true);
                            break;

                        case 8:
                            rioter = true;
                            metalTimers[4].SetActive(true);
                            break;

                        case 9:
                            soother = true;
                            metalTimers[5].SetActive(true);
                            break;
                    }

                    selectedEquipment = -1;

                }
            }
        }

        public void NewMessage(List<string> message)
        {
            messageBox.SetActive(true);
            messageBox.GetComponent<MessageBox>().DisplayMessage(message);
        }

    }
}
