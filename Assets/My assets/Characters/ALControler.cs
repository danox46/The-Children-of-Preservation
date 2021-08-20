using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class ALControler : MonoBehaviour
{
    public Rigidbody2D attachedMetal;
    public bool mouseOver = false;
    public bool facingRight = true;
    //base force need to be moved to ALMLineInst
    private int force;

    //private float angle = 0;
    private LineRenderer lineR;
    private Ray fromChar;
    private GameObject player;
    private BoxCollider2D lineCollider;
    private Vector2 angleVector;
    public bool pushing = false;
    public bool pulling = false;

    private void Start()
    {
        //Reset the local position of the line
        transform.localPosition = new Vector3(0, 0, 0);

        //Get the Line Renderer in this object
        lineR = gameObject.GetComponent<LineRenderer>();

        //Get the box collider attached to this object
        lineCollider = gameObject.GetComponent<BoxCollider2D>();

        //Debug.Log("Partent: " + GetComponentInParent<Transform>().name);
        force = GetComponentInParent<Transform>().GetComponentInParent<ALSpawmer>().allomanticForce;

    }

    void Update()
    {

        //Draw the lines
        if (this.GetComponentInParent<PlatformerCharacter2D>().alive && (this.GetComponentInParent<ALSpawmer>().burnSteel || this.GetComponentInParent<ALSpawmer>().burnIron))
        {
            //When the mouse is over this collider double it's size, else return it to normal
            if (mouseOver)
                lineR.widthMultiplier = 0.3f;
            else
                lineR.widthMultiplier = 0.05f;
        }
        else
        {
            lineR.widthMultiplier = 0;
        }
    }

    private void FixedUpdate()
    {
        // Find what layer 14 is for
        //if the coin is destoyed destroy this too
        if (attachedMetal != null && ((attachedMetal.gameObject.layer == 9) || (attachedMetal.gameObject.layer == 10)))
            lineUpdate();
        else
            GetComponentInParent<ALSpawmer>().DestroyLine(this.transform);

        //return mouseOver to false after each update
        //mouseOver = false;

        if (mouseOver && pushing)
        {
            //Debug.LogWarning("x: " + angleVector.x + " Y: " + angleVector.y);
            //Get rigid body
            Rigidbody2D plataformer = transform.parent.GetComponentInParent<Rigidbody2D>();
            // check if the metal is to the left or right of the character
            if (attachedMetal.transform.position.x < transform.parent.position.x)
            {
                //apply force to the metal
                attachedMetal.AddForce(angleVector * (-force));
                // if the metal can't move, apply inverse force to the char.
                if (attachedMetal.velocity.magnitude <= 0.01f)
                {
                    plataformer.AddForce(angleVector * (force));
                }
            }
            else
            {
                //apply force with inverse direction
                attachedMetal.AddForce(angleVector * force);
                if (attachedMetal.velocity.magnitude <= 0.01f)
                {
                    plataformer.AddForce(angleVector * (-force));
                }
            }
        }

        if (mouseOver && pulling)
        {


            Rigidbody2D plataformer = transform.parent.GetComponentInParent<Rigidbody2D>();
            if (attachedMetal.transform.position.x < transform.parent.position.x)
            {
                attachedMetal.AddForce(angleVector * (force));
                if (attachedMetal.velocity.magnitude <= 0.01f)
                {
                    plataformer.AddForce(angleVector * (-force));
                }
            }
            else
            {
                attachedMetal.AddForce(angleVector * (-force));
                if (attachedMetal.velocity.magnitude <= 0.01f)
                {
                    plataformer.AddForce(angleVector * (force));
                }
            }
        }

    }

    private void lineUpdate()
    {
        Vector3 metalPosition = attachedMetal.transform.position;
        Vector3 charPosition = GetComponentInParent<Mistborn>().chest.position;
        float angleY;
        float angleX;
        float angle = 0;
        float offX = 0;
        float offY = GetComponentInParent<Mistborn>().chest.localPosition.y;

        

        //Draw the line from the current position of the attached metal and the character
        lineR.SetPosition(1, metalPosition);
        lineR.SetPosition(0, charPosition);

        //Ajust the box collider to the line
        //lenght of the collider
        lineCollider.size = new Vector2((Vector3.Distance(charPosition, metalPosition)) / 2, 1f);

        //if the character position is lower than the metal position on the x axis 
        //adjust the angle's values according to the positions and set the offset on x to half the base of the collider 
        if (metalPosition.x > lineR.GetPosition(0).x)
        {
            offY -=  offY / (metalPosition.x - lineR.GetPosition(0).x + 1);
            offX -= (lineCollider.size.x) / 2;
            angleX = (metalPosition.x - lineR.GetPosition(0).x);
            angleY = (metalPosition.y - lineR.GetPosition(0).y);
        }
        else
        {
            offY -= offY / (lineR.GetPosition(0).x - metalPosition.x + 1);
            offX += (lineCollider.size.x) / 2;
            angleX = (lineR.GetPosition(0).x - metalPosition.x);
            angleY = (lineR.GetPosition(0).y - metalPosition.y);
        }

        if (facingRight)
            offY = -offY;

        //Calculate the angle for the collider and set it to degrees
        angle = Mathf.Atan2(angleY, angleX) * Mathf.Rad2Deg;


        // if the character is flipped use -angle, else invert the angle
        if (!facingRight)
        {
            angle *= -1;
        }
        else
        {
            angle = (angle + 180) % 360;
        }


        if (angleX < angleY)
        {
            if (angleX / angleY > -1)
                angleVector = new Vector2(angleX / angleY, 1);
            else
                angleVector = new Vector2(-1, 1);
        }
        else
        {
            if (angleY / angleX > -1)
                angleVector = new Vector2(1, angleY / angleX);
            else
                angleVector = new Vector2(0.4f, -1);
        }

        //Then send the angle to the local transform
        transform.localEulerAngles = new Vector3(0, 0, angle);

        //And set the offset;
        lineCollider.offset = new Vector2(offX, offY);
    }

}
