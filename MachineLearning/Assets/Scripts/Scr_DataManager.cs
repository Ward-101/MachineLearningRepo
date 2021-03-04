using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("Data")]
public class Scr_DataManager : MonoBehaviour
{
    public static Scr_DataManager instance = null;

    public string path;
    XmlSerializer serializer = new XmlSerializer(typeof(Scr_Data));

    public string loadoutPath;
    XmlSerializer loadoutSerializer = new XmlSerializer(typeof(Scr_LoadoutData));

    public string championPath;
    public Scr_Data championNet;

    private Encoding encoding = Encoding.GetEncoding("UTF-8");

    private void Awake()
    {
        if (instance == null) instance = this;

        SetPath();
        SetLoadoutPath();
    }

    public void Save(List<Scr_NeuralNetwork> _nets)
    {
        StreamWriter streamWriter = new StreamWriter(path, false, encoding);
        Scr_Data data = new Scr_Data { nets = _nets };

        serializer.Serialize(streamWriter, data);
    }

    public Scr_Data Load()
    {
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);

            return serializer.Deserialize(fileStream) as Scr_Data;
        }

        return null;
    }

    public void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
    }

    private void SetLoadoutPath()
    {
        loadoutPath = Path.Combine(Application.persistentDataPath, "LoadoutData.xml");
    }

    public void SaveLoadout(List<Scr_CustomLoadout> _loadouts)
    {
        StreamWriter streamWriter = new StreamWriter(loadoutPath, false, encoding);
        Scr_LoadoutData loadoutData = new Scr_LoadoutData { loadouts = _loadouts };

        loadoutSerializer.Serialize(streamWriter, loadoutData);
    }

    public Scr_LoadoutData LoadLoadout()
    {
        if (File.Exists(loadoutPath))
        {
            FileStream fileStream = new FileStream(loadoutPath, FileMode.Open);

            return loadoutSerializer.Deserialize(fileStream) as Scr_LoadoutData;
        }

        return null;
    }

    #region VERSUS MODE

    public void SaveChampion(List<Scr_NeuralNetwork> _net)
    {
        StreamWriter championStreamWriter = new StreamWriter(path, false, encoding);

        Scr_Data championData = new Scr_Data { nets = _net };
        serializer.Serialize(championStreamWriter, championData);
    }

    public Scr_Data LoadChampion()
    {
        if (File.Exists(championPath))
        {
            FileStream fileStream = new FileStream(championPath, FileMode.Open);

            return serializer.Deserialize(fileStream) as Scr_Data;
        }

        return null;
    }

    #endregion


}
