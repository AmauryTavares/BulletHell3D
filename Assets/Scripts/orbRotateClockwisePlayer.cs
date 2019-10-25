using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbRotateClockwisePlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.up, 100 * Time.deltaTime);
    }
}
