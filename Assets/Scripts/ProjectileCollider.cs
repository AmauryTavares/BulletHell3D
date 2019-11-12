using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    public float damage = 0f;
    public string elementType = "Fire";
    private GameObject explosion;

    public GameObject explosionRed;
    public GameObject explosionBlue;
    public GameObject explosionGreen;
    public GameObject explosionYellow;
    public GameObject explosionOrange;

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
            spawnExplosion();
            GameObject.Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            spawnExplosion();
            GameObject.Destroy(gameObject);
        } 
        else if (other.gameObject.tag == "Enemy")
        {
            spawnExplosion();
            GameObject.Destroy(gameObject);
        }
    }

    void spawnExplosion ()
    {
        if (elementType == "Fire")
        {
            explosion = explosionRed;
        }
        else if(elementType == "Nature")
        {
            explosion = explosionGreen;
        }
        else if (elementType == "Water")
        {
            explosion = explosionBlue;
        }
        else if (elementType == "Lightning")
        {
            explosion = explosionYellow;
        }
        else if (elementType == "Earth")
        {
            explosion = explosionOrange;
        }

        GameObject exp = Instantiate(explosion);
        //exp.transform.parent = gameObject.transform;
        exp.transform.position = gameObject.transform.position;
        exp.transform.rotation = gameObject.transform.rotation;
    }
}
