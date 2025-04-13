using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


//obtained from https://www.riccardostecca.net/articles/save_and_load_mesh_data_in_unity/
//saves and loads any serializable data, which can be made with the [System.Serializable] tag outside of the class declaration

public static class GenericSaveLoad
{
    /*creates/overwrite's a file with serializable data
    //for the purposes of this project, this function should not be used on props except to create the mesh initially
    //it could also be used to create the save data file if we create our own serializable class
    */

    /*
     Save method not needed for final build (I think)

    public static void Save<T>(T data, string filePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, data);
            }
        }
        catch
        {
            // Forward exceptions
            throw;
        }
    }
    */

    /*
    //retrieves data's file into the specified serilizable format
    public static T Load<T>(string filePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fileStream);
            }
        }
        catch
        {
            // Forward exceptions
            throw;
            // return default;
        }
    }
    */

    /*
    //function that's supposed to retrieve data from online using WebRequests. Only need the name of the file
    // use https://www.sceneitbefore.org/WebGLBuild/mesh.dat
    public static T WebLoad<T>(string fileName)
    {
        try
        {
            //from a Google AI overview
            using (UnityWebRequest request = UnityWebRequest.Get($"https://www.sceneitbefore.org/WebGLBuild/{fileName}"))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(request);
            }
        }
        catch
        {
            // Forward exceptions
            throw;
            // return default;
        }
    }
    */

    //need a coroutine since an asynchronous load
    public static async Task<T> RequestMesh<T>(string fileName)
    {

        using (UnityWebRequest request = UnityWebRequest.Get($"https://www.sceneitbefore.org/WebGLBuild/{fileName}"))
        {
            //sends the request fo th efile at the URL
            //yield return request.SendWebRequest();

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //gets the byte data that is hopefully not header data
                byte[] results = request.downloadHandler.data;

                using (var stream = new MemoryStream(results))
                    using (var binaryStream = new BinaryReader(stream))
                    {

                    IFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(binaryStream.BaseStream);
                }

            }
            else
            {
                UnityEngine.Debug.Log("Data not retrieved with UnityWebRequest");
                throw new InvalidConnectionException();
            }
        }
    }
}
