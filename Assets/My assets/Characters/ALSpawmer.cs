using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.UI;

public class ALSpawmer : MonoBehaviour
{
    public Transform aLMlinePF;
    public Rigidbody2D coinPF;
    public int coinPouch;
    public int allomanticForce;
    public Vector3 mousePos;

    //Metals
    public float steelRes;
    public bool burnSteel;

    public float ironRes;
    public bool burnIron;

    public float pewterRes;
    public bool burnPewter;

    public float tinRes;
    public bool burnTin;

    public float zincRes;
    public bool burnZinc;

    public float brassRes;
    public bool burnBrass;

    public Camera mainCamera;

    public GameObject fireS;
    public GameObject fireI;
    public GameObject fireP;
    public GameObject fireT;
    public GameObject fireZ;
    public GameObject fireB;

    private List<EmotionalUI> emotionalInRange = new List<EmotionalUI>();


    // Fix so the lines and metals are lists rather than arrays
    private List<Transform> lines;
    private List<Rigidbody2D> metals;
    ALControler lineController;

    

    //UI
    public Text coinPouchCount;
    public Text steelDisplay;
    public Text ironDisplay;
    public Text pewterDisplay;
    public Text tinDisplay;
    public Text zincDisplay;
    public Text brassDisplay;

    private void Start()
    {
        lines = new List<Transform>();
        metals = new List<Rigidbody2D>();
    }

    private void Update()
    {
        coinPouchCount.text = coinPouch.ToString();

        steelDisplay.text = Mathf.RoundToInt(steelRes).ToString();
        ironDisplay.text = Mathf.RoundToInt(ironRes).ToString();
        pewterDisplay.text = Mathf.RoundToInt(pewterRes).ToString();
        tinDisplay.text = Mathf.RoundToInt(tinRes).ToString();
        zincDisplay.text = Mathf.RoundToInt(zincRes).ToString();
        brassDisplay.text = Mathf.RoundToInt(brassRes).ToString();

        if (burnSteel)
            steelRes -= Time.deltaTime;

        if (burnIron)
            ironRes -= Time.deltaTime;

        if (burnPewter)
            pewterRes -= Time.deltaTime * 2;

        if (burnTin)
            tinRes -= Time.deltaTime;

        if (burnZinc)
            zincRes -= Time.deltaTime;

        if (burnBrass)
            brassRes -= Time.deltaTime;



        if (steelRes <= 0 && burnSteel)
            BurningSteel();

        if (ironRes <= 0 && burnIron)
            BurningIron();

        if (pewterRes <= 0 && burnPewter)
            BurningPewter();


        if (tinRes <= 0 && burnTin)
            BurningTin();

        if (zincRes <= 0 && burnZinc)
            BurningZinc();

        if (brassRes <= 0 && burnBrass)
            BurningBrass();

        List<int> emotionalindexes = new List<int>();

        foreach(EmotionalUI current in emotionalInRange)
        {
            if (!current.GetComponentInParent<Kolos>().alive)
            {
                emotionalindexes.Add(emotionalInRange.IndexOf(current));
            }
        }

        foreach(int index in emotionalindexes)
        {
            if(index < emotionalInRange.Count)
                emotionalInRange.RemoveAt(index);
        }


    }

