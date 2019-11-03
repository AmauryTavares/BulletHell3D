using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

 [System.Serializable]
public class GameData
{
    public int score;

    public GameData(int scoreInt)
    {
        score = scoreInt;
    }
}

public class GameManager : MonoBehaviour
{
    public static int recorder = 0;
    private int score = 0;
    private int level = 1;
    private float multiplier = 0.05f;
    private int countEnemyPortal = Random.Range(5, 31);
    private Vector3 lastEnemyPos;

    public Text textLevel;
    public Text textScore;
    // Start is called before the first frame update
    void Start()
    {
        countEnemyPortal = Random.Range(5, 31);
        GetComponent<GenerateLevel>().spawnMap(level);
        //LoadFile();
    }

    // Update is called once per frame
    void Update()
    {
        checkCountEnemyPortal();
        updateGUI();
    }

    public void endGame()
    {
        if (score > recorder)
        {
            recorder = score;
            //SaveFile();
        }
        SceneManager.LoadScene("Menu");
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenWrite(destination);
        else
            file = File.Create(destination);

        GameData data = new GameData(score);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
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
            data = (GameData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            recorder = 0;
            return;
        }

        recorder = data.score;
    }

    void updateGUI()
    {
        textLevel.text = "Level " + level;
        textScore.text = "Score: " + score;
    }

    public void addScore(int points)
    {
        score += points;
    }

    public void subCountEnemyPortal(Vector3 posEnemy)
    {
        if (countEnemyPortal > 0)
        {
            lastEnemyPos = posEnemy;
            countEnemyPortal -= 1;
        }
    }

    private void checkCountEnemyPortal()
    {
        if (countEnemyPortal <= 0)
        {
            GetComponent<GenerateLevel>().destroyAllGameObjects();
            GetComponent<GenerateLevel>().spawnPortal(lastEnemyPos);
            countEnemyPortal = Random.Range(5, 31);
        }
    }

    public void nextLevel()
    {
        level += 1;
        GetComponent<GenerateLevel>().spawnMap(level);
        GetComponent<GenerateLevel>().newPlayerPos();
        GetComponent<GenerateLevel>().destroyPortal();
    }
}
