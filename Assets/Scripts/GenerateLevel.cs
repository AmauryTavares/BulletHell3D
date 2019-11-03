using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject enemyFire;
    public GameObject enemyWater;
    public GameObject enemyEarth;
    public GameObject enemyLightning;
    public GameObject enemyNature;
    public GameObject portal;
    public GameObject player;
    private GameObject instancePortal;

    public GameObject terrainObject1;
    public GameObject terrainObject2;
    public GameObject terrainObject3;
    public GameObject terrainObject4;

    public int numberOfEnemies;
    public int numberOfTerrain1Objects;
    public int numberOfTerrain2Objects;
    public int numberOfTerrain3Objects;
    public int numberOfTerrain4Objects;

    public GameObject terrain;
    private BoxCollider col;

    private int mapLevel;

    private List<GameObject> allGameObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnMap(int level)
    {
        mapLevel = level;
        col = terrain.GetComponent<BoxCollider>();
        GenerateObject(terrainObject1, numberOfTerrain1Objects);
        GenerateObject(terrainObject2, numberOfTerrain2Objects);
        GenerateObject(terrainObject3, numberOfTerrain3Objects);
        GenerateObject(terrainObject4, numberOfTerrain4Objects);
        GenerateObject(enemyFire, Mathf.RoundToInt(numberOfEnemies / 5));
        GenerateObject(enemyWater, Mathf.RoundToInt(numberOfEnemies / 5));
        GenerateObject(enemyEarth, Mathf.RoundToInt(numberOfEnemies / 5));
        GenerateObject(enemyLightning, Mathf.RoundToInt(numberOfEnemies / 5));
        GenerateObject(enemyNature, Mathf.RoundToInt(numberOfEnemies / 5));
    }

    public void destroyAllGameObjects()
    {
        foreach (GameObject gameObj in allGameObjects)
        {
            if (gameObj != null && gameObj.tag == "Enemy")
            {
                gameObj.GetComponent<enemyController>().addScore = false;
                gameObj.GetComponent<enemyController>().life = 0;
            }
            else
            {
                GameObject.Destroy(gameObj);
            }
        }
    }

    public void spawnPortal(Vector3 posPortal)
    {
        instancePortal = Instantiate(portal);

        instancePortal.gameObject.transform.position = new Vector3(posPortal.x, 6, posPortal.z);
    }

    void GenerateObject(GameObject go, int amount)
    {

        if (go == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject tmp = Instantiate(go);
            
            if (tmp.tag == "Enemy")
            {
                float mult = 0;
                if (Random.Range(1, 11) <= 1)
                {
                    mult = (1 + (mapLevel * 0.5f));
                    tmp.gameObject.transform.localScale = new Vector3(2, 2, 2);
                }
                else
                    mult = (1 + (mapLevel * 0.15f));

                tmp.GetComponent<enemyController>().life *= mult;
                tmp.GetComponent<enemyController>().velocity *= mult;
                tmp.GetComponent<enemyController>().vision *= mult;
                tmp.GetComponent<enemyController>().damageAttack *= mult;
                tmp.GetComponent<enemyController>().cooldownAttack *= mult;
                tmp.GetComponent<enemyController>().cooldownAttackMax *= mult;
                tmp.GetComponent<enemyController>().score = Mathf.RoundToInt(tmp.GetComponent<enemyController>().score * mult);
                tmp.GetComponent<enemyController>().gameManager = gameObject;
            }

            allGameObjects.Add(tmp);

            Vector3 randomPoint = GetRandomPoint();
            tmp.gameObject.transform.position = new Vector3(randomPoint.x, tmp.transform.position.y, randomPoint.z);
        }

    }

    public void newPlayerPos()
    {
        Vector3 randomPos = GetRandomPoint();
        player.gameObject.transform.position = new Vector3(randomPos.x, 1, randomPos.z);
    }

    public void destroyPortal()
    {
        GameObject.Destroy(instancePortal);
    }

    Vector3 GetRandomPoint()
    {
        int xRandom = 0;
        int zRandom = 0;


        xRandom = (int)Random.Range(col.bounds.min.x, col.bounds.max.x);
        zRandom = (int)Random.Range(col.bounds.min.z, col.bounds.max.z);

        return new Vector3(xRandom, 0.0f, zRandom);
    }
}
