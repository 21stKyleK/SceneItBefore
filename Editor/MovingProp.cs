using UnityEngine;

public class MovingProp : MonoBehaviour
{
    public GameObject clickProp = null;

    //to determine how the prop should move.
    //1 = XY movement, 2 = XZ movement, 3 = XY rotation, 4 = XZ rotation, 5 = XY scale, 6 = XZ scale, 7 = delete
    public int toolMode = 0;
    //former prevents the prop from flying off the screen, while hte latter is to undo some of the former to make rotates faster
    public float limitFactor = 0.01f, rotateFactor = 3.8f;

    private bool firstMove = true;

    private Vector3 oldMousePos, newMousePos;

    // Update is called every so often
    void FixedUpdate()
    {
        if (clickProp)
        {
            //change the position of the prop's x and y coordinates
            //https://gamedevbeginner.com/how-to-move-an-object-with-the-mouse-in-unity-in-2d/

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
                default:
                    moveXY(difference);
                    break;
            }

            //update oldMousePos to current
            oldMousePos = newMousePos;
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
        clickProp.transform.position = new Vector3(clickProp.transform.position.x + difference.x,
            clickProp.transform.position.y + difference.y, clickProp.transform.position.z);
    }

    void moveXZ(Vector3 difference)
    {
        //set the position of the selected object to itself plus the difference of the mouse position in terms of x and z
        clickProp.transform.position = new Vector3(clickProp.transform.position.x + difference.x,
            clickProp.transform.position.y, clickProp.transform.position.z + difference.y);
    }

    //move prop rotation. Y movement will roate along X axis. Undo the limit factor to speed up rotation
    void rotateXY(Vector3 difference)
    {
        //set the rotation of the selected object to itself plus the difference of the mouse position in terms of x and y
        clickProp.transform.Rotate(difference.y / (limitFactor * rotateFactor), difference.x / (limitFactor * rotateFactor),
            0, Space.World);
    }

    void rotateXZ(Vector3 difference)
    {
        //set the rotation of the selected object to itself plus the difference of the mouse position in terms of x and z
        clickProp.transform.Rotate(difference.y / (limitFactor * rotateFactor), 0,
            difference.x / (limitFactor * rotateFactor), Space.World);
        //I think the extra Space.World parameter makes the rotation better
    }

    //change prop's scale
    void scaleXY(Vector3 difference)
    {
        //set the scale of the selected object to itself plus the difference of the mouse position in terms of x and y
        clickProp.transform.localScale = new Vector3(clickProp.transform.localScale.x + difference.x,
            clickProp.transform.localScale.y + difference.y, clickProp.transform.localScale.z);
    }

    void scaleXZ(Vector3 difference)
    {
        //set the scale of the selected object to itself plus the difference of the mouse position in terms of x and z
        clickProp.transform.localScale = new Vector3(clickProp.transform.localScale.x + difference.x,
            clickProp.transform.localScale.y, clickProp.transform.localScale.z + difference.y);
    }
}
