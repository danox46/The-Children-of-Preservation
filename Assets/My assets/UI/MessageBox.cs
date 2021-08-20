using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    private List<string> fields;
    private int currentField = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        fields = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public virtual void DisplayMessage(List<string> message)
    {
        fields = message;
        GetComponentInChildren<Text>().text = fields[0];
        currentField = 0;
    }

    public void NextField()
    {
        //Debug.Log("fields count: " + fields.Count);
        if (currentField + 1 < fields.Count)
        {
            GetComponentInChildren<Text>().text = fields[currentField + 1];
            currentField++;
        }
        else
        {
            //Debug.Log("on else");
            Deactivate();
        }

    }

    public virtual void Deactivate()
    {
        GetComponentInChildren<Text>().text = "";
        gameObject.SetActive(false);
    }
}
