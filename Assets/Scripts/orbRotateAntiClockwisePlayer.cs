using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbRotateAntiClockwisePlayer : MonoBehaviour
{
    private GameObject player;
    public float velocity = 100f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.down, velocity * Time.deltaTime);
    }
}
