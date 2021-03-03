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
    private Encoding encoding = Encoding.GetEncoding("UTF-8");

    private void Awake()
    {
        if (instance == null) instance = this;
        SetPath();
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



}
