using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    public float damage = 0f;
    public string elementType = "Fire";
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Wall")
        {
            //spawnExplosion();
            GameObject.Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            //spawnExplosion();
            GameObject.Destroy(gameObject);
        } 
        else if (other.gameObject.tag == "Enemy")
        {
            //spawnExplosion();
            GameObject.Destroy(gameObject);
        }
    }

    void spawnExplosion ()
    {
        GameObject exp = Instantiate(explosion);
        exp.transform.parent = gameObject.transform;
        exp.transform.position = gameObject.transform.position;
        exp.transform.rotation = gameObject.transform.rotation;
        exp.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }
}
