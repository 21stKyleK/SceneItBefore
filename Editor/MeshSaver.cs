using UnityEngine;

//obtained from https://www.riccardostecca.net/articles/save_and_load_mesh_data_in_unity/

/*the purpose of this script is to create a raw mesh data file from a prop
 will require the usage of GenericSaveLoad.Save<SerializableMeshInfo> and a pre-existing mesh

Should be tested to see if it can save things across the internet
 */
public class MeshSaver : MonoBehaviour
{
    //the mesh to be saved. Attach it in the Unity editor
    [SerializeField] Mesh meshIn; 
    public string filePath = "mesh.dat";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //the assets that we plan to use can't easily be made into meshes. I need a mesh to test this
        //if this doesn't work, then a new model will have to be imported
        meshIn = GetComponent<MeshFilter>().sharedMesh;
        //line obtained from https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mesh-isReadable.html
        //meshIn.UploadMeshData(false);

        //Debug.Log($"Printing to {Application.dataPath}/{filePath}");

        //get the asset from the filePath appended to the application path
        //points to the project assets folder if done within Unity editor
        string path = $"{Application.dataPath}/{filePath}";
        GenericSaveLoad.Save<SerializableMeshInfo>(new SerializableMeshInfo(meshIn), path);
    }

}
