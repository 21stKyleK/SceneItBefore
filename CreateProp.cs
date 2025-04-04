using UnityEngine;

public class CreateProp : MonoBehaviour
{
    //upload prop prefab to this variable
    public GameObject makeThis = null;

    //use a Vector3Var to make the position that will house new props
    [SerializeField] Vector3Var defaultPosition;

    //file path that will be appended to whatever the folder that contains the mesh data
    public string filePath = "mesh.dat";
    
    //create prop at defaultPosition
    public void CreateTheProp()
    {
        //new instances can't just have a Vector3
        //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Object.Instantiate.html
        GameObject newThing = Instantiate(makeThis, defaultPosition.Value, new Quaternion(0, 0, 0, 0));

        //activate the MeshLoader
        newThing.GetComponent<MeshLoader>().LoadMeshFromPath($"{Application.dataPath}/{filePath}");
        /*alter this to use a more fitting file path
         */
    }

    //overload incase previous implementation doesn't work
    public void CreateTheProp(GameObject makeThat)
    {
        GameObject newThing = Instantiate(makeThat, defaultPosition.Value, new Quaternion(0, 0, 0, 0));

        newThing.GetComponent<MeshLoader>().LoadMeshFromPath($"{Application.dataPath}/{filePath}");
    }

    //overload with a file path instead
    public void CreateTheProp(string fPath)
    {
        GameObject newThing = Instantiate(makeThis, defaultPosition.Value, new Quaternion(0, 0, 0, 0));

        newThing.GetComponent<MeshLoader>().LoadMeshFromPath($"{Application.dataPath}/{fPath}");
    }

    public void SetFilePath(string newPath)
    {
        filePath = newPath;
    }

    //sets the prop to load to a new one
    public void SetNewProp(GameObject newThing)
    {
        makeThis = newThing;
    }
}
