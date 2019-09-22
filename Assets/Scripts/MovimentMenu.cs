using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentMenu : MonoBehaviour
{

    private Quaternion previousRotation = Quaternion.Euler(0, 0, 0);
    private Quaternion rotationTarget = Quaternion.Euler(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        print(mousePos);
        rotationTarget = Quaternion.Euler(mousePos.y * 5, -mousePos.x, 0);

        if (previousRotation != rotationTarget || previousRotation == Quaternion.Euler(0, 0, 0))
        {
            Quaternion rotationFinal = rotationTarget * Quaternion.Inverse(previousRotation);
            rotationFinal.z = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationFinal, Time.deltaTime);
            previousRotation = rotationTarget;
        }
       
    }
}