    void OnTriggerEnter2D(Collider2D metalCol)
    {

        //if the game object is metal 
        if (metalCol.gameObject.layer == 9 || (metalCol.gameObject.layer == 10))
        {
            Transform aLMLineInstance;
            //bool lineMade = false;
            //int arrayPos = -1;

            //Instanciate the line to the new metal
            aLMLineInstance = Instantiate(aLMlinePF, this.transform, false);

            //Reference the line controller scipt, send it the rigidbody of the target metal and if the character is flipped
            lineController = aLMLineInstance.gameObject.GetComponent<ALControler>();
            lineController.attachedMetal = metalCol.attachedRigidbody;
            //C: getcomponent in parent
            //Try to fix this so m_facingright can be protected
            lineController.facingRight = this.GetComponentInParent<PlatformerCharacter2D>().GetFacingRight();

            //Find the first empty space in the in metals, then fill it with the current line and metal  

            aLMLineInstance.name = "aLMLine to: " + metalCol.name + lines.Count;
            lines.Add(aLMLineInstance);
            metals.Add(metalCol.attachedRigidbody);

            //Debug.Log("Adding AL, positions line: " + lines.FindIndex(x => x == aLMLineInstance) + " metal: " + metals.FindIndex(x => x == metalCol.attachedRigidbody));

            /*do
            {
                arrayPos++;

                if (metals[arrayPos] == null)
                {
                    aLMLineInstance.name = "aLMLine to: " + metalCol.name + arrayPos;
                    lines[arrayPos] = aLMLineInstance;
                    metals[arrayPos] = metalCol.attachedRigidbody;
                    lineMade = true;
                }

            } while (!lineMade && arrayPos < metals.Length);*/

            //Name the line

        }

        if(metalCol.gameObject.layer == 17)
        {
            if (!(!burnBrass && !burnZinc))
                metalCol.GetComponent<EmotionalUI>().Activate();

            if (!emotionalInRange.Contains(metalCol.GetComponent<EmotionalUI>()))
                emotionalInRange.Add(metalCol.GetComponent<EmotionalUI>());
        }
    }

    //An object is leaving the allomantic range
    private void OnTriggerExit2D(Collider2D metalCollision)
    {
        if ((metalCollision.gameObject.layer == 9) || (metalCollision.gameObject.layer == 10))
        {
            int pos;

            Rigidbody2D metal = metalCollision.GetComponent<Rigidbody2D>();

            pos = metals.FindIndex(y => y == metal);

            //Debug.Log("Destroying line to metal called: " + metalCollision.name + " metal name: " + metal.name);

            DestroyLine(lines[pos]);



            /*bool x = false;
            int count = 0;

            //Find the target metal in the metals list;

            

            //Debug.LogWarning("metals lenght: " + metals.Length);

            while (!x && count < metals.Length)
            {
                if (lines[count] != null)
                {
                    x = (metalCollision.transform == metals[count].transform);
                }
                count++;
            }

            //Using the same count destroy the line and clean the lists
            if (x)
            {
                DestroyLine(lines[count - 1]);
            }
            else
            {
                Debug.LogWarning("not found");
            }*/

            

        }

        if (metalCollision.gameObject.layer == 17)
        {
            metalCollision.GetComponent<EmotionalUI>().Deactivate();

            emotionalInRange.Remove(metalCollision.GetComponent<EmotionalUI>());
        }
    }

    /*public IEnumerator shootCoin(Vector3 mousePos)
    {
        if (!shooting && (coinPouch > 0))
        {
            if (this.GetComponentInParent<PlatformerCharacter2D>().alive)
            {
                shooting = true;
                Debug.Log("shooting coin");
                //Fix so m_anim can be private or protected
                GetComponentInParent<PlatformerCharacter2D>().m_Anim.SetTrigger("Shooting");
                //This needs to be synced with the animator
                yield return new WaitForSeconds(0.5f);
                Rigidbody2D coin = Instantiate(coinPF);
                if (GetComponentInParent<PlatformerCharacter2D>().m_FacingRight)
                {
                    coin.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, 0);
                }
                else
                {
                    coin.transform.position = new Vector3(transform.position.x - 1f, transform.position.y, 0);
                }
                //force add should depend on base force
                coin.AddForce(mousePos * 5, ForceMode2D.Impulse);

                yield return new WaitForSeconds(0.5f);
                shooting = false;
                coinPouch--;
            }

        }
    }*/

    public void Aim(Vector3 newMousePos)
    {
        Mistborn player = GetComponentInParent<Mistborn>();
        mousePos = newMousePos;

        if ((!player.GetShooting() && (coinPouch > 0)) && player.alive)
        {
            player.SetShooting(true);
        }
    }

    public void Shoot()
    {
        Rigidbody2D coin = Instantiate(coinPF);
        if (GetComponentInParent<PlatformerCharacter2D>().GetFacingRight())
        {
            coin.transform.position = new Vector3(transform.position.x + 3f, transform.position.y + 2, 0);
        }
        else
        {
            coin.transform.position = new Vector3(transform.position.x - 4f, transform.position.y + 2, 0);
        }
        //force add should depend on base force
        coin.AddForce(mousePos * 5, ForceMode2D.Impulse);

        coinPouch--;
    }

