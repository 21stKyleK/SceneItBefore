using UnityEngine;

/*
 * purpose of this script is to control how props behave when selected.
 * Has 2 movement modes (1, 2), 2 rotation modes (3, 4), 2 scaling modes (5, 6), and deleting the prop (7)
 * Tells the selected prop to continue being selected until this component loses track of it
 */

public class MovingProp : MonoBehaviour
{
    //prop that the user is controlling, provided by the WhatClickOn script
    public GameObject clickProp = null;
    //component with the selection values to prevent hte prop from being selected by other users
    public PropTracker clickPropTrack = null;

    //to determine how the prop should move. Should be set via a UI button
    //1 = XY movement, 2 = XZ movement, 3 = XY rotation, 4 = XZ rotation, 5 = XY scale, 6 = XZ scale, 7 = delete
    public int toolMode = 0;
    //former prevents the prop from flying off the screen, while hte latter is to undo some of the former to make rotates faster
    [SerializeField] FloatVar limitFactor, rotateFactor;

    private bool firstMove = true;

    private Vector3 oldMousePos, newMousePos;

    //update called once per frame
    void Update()
    {
        //inform the prop that it is being controlled by the current user
        if (clickPropTrack)
        {
            //Debug.Log("Setting prop to false.");

            //former handles the check to put prop into to-be-selectable
            clickPropTrack.notInUse = false;
            //latter will confirm whether or not to go into a selectable state
            clickPropTrack.isFree = false;
        }

    }

    //FixedUpdate is called every so often
    void FixedUpdate()
    {
        if (clickProp)
        {
            //change the position of the prop's x and y coordinates
            //https://gamedevbeginner.com/how-to-move-an-object-with-the-mouse-in-unity-in-2d/

            //if want to implement multiple camera angles, may have to change this to represent the world position via the camera
            newMousePos = Input.mousePosition;
            //if there's no oldMousePos, get new mouse positions
            if (firstMove)
            {
                oldMousePos = newMousePos;
                firstMove = false;
            }

            Vector3 difference = mousePosDiff();

            //iterate through the toolModes to determine the function
            switch (toolMode)
            {
                case 2:
                    moveXZ(difference);
                    break;
                    case 3:
                        rotateXY(difference); break;
                    case 4:
                        rotateXZ(difference); break;
                    case 5:
                        scaleXY(difference); break;
                    case 6:
                        scaleXZ(difference); break;
                case 7:
                    deleteProp(); break;
                default:
                    moveXY(difference);
                    break;
            }

            //update oldMousePos to current
            oldMousePos = newMousePos;

            //don't do anything with the prop here, as it could reference the object after it's been deleted
        }
        else
        {
            firstMove = true;
            //toolMode = 0;
        }
    }

    //calculates the difference between the previous cursor postition and the current
    private Vector3 mousePosDiff()
    {
        return (newMousePos - oldMousePos) * limitFactor;
    }

    //move prop possition
    void moveXY(Vector3 difference)
    {
        //set the position of the selected object to itself plus the difference of the mouse position in terms of x and y
        Vector3 newPos = new Vector3(clickProp.transform.position.x + difference.x,
            clickProp.transform.position.y + difference.y, clickProp.transform.position.z);

        clickProp.transform.position = newPos;
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Move^" + clickProp.name + "^" + newPos.x + "^" 
            + newPos.y + "^" + newPos.z);
    }

    void moveXZ(Vector3 difference)
    {
        //set the position of the selected object to itself plus the difference of the mouse position in terms of x and z
        Vector3 newPos = new Vector3(clickProp.transform.position.x + difference.x,
            clickProp.transform.position.y , clickProp.transform.position.z + difference.y);

        clickProp.transform.position = newPos;
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Move^" + clickProp.name + "^" + newPos.x + "^"
            + newPos.y + "^" + newPos.z); 
        
    }

    //move prop rotation. Y movement will roate along X axis. Undo the limit factor to speed up rotation
    void rotateXY(Vector3 difference)
    {
        //set the rotation of the selected object to itself plus the difference of the mouse position in terms of x and y
        clickProp.transform.Rotate(difference.y / (limitFactor * rotateFactor), difference.x / (limitFactor * rotateFactor),
            0, Space.World);
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Rotate^" + clickProp.name + "^" + clickProp.transform.rotation.x + "^" 
            + clickProp.transform.rotation.y + "^" + clickProp.transform.rotation.z);
    }

    void rotateXZ(Vector3 difference)
    {
        //set the rotation of the selected object to itself plus the difference of the mouse position in terms of x and z
        clickProp.transform.Rotate(difference.y / (limitFactor * rotateFactor), 0,
            difference.x / (limitFactor * rotateFactor), Space.World);
        //I think the extra Space.World parameter makes the rotation better
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Rotate^" + clickProp.name + "^" + clickProp.transform.rotation.x + "^" 
            + clickProp.transform.rotation.y + "^" + clickProp.transform.rotation.z);
    }

    //change prop's scale
    void scaleXY(Vector3 difference)
    {
        //set the scale of the selected object to itself plus the difference of the mouse position in terms of x and y
        clickProp.transform.localScale = new Vector3(clickProp.transform.localScale.x + difference.x,
            clickProp.transform.localScale.y + difference.y, clickProp.transform.localScale.z);
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Scale^" + clickProp.name + "^"
           + clickProp.transform.localScale.x + "^" + clickProp.transform.localScale.y + "^" + clickProp.transform.localScale.z);
    }

    void scaleXZ(Vector3 difference)
    {
        //set the scale of the selected object to itself plus the difference of the mouse position in terms of x and z
        clickProp.transform.localScale = new Vector3(clickProp.transform.localScale.x + difference.x,
            clickProp.transform.localScale.y, clickProp.transform.localScale.z + difference.y);
        Connection connect = GameObject.Find("Connect").GetComponent<Connection>();
        connect.SendWebSocketMessage("Scale^" + clickProp.name  + "^" 
            + clickProp.transform.localScale.x + "^" + clickProp.transform.localScale.y + "^" + clickProp.transform.localScale.z);
    }

    void deleteProp()
    {
        //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Object.Destroy.html
        Destroy(clickProp);
        clickProp = null;
    }

    //Takes the already existing mode and makes it functional for the buttons by assigning them the existing numbers into it via ButtonClick()
    public void SetToolMode(int mode)
    {
        toolMode = mode;
    }
}
