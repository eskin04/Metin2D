using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SystemSave
{

    public static void SavePlayer(PlayerData player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        Data data = new Data(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static Data LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    public static void RemovePlayer(PlayerData player)
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }


}
