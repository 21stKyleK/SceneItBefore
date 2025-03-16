using UnityEngine;

public class MovingProp : MonoBehaviour
{
    public GameObject clickProp = null;

    //to determine how the prop should move.
    //1 = XY movement, 2 = XZ movement
    public int toolMode = 0;
    //prevents the prop from flying off the screen
    public float limitFactor = 0.01f;

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

    private Vector3 mousePosDiff()
    {
        return (newMousePos - oldMousePos) * limitFactor;
    }

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
}
