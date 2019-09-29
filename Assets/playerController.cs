using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private float velocity = 5;
    private float turnspeed = 10;

    private Vector3 input;
    private Vector2 mousePos;
    private float angleRotation;

    private Quaternion targetRotation;
    private Transform cam;


    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); // atualiza os valores de entrada
        CalculateDirectionRotation();
        Rotate();
        updateAnimation();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.z) < 1) return; //movimenta quando um dos eixos eh igual a 1

        Move();
    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");

        mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
    }

    void CalculateDirectionRotation()
    {
        angleRotation = Mathf.Atan2(mousePos.x, mousePos.y);
        angleRotation = Mathf.Rad2Deg * angleRotation;
        //angleRotation += cam.eulerAngles.y;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angleRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnspeed * Time.deltaTime);
    }

    void Move()
    {
        input.Normalize();
        transform.position += input * velocity * Time.deltaTime; // direcao Z * velocidade * DT
    }

    void updateAnimation()
    {

        if (input.x < 1 && input.z < 1)
        {
            animator.Play("idle");
        }

        float angleY = transform.rotation.eulerAngles.y;
        if (input.z > 0 && input.x == 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_right_forward");
            }
        }
        else if (input.z > 0 && input.x > 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_right");
            }
        }
        else if (input.z == 0 && input.x > 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_right");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_right_backward");
            }
        }
        else if (input.z < 0 && input.x > 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_backward");
            }
        }
        else if (input.z < 0 && input.x == 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_left_backward");
            }
        }
        else if (input.z < 0 && input.x < 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_left");
            }
        }
        else if (input.z == 0 && input.x < 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_left");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_forward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_left_forward");
            }
        }
        else if (input.z > 1 && input.x < 0)
        {
            if ((angleY > 338 && angleY <= 360) || (angleY > 0 && angleY <= 23))
            {
                animator.Play("walk_left_forward");
            }
            else if (angleY > 23 && angleY <= 68)
            {
                animator.Play("walk_left");
            }
            else if (angleY > 68 && angleY <= 113)
            {
                animator.Play("walk_left_backward");
            }
            else if (angleY > 113 && angleY <= 158)
            {
                animator.Play("walk_backward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_right");
            }
            else if (angleY > 203 && angleY <= 248)
            {
                animator.Play("walk_right_backward");
            }
            else if (angleY > 248 && angleY <= 293)
            {
                animator.Play("walk_right_forward");
            }
            else if (angleY > 293 && angleY <= 338)
            {
                animator.Play("walk_forward");
            } 
        }
    }
}
