using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

[XmlRoot("Data")]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string path;
    public string championPath;

    public Data championNet;

    XmlSerializer serializer = new XmlSerializer(typeof(Data));
    Encoding encoding = Encoding.GetEncoding("UTF-8");

    public void Awake()
    {
        instance = this;
        SetPath();
    }

    public void Save(List<NeuralNetwork> _net)
    {
        StreamWriter streamWriter = new StreamWriter(path, false, encoding);
        Data data = new Data { nets = _net };

        serializer.Serialize(streamWriter, data);
    }

    public Data Load()
    {
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);

            return serializer.Deserialize(fileStream) as Data;
        }

        return null;
    }

    public void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
        championPath = Path.Combine(Application.persistentDataPath, "ChampionData.xml"); // ADDED FOR THE VERSUS MODE //
    }

    #region VERSUS MODE

    public void SaveChampion(List<NeuralNetwork> _net)
    {
        StreamWriter championStreamWriter = new StreamWriter(path, false, encoding);
        Data championData = new Data { nets = _net };

        serializer.Serialize(championStreamWriter, championData);
    }

    public Data LoadChampion()
    {
        if (File.Exists(championPath))
        {
            FileStream fileStream = new FileStream(championPath, FileMode.Open);

            return serializer.Deserialize(fileStream) as Data;
        }

        return null;
    }

    #endregion
}
