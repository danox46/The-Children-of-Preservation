using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionalUI : MonoBehaviour
{
    public Sprite brassSymbol;
    public Sprite zincSymbol;
    private bool zinc;

    private Vector3 rageScale;
    public Transform rageBar;
    public SpriteRenderer rageBarBG;

    [SerializeField] private bool active;
    private float timer;
    public float MaxSwitchTime;
    public float MinSwitchTime;
    [SerializeField] private float switchTime;
    [SerializeField] private float pivotDistance;
    private bool going;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        going = true;
        zinc = false;
        timer = 0;
        rageScale = rageBar.localScale;
    }

    // Update is called once per frame
    void Update()
    {


        if (active)
        {
            UpdateRage();
            timer += Time.deltaTime;


            if (timer >= switchTime)
            {
                SwitchSymbols();
            }

            if (Mathf.Abs(transform.localPosition.x) >= pivotDistance)
                going = !going;

            if (going)
                transform.position = new Vector3(transform.position.x + Time.deltaTime, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x - Time.deltaTime, transform.position.y, transform.position.z);

        } 

        
    }

    public void SwitchSymbols()
    {
        timer = 0;
        switchTime = UnityEngine.Random.Range(MinSwitchTime,MaxSwitchTime);

        if(!zinc)
        {
            GetComponent<SpriteRenderer>().sprite = zincSymbol;
            zinc = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = brassSymbol;
            zinc = false;
        }
    }

    public void Activate()
    {
        if (GetComponentInParent<Kolos>().alive && GetComponentInParent<Kolos>().tag == "Enemy")
        {
            timer = 0;
            switchTime = UnityEngine.Random.Range(MinSwitchTime, MaxSwitchTime);

            active = true;
            GetComponent<SpriteRenderer>().enabled = true;
            rageBar.GetComponent<SpriteRenderer>().enabled = true;
            rageBarBG.enabled = true;
        }

    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        rageBar.GetComponent<SpriteRenderer>().enabled = false;
        rageBarBG.enabled = false;
        active = false;

    }

    public void UpdateRage()
    {
        rageBar.transform.localScale = new Vector3(1, rageScale.y*(GetComponentInParent<Kolos>().rage / 100), 1);
    }

    public void EnrageKolo(bool pushing)
    {
        if(zinc == pushing)
        {
            GetComponentInParent<Kolos>().rage += 20;
        }
        else
        {
            if (GetComponentInParent<Kolos>().rage - 10 > 0)
                GetComponentInParent<Kolos>().rage -= 10;
            else
                GetComponentInParent<Kolos>().rage = 0;
        }
    }

}
