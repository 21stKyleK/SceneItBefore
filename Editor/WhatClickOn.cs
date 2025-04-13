using UnityEngine;
using UnityEngine.InputSystem;

public class WhatClickOn : MonoBehaviour
{
    public MovingProp mps;

    //to test the toolMode behavior for MoveProp. Should be removed after implementing UI button functionality
    public int debugMode = 1;

    //due to the size of the stage, the ray doesn't extend far enough to hit objects
    public float scaleOfStage = 1.0f;

    //double-click due to something with how the OnClick() function is called. To handle that behavior
    private bool dumbClick = false;

    //variables to hold the object and component
    private GameObject clickThing = null;
    private PropTracker clickThingCom = null;
    Connection connect;


    void Start()
    {
        mps = GetComponent<MovingProp>();
        connect = GameObject.Find("Connect").GetComponent<Connection>();
        if (connect == null)
        {
            //connect = new Connection();
            Debug.Log("Oops");
        }
    }


    //https://gamedevbeginner.com/how-to-use-the-player-input-component-in-unity/
    //https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
    //activates twice for some reason, which may interfere with program if not accounted for. May cause issues later
    public void OnClick()
    {
        //null out the object in case nothing happens
        clickThing = null;
        clickThingCom = null;

        if (dumbClick)
        {
            dumbClick = false;
        }
        else
        {
            //dumbClick will track if press is on or off
            dumbClick=true;

            /*
            //get the current mouse position on click, and then set the Z position to a non-zero value
            Vector2 tempMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePos = new Vector3(tempMousePos.x, tempMousePos.y, Camera.main.nearClipPlane);

            //https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
            //set the mouse position to a world position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //https://gamedevbeginner.com/raycasts-in-unity-made-easy/#other_raycast_types
            //create a ray to cast at the mousePos
            Ray theRay = new Ray(mousePos, Vector3.forward);
            */


            //https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
            //doesn't work with touchscreen very well, at least on my laptop. May be an issue with implementation
            Ray theRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            //draw a line in the Unity editor view starting from the origin, in the direction for some units, colored and lasting for some seconds
            //Debug.DrawRay(theRay.origin, theRay.direction * 10 * scaleOfStage, Color.blue, 10);

            //cast a raycast to get an object
            RaycastHit selectedObj;
            //gets RaycastHit information from this function, which returns true if something is hit
            bool hitThing = Physics.Raycast(theRay, out selectedObj, 10.0f * scaleOfStage);

            //Debug.Log(hitThing);
            if (hitThing)
            {
                //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/RaycastHit.html
                //print out the transform of the selected object. It should only be of the prop objects and not anything else
                    //put non prop objects on the "ignore raycast" layer via the Unity inspector, top right
                //Debug.Log("Selected Object\t" + selectedObj.colliderInstanceID);

                clickThingCom = selectedObj.transform.gameObject.GetComponent<PropTracker>();

                //check if the object is held by another user (as per the messages)
                if (clickThingCom.nobodyElse)
                {
                    clickThing = selectedObj.transform.gameObject;
                    // mps.toolMode = debugMode; // no longer needed 
                }
                else
                {
                    //undo the clickThingCom assignment because gets assigned to the MovingProp script
                    clickThingCom = null ;
                }

            }
            /*
            else
            {
                //Debug.Log(theRay);
                //Instantiate(new GameObject(), theRay.origin + new Vector3(0,0,10.0f), Camera.main.transform.rotation);
            }
            */

        }

        //set the component's object to the determined object
        mps.clickProp = clickThing;
        mps.clickPropTrack = clickThingCom;
    }
}
