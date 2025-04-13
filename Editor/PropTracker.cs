using System.Text;
using UnityEngine;

/*
 * this script controls how the prop behaves on the stage while selected
 * if this prop is selected, then it will recieve from MovingProp values that confirm it as being in use
 * however, it will always try to make itself available. This shouldn't break anything as it must go through 2 checks before
 *      successfully becoming free, and the rate at which it does so is slower than the rate in which it is confirmed as selected
 *      due to the implementation of FixedUpdate() versus Update()
 */

public class PropTracker : MonoBehaviour
{
    //coordinate of the center of the stage
    [SerializeField] Vector3Var centerStage;

    //boundaries based on the center of the stage, being horizontal, vertical, and depth respectively
    [SerializeField] FloatVar xBound, yBound, zBound;

    //booleans to control whether or not the prop is selected by a user
    public bool isFree = true, notInUse = true;
    //true means that users can select them, isFree = false means users can't select, notInUse = false means that a user is still using it

    private StringBuilder strB = new StringBuilder();

    // Update is called every so often
    void FixedUpdate()
    {
        //debug line to check if the prop actually clamps. It does
        //isFree = false;

        //to reduce the number of operations if idle, only check position if prop isn't free
        if (!isFree)
        {
            //try to become unselected from users
            if (notInUse) { isFree = true;

                //Debug.Log("Prop became Free");

                //keep the prop in bounds. Only has to be done once so prop looks nice while tracking mouse
                //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Clamp.html
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, centerStage.Value.x - xBound, centerStage.Value.x + xBound),
                    Mathf.Clamp(transform.position.y, centerStage.Value.y - yBound, centerStage.Value.y + yBound),
                    Mathf.Clamp(transform.position.z, centerStage.Value.z - zBound, centerStage.Value.z + zBound));
                //clamp will return either the first parameter (within the bounds) or the other 2 (under the minimum or over the maximum)

                Connection connect = GameObject.Find("Connect").GetComponent<Connection>();

                strB.Clear().Append("Move^").Append(name).Append("^").Append(transform.position.x).Append("^").Append(transform.position.y).Append("^").Append(transform.position.z);

                connect.SendWebSocketMessage(strB.ToString());
            }

                //go into a state that can unselect itself
                notInUse = true;
            //will be updated less often than the user's confirmation of control, thus the unselect statement should never occur
        }
    }
}
