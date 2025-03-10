using UnityEngine;

//obtained from https://www.riccardostecca.net/articles/save_and_load_mesh_data_in_unity/

/*the purpose of this script is to get mesh data from a file and load it at runtime
 will require the usage of GenericSaveLoad.Load<SerializableMeshInfo> and a GameObject with a Mesh and Mesh Renderer
    and, of course, the file with the mesh data

Should be tested to see if it can retrieve things across the interent
 */

public class MeshLoader : MonoBehaviour
{
    
    public string filePath = "mesh.dat";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-dataPath.html
        //application.dataPath leads to the executable's data folder, which can be anywhere on Windows
        //named ProjectBuildName_Data and when first building, will be found with the executable
        string path = $"{Application.dataPath}/{filePath}";
        //look for the mesh at the above path, and if there, then get the mesh info and store in the Filter
        GetComponent<MeshFilter>().sharedMesh = GenericSaveLoad.Load<SerializableMeshInfo>(path).GetMesh();
    }
}
