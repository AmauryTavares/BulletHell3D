using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;

    public float moveSpeed = 5;
    public float smoothTime = 0.7f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveWithTarget();
    }

    void MoveWithTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        Vector3 mouseOffset = new Vector3(mousePos.x, 0, mousePos.y);
        targetPos = target.position + offsetPos + mouseOffset/70;

        //transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

}
