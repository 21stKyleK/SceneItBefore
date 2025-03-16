using UnityEngine;
using UnityEngine.InputSystem;

public class WhatClickOn : MonoBehaviour
{
    public MovingProp mps;

    //to test the toolMode behavior for MoveProp
    public int debugMode = 1;

    private bool dumbClick = false;

    void Start()
    {
        mps = GetComponent<MovingProp>();
    }


    //https://gamedevbeginner.com/how-to-use-the-player-input-component-in-unity/
    //https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
    //activates twice for some reason, which may interfere with program if not accounted for. May cause issues later
    public void OnClick()
    {
        //null out the object in case nothing happens
        GameObject clickThing = null;

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

            //Debug.DrawRay(theRay.origin, theRay.direction * 10, Color.blue, 10);

            //cast a raycast to get an object
            RaycastHit selectedObj;
            //gets RaycastHit information from this function, which returns true if something is hit
            bool hitThing = Physics.Raycast(theRay, out selectedObj, 10.0f);

            //Debug.Log(hitThing);
            if (hitThing)
            {
                //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/RaycastHit.html
                //print out the transform of the selected object
                Debug.Log("Selected Object\t" + selectedObj.colliderInstanceID);

                clickThing = selectedObj.transform.gameObject;
                mps.toolMode = debugMode;
            }
            else
            {
                Debug.Log(theRay);
                //Instantiate(new GameObject(), mousePos, Camera.main.transform.rotation);
            }

        }

        //set the component's object to the determined object
        mps.clickProp = clickThing;
    }
}
