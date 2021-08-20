using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class GameManager : MonoBehaviour
{
    //Character control
    public PlatformerCharacter2D player;
    //public bool burningAtium = true;

    /*
    public Text win;
    public Text home;
    public bool winner;
    public GameObject key;
    */

    public bool winner = false;
    public GameObject menu;
    public GameObject pauseButton;
    public GameObject unpauseButton;
    public StoryMessage storyMessageBox;
    public GameObject caveBlock;
    public bedroom bed;
    public Transform respawnPoint;
    public GameObject koloPrefab;
    public Transform spawmPoint;
    public int spawmIndex;
    public AudioClip finalSong;
    public AudioSource audioPlayer;
    public List<GameObject> enviromentalMessages;
    public BoxCollider2D houseBackDoor;

    public List<GameObject> deleteButtons;

    public List<Kolos> initialKolos;
    public List<Kolos> spawmedKolos;
    public List<GameObject> spawmHomes;

    private float spawmTimer = 0;

    /*private void Start()
    {
        win.text = "";
        home.text = "";
    }*/

    private void Start()
    {
        List<string> message = new List<string>();
        message.Add("Screams in the background...\n Ali: This can't be happening \n You hear loud thumps and metallic noises \n Ali gets a strong impulse to hide in the hovel");
        message.Add("Then, she feels a sense of encouragement \n almost like a whisper, urging her forward. \n Ali: Take a deep breath... \n First, get the pewter spike \n deal with the emergency ");
        message.Add("Ali: then I'll visit the caverns \n Use 'R' to interact with and loot items \n search the chest and get the spike");

        storyMessageBox.DisplayMessage(message);
    }

    void Update()
    {

        List<int> kolosIndexes = new List<int>();

        foreach(Kolos current in initialKolos)
        {
            if (!current.alive || current.tag == "Enraged")
                kolosIndexes.Add(initialKolos.IndexOf(current));
        }

        foreach(int i in kolosIndexes)
        {
            initialKolos.RemoveAt(i);
        }

        if (!winner)
        {
            if (initialKolos.Count == 0)
            {
                caveBlock.SetActive(false);
                bed.won = true;
                player.GetComponent<Mistborn>().won = true;
                NewStoryMessage(3);
                audioPlayer.clip = finalSong;
                winner = true;
                houseBackDoor.enabled = false;

                foreach(GameObject current in enviromentalMessages)
                {
                    current.SetActive(false);
                }

            }
        }
        else
        {
            if (spawmedKolos.Count < 4)
            {
                spawmTimer += Time.deltaTime;
                if(spawmTimer > 10)
                {
                    SpawmKolo();
                    spawmTimer = 0;
                }
            }

            List<int> sKolosIndexes = new List<int>();

            foreach (Kolos current in spawmedKolos)
            {
                if (!current.alive || current.tag == "Enraged")
                    sKolosIndexes.Add(spawmedKolos.IndexOf(current));
            }

            foreach (int i in sKolosIndexes)
            {
                spawmedKolos.RemoveAt(i);
            }

        }

        if (CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            PauseGame();
        }

        

        /*if (winner)
            StartCoroutine(Win());
            */

        /*if (!player.alive)
        {
            StartCoroutine(Defeat());
        }*/
    }

    /*public IEnumerator Win()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;
        menu.SetActive(true);
        win.text = "Victory";

        home.text = "Sweet, I found the key now go rescue my baby brother. Please support the developer so he can make the next map...";

    }*/

    public IEnumerator Defeat()
    {

        yield return new WaitForSeconds(1f);
        //Debug.Log("U DEAD");
        //Time.timeScale = 0f;
        
        /*menu.SetActive(true);
        win.text = "Defeat";

        home.text = "Ouchh... better luck next time";*/

    }

    public void PauseGame()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            pauseButton.SetActive(true);
            unpauseButton.SetActive(false);

            foreach(GameObject current in deleteButtons)
            {
                current.SetActive(false);
            }

            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0f;

            foreach (GameObject current in deleteButtons)
            {
                current.SetActive(true);
            }

            menu.SetActive(true);
            pauseButton.SetActive(false);
            unpauseButton.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        player.GetComponent<Mistborn>().TriggerRespawn();
        player.transform.position = respawnPoint.position;
    }

    public void NewStoryMessage(int index)
    {
        List<string> currentStory = new List<string>();

        storyMessageBox.gameObject.SetActive(true);

        switch (index)
        {
            case 0:
                currentStory.Add("By Preservation's knife! \n Those are Koloss, they shouldn't be here... \n I better burn pewter to fight it");
                currentStory.Add("After equipping the pewter spike \n you can burn pewter by pressing 'V' \n while burning pewter you'll be stronger");
                currentStory.Add("Just be careful with your reserves \n pewter burns away quickly \n you can stop burning by pressing 'V' again");
                break;

            case 1:
                currentStory.Add("I don't have the key to the front door \n I should try the balcony \n I can steel jump there pushing on a coin");
                currentStory.Add("To drop a coin hold 'F'\n Aim with your mouse and left-click \n then you'll have to steel push it");
                currentStory.Add("To burn steel press 'Z' \n blue lines will point from you \n to all nearby sources of metal \n Click on a line to push");
                currentStory.Add("In allomancy every action has a reaction \n Push on coin and it will fly away \n but if that coin has no way to go \n you'll be pushed in the oposite direction");
                break;

            case 2:
                currentStory.Add("That's a Zinc spike! \n I can use emotional allomancy to make the koloss \n fight each other");
                currentStory.Add("You can burn Zinc after equipping the spike \n To burn Zinc press 'B' then approach a koloss \n You can push and pull on a Koloss emotions \n by clicking the right symbols above its head");
                currentStory.Add("It won't have any effect at first \n after you fill a Koloss emotional bar \n it won't attack you, but other Koloss \n Use your Zinc to make the Koloss \n above fight each other");
                break;

            case 3:
                currentStory.Add("Ali: Finally! the koloss are dealt with \n Now I need to enter the caverns \n the hemalurgic spikes there will help \n Maybe I should take a nap before that...");
                currentStory.Add("Sleeping will recover your HP \n if you die you'll respawn there \n you also unlocked the caverns \n go and claim the Hemalurgic Spikes \n left there by your ancestors");
                currentStory.Add("Beware, you dealt with the invading koloss \n However, more are bound to come");
                break;

            case 4:
                currentStory.Add("Great, drink the vials to get some reserves. \n You can see your metal reserves on the \n lower left corner of your screen \n Get out of the hovel and head to the mannor");
                currentStory.Add("Controls\n Move: A,W,S,D \n Jump: Spacebar \n Attack: E \n Crouch: Crt");
                break;
        }

        storyMessageBox.DisplayMessage(currentStory);
    }

    public void SpawmKolo()
    {
        GameObject currentKolo = Instantiate(koloPrefab, spawmPoint);
        Kolos currentKoloBeh = currentKolo.GetComponent<Kolos>();

        spawmedKolos.Add(currentKoloBeh);

        if(spawmIndex < spawmHomes.Count)
        {
            currentKoloBeh.patrolBase = spawmHomes[spawmIndex].transform;
            spawmIndex++;

        }
        else
        {
            currentKoloBeh.patrolBase = spawmHomes[0].transform;
            spawmIndex = 1;
        }
        

    }

    public void FacebookLink()
    {
        Application.OpenURL("https://www.facebook.com/ChildrenofPreservation");
    }

    public void ItchLink()
    {
        Application.OpenURL("https://dnxgaming.itch.io/the-children-of-preservation");
    }

    /*public bool AtiumOn()
    {
        return burningAtium;
    }*/


}
