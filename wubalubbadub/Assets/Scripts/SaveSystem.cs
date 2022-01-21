using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

        public static void SaveMap(string filename,creategrid map) {
        string _filename = filename + ".map";
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Unity/" + _filename;
        FileStream stream = new FileStream(path, FileMode.Create);

        MapData mapData = new MapData(map);
        mapData.name = filename;

        formatter.Serialize(stream, mapData);
        stream.Close();
    }

    public static MapData LoadMap(string filename)
    {
        string path = Application.persistentDataPath + "/Unity/" + filename+".map";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            MapData map = formatter.Deserialize(stream) as MapData;
            stream.Close();
            return map;
        }
        else
        {
            Debug.Log("Map file: " + filename + " does not exist.");
            return null;
        }
    }
    
}