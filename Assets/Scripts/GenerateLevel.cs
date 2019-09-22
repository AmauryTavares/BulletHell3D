using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject enemy;
    public GameObject terrainObject;

    public int numberOfEnemies;
    public int numberOfTerrainObjects;

    public GameObject terrain;
    private MeshCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = terrain.GetComponent<MeshCollider>();
        GenerateObject(terrainObject, numberOfTerrainObjects);
        GenerateObject(enemy, numberOfEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateObject(GameObject go, int amount)
    {

        if (go == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject tmp = Instantiate(go);

            Vector3 randomPoint = GetRandomPoint();
            tmp.gameObject.transform.position = new Vector3(randomPoint.x, tmp.transform.position.y, randomPoint.z);
        }

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
