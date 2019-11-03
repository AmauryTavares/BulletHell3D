using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text recorderText;
    // Start is called before the first frame update
    void Start()
    {
        //LoadFile();
        recorderText.text = "" + GameManager.recorder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        GameData data = null;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);

            BinaryFormatter bf = new BinaryFormatter();
            file.Position = 0;
            data = (GameData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            recorderText.text = "0";
            return;
        }

        recorderText.text = "" + data.score;
    }
}