    public void BurningSteel()
    {

        if (burnSteel)
        {
            burnSteel = false;
            fireS.GetComponent<Image>().enabled = false;
        }
        else
        {
            if (steelRes > 0)
            {
                fireS.GetComponent<Image>().enabled = true;
                burnSteel = true;
            }
        }

    }

    public void BurningIron()
    {
        if (burnIron)
        {
            burnIron = false;
            fireI.GetComponent<Image>().enabled = false;
        }
        else
        {
            if(ironRes > 0)
            {
                burnIron = true;
                fireI.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void BurningPewter()
    {
        if (burnPewter)
        {
            GetComponentInParent<Mistborn>().Pewter(false);
            fireP.GetComponent<Image>().enabled = false;
            burnPewter = false;
        }
        else
        {
            if (pewterRes > 0)
            {
                GetComponentInParent<Mistborn>().Pewter(true);
                fireP.GetComponent<Image>().enabled = true;
                burnPewter = true;
            }
        }
    }

    public void BurningTin()
    {
        if (burnTin)
        {
            mainCamera.orthographicSize = 10;
            fireT.GetComponent<Image>().enabled = false;
            burnTin = false;
        }
        else
        {
            if (tinRes > 0)
            {
                mainCamera.orthographicSize = 15;
                fireT.GetComponent<Image>().enabled = true;
                burnTin = true;
            }
        }
    }

    public void BurningZinc()
    {
        if (burnZinc)
        {
            burnZinc = false;
            fireZ.GetComponent<Image>().enabled = false;
            if (!burnBrass)
            {
                foreach(EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Deactivate();
                }
            }
        }
        else
        {
            if (zincRes > 0)
            {
                burnZinc = true;
                fireZ.GetComponent<Image>().enabled = true;

                foreach(EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Activate();
                }

            }
        }
    }

    public void BurningBrass()
    {
        if (burnBrass)
        {
            burnBrass = false;
            fireB.GetComponent<Image>().enabled = false;
            if (!burnZinc)
            {
                foreach (EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Deactivate();
                }
            }
        }
        else
        {
            if (brassRes > 0)
            {
                burnBrass = true;
                fireB.GetComponent<Image>().enabled = true;
                foreach (EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Activate();
                }
            }
        }
    }

    public void MetalsOff()
    {
        if (burnSteel)
        {
            burnSteel = false;
            fireS.GetComponent<Image>().enabled = false;
        }
        if (burnIron)
        {
            burnIron = false;
            fireI.GetComponent<Image>().enabled = false;
        }

        if (burnPewter)
        {
            GetComponentInParent<Mistborn>().Pewter(false);
            fireP.GetComponent<Image>().enabled = false;
            burnPewter = false;
        }

        if (burnTin)
        {
            mainCamera.orthographicSize = 10;
            fireT.GetComponent<Image>().enabled = false;
            burnTin = false;
        }

        if (burnZinc)
        {
            burnZinc = false;
            fireZ.GetComponent<Image>().enabled = false;
            if (!burnBrass)
            {
                foreach (EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Deactivate();
                }
            }
        }

        if (burnBrass)
        {
            burnBrass = false;
            fireB.GetComponent<Image>().enabled = false;
            if (!burnZinc)
            {
                foreach (EmotionalUI currentEUI in emotionalInRange)
                {
                    currentEUI.Deactivate();
                }
            }
        }
    }


    public void DestroyLine(Transform line)
    {

        
        metals.RemoveAt(lines.FindIndex(y => y == line));
        lines.Remove(line);
        Destroy(line.gameObject);
        

        /*bool x = false;
        int metalPos = 0;

        //Find the target metal in the metals list;
        while (!x && metalPos < metals.Length)
        {
            if (lines[metalPos] != null)
            {
                x = (line.name == lines[metalPos].name);
            }
            metalPos++;
        }

        if (x)
        {
            metals[metalPos - 1] = null;
            lines[metalPos - 1] = null;
        }
        /*else
        {
            Debug.LogError("the line was not found");
        }
        
     */
    }

}



