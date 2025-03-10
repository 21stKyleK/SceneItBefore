using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//obtained from https://www.riccardostecca.net/articles/save_and_load_mesh_data_in_unity/
//saves and loads any serializable data, which can be made with the [System.Serializable] tag outside of the class declaration

public static class GenericSaveLoad
{
    /*creates/overwrite's a file with serializable data
    //for the purposes of this project, this function should not be used on props except to create the mesh initially
    //it could also be used to create the save data file if we create our own serializable class
    */
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
}
