using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public GameObject selectElement;
    public GameObject orbFire;
    public GameObject orbWater;
    public GameObject orbNature;
    public GameObject orbEarth;
    public GameObject orbLightning;

    // Status Player
    private float lifeMax = 1500;
    private float life = 1500;
    private float shield = 0;
    private float shieldMax = 300;
    private float damageAttack = 150f;
    private float velocity = 10;
    private float normalVelocity = 10;
    private float cooldownAttackMax = 0.6f;
    private float cooldownAttack = 0.6f;
    //

    private float orbRotateVelocity = 100f;

    private float turnspeed = 10;
    private float projectileSpeed = 30;
    private float cooldownSelectElement = 3f;
    private float cooldownSelectElementMax = 3f;
    private string firstElementType;
    private string secondElementType;
    private GameObject firstElement;
    private GameObject secondElement;
    private bool spawnedProjectile = false;
    private bool attacking = false;
    private bool dead = false;
    private bool isShowingSelectElement;

    private bool selectedOrbFire = false;
    private bool selectedOrbWater = false;
    private bool selectedOrbNature = false;
    private bool selectedOrbEarth = false;
    private bool selectedOrbLightning = false;

    private bool activeExtraStatus = false;
    private float cooldownExtraStatusMax = 5f;
    private float cooldownExtraStatus = 5f;

    private float cooldownImunity = 0f;
    private float cooldownImunityMax = 2f;

    private bool fisrtAttack = true;

    private Vector3 input;
    private Vector2 mousePos;
    private float angleRotation;

    private Quaternion targetRotation;
    private Transform cam;

    private float previousAngleY;

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
        isDead();
        if (dead) return;
        reduceTime();

        showSelectElement();
        checkHoverElement();
        checkSelectedElement();

        regenLife();

        if (attacking) return;

        isDash();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attack();// attack quando botão esquerdo do mouse eh 
            return;
        }; 

        GetInput(); // atualiza os valores de entrada
        CalculateDirectionRotation();
        Rotate();
        updateAnimation();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.z) < 1) return; //movimenta quando um dos eixos eh igual a 1

        Move();
    }

    void regenLife()
    {
        if (activeExtraStatus && (firstElementType == "Nature" || secondElementType == "Nature") && life < lifeMax)
        {
            life += life * 0.010f * Time.deltaTime;

            if (life > lifeMax)
            {
                life = lifeMax;
            }

            print(life);
        }    
    }

    void speedUp()
    {
        if (activeExtraStatus && (firstElementType == "Lightning" || secondElementType == "Lightning"))
        {
            velocity *= 1.5f;
        }
    }

    void shieldUp()
    {
        if (activeExtraStatus && (firstElementType == "Earth" || secondElementType == "Earth"))
        {
            shield = shieldMax;
        }
    }

    void imunityUp()
    {
        if (activeExtraStatus && (firstElementType == "Water" || secondElementType == "Water"))
        {
            cooldownImunity = cooldownImunityMax;
        }
    }

    void orbUp()
    {
        firstElement.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        firstElement.GetComponent<orbRotateClockwisePlayer>().velocity = orbRotateVelocity * 3;

        secondElement.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        secondElement.GetComponent<orbRotateAntiClockwisePlayer>().velocity = orbRotateVelocity * 3;
    }

    void orbDown()
    {
        firstElement.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        firstElement.GetComponent<orbRotateClockwisePlayer>().velocity = orbRotateVelocity;

        secondElement.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        secondElement.GetComponent<orbRotateAntiClockwisePlayer>().velocity = orbRotateVelocity;
    }

    void isDash()
    {
        if (!activeExtraStatus && Input.GetKeyDown(KeyCode.Space))
        {
            print("ExtraStatus");
            activeExtraStatus = true;

            shieldUp();
            imunityUp();
            speedUp();
            orbUp();
        }
    }

    void isDead()
    {
        if (life <= 0)
        {
            die();
        }
    }

    void showSelectElement()
    {
        selectElement.SetActive(Input.GetKey(KeyCode.Mouse1));
        isShowingSelectElement = Input.GetKey(KeyCode.Mouse1);
    }

    void checkHoverElement()
    {
        float dist = Vector3.Distance(new Vector3(0, 0, 0), mousePos);

        if (isShowingSelectElement && dist > 140)
        {
            float angleY = transform.rotation.eulerAngles.y;
            
            if (angleY >= 324 || angleY < 36)
            {
                setScaleOrb(1);
            }
            else if (angleY >= 36 && angleY < 108)
            {
                setScaleOrb(2);
            }
            else if (angleY >= 108 && angleY < 180)
            {
                setScaleOrb(3);
            }
            else if (angleY >= 180 && angleY < 252)
            {
                setScaleOrb(4);
            }
            else if (angleY >= 252 && angleY < 324)
            {
                setScaleOrb(5);
            }
        } else
        {
            setScaleOrb(0);
        }
    }

    void checkSelectedElement()
    {
        float dist = Vector3.Distance(new Vector3(0, 0, 0), mousePos);

        if (Input.GetKeyUp(KeyCode.Mouse1) && dist > 140)
        {
            float angleY = transform.rotation.eulerAngles.y;

            if (angleY >= 324 || angleY < 36)
            {
                selectedOrbFire = true;
                showOrbs();
                selectedOrbFire = false;
            }
            else if (angleY >= 36 && angleY < 108)
            {
                selectedOrbNature = true;
                showOrbs();
                selectedOrbNature = false;
            }
            else if (angleY >= 108 && angleY < 180)
            {
                selectedOrbEarth = true;
                showOrbs();
                selectedOrbEarth = false;
            }
            else if (angleY >= 180 && angleY < 252)
            {
                selectedOrbLightning = true;
                showOrbs();
                selectedOrbLightning = false;
            }
            else if (angleY >= 252 && angleY < 324)
            {
                selectedOrbWater = true;
                showOrbs();
                selectedOrbWater = false;
            }
        }
    }

    void showOrbs()
    {
        Vector3 orbPos = transform.position + new Vector3(1.5f, 2.5f, 0);

        if (firstElementType == null || cooldownSelectElement <= 0)
        {
            spawnFirstOrb(orbPos);
        }
        else
        {
            spawnSecondOrb(orbPos);
        }
    }

    void spawnFirstOrb(Vector3 orbPos)
    {
        if (firstElementType != null)
        {
            Object.Destroy(firstElement);
            Object.Destroy(secondElement);
        }

        if (selectedOrbFire)
        {
            GameObject orb = Instantiate(orbFire);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateClockwisePlayer>().enabled = true;
            firstElementType = "Fire";
            firstElement = orb;
            cooldownSelectElement = cooldownSelectElementMax;
            secondElementType = null;
            secondElement = null;
        }
        else if (selectedOrbNature)
        {
            GameObject orb = Instantiate(orbNature);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateClockwisePlayer>().enabled = true;
            firstElementType = "Nature";
            firstElement = orb;
            cooldownSelectElement = cooldownSelectElementMax;
            secondElementType = null;
            secondElement = null;
        }
        else if (selectedOrbEarth)
        {
            GameObject orb = Instantiate(orbEarth);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateClockwisePlayer>().enabled = true;
            firstElementType = "Earth";
            firstElement = orb;
            cooldownSelectElement = cooldownSelectElementMax;
            secondElementType = null;
            secondElement = null;
        }
        else if (selectedOrbLightning)
        {
            GameObject orb = Instantiate(orbLightning);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateClockwisePlayer>().enabled = true;
            firstElementType = "Lightning";
            firstElement = orb;
            cooldownSelectElement = cooldownSelectElementMax;
            secondElementType = null;
            secondElement = null;
        }
        else if (selectedOrbWater)
        {
            GameObject orb = Instantiate(orbWater);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateClockwisePlayer>().enabled = true;
            firstElementType = "Water";
            firstElement = orb;
            cooldownSelectElement = cooldownSelectElementMax;
            secondElementType = null;
            secondElement = null;
        }
    }

    void spawnSecondOrb(Vector3 orbPos)
    {
        if (selectedOrbFire)
        {
            GameObject orb = Instantiate(orbFire);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateAntiClockwisePlayer>().enabled = true;
            secondElementType = "Fire";
            secondElement = orb;
            cooldownSelectElement = 0;
        }
        else if (selectedOrbNature)
        {
            GameObject orb = Instantiate(orbNature);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateAntiClockwisePlayer>().enabled = true;
            secondElementType = "Nature";
            secondElement = orb;
            cooldownSelectElement = 0;
        }
        else if (selectedOrbEarth)
        {
            GameObject orb = Instantiate(orbEarth);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateAntiClockwisePlayer>().enabled = true;
            secondElementType = "Earth";
            secondElement = orb;
            cooldownSelectElement = 0;
        }
        else if (selectedOrbLightning)
        {
            GameObject orb = Instantiate(orbLightning);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateAntiClockwisePlayer>().enabled = true;
            secondElementType = "Lightning";
            secondElement = orb;
            cooldownSelectElement = 0;
        }
        else if (selectedOrbWater)
        {
            GameObject orb = Instantiate(orbWater);
            orb.transform.parent = gameObject.transform;
            orb.transform.position = orbPos;
            orb.transform.rotation = gameObject.transform.rotation;
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.GetComponent<orbRotateAntiClockwisePlayer>().enabled = true;
            secondElementType = "Water";
            secondElement = orb;
            cooldownSelectElement = 0;
        }
    }

    void setScaleOrb(int orb)
    {
        if (orb == 0)
        {
            orbWater.transform.localScale = new Vector3(1, 1, 1);
            orbFire.transform.localScale = new Vector3(1, 1, 1);
            orbNature.transform.localScale = new Vector3(1, 1, 1);
            orbEarth.transform.localScale = new Vector3(1, 1, 1);
            orbLightning.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (orb == 1)
        {
            orbFire.transform.localScale = new Vector3(2, 2, 2);
            orbWater.transform.localScale = new Vector3(1, 1, 1);
            orbNature.transform.localScale = new Vector3(1, 1, 1);
            orbEarth.transform.localScale = new Vector3(1, 1, 1);
            orbLightning.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (orb == 2)
        {
            orbNature.transform.localScale = new Vector3(2, 2, 2);
            orbFire.transform.localScale = new Vector3(1, 1, 1);
            orbWater.transform.localScale = new Vector3(1, 1, 1);
            orbEarth.transform.localScale = new Vector3(1, 1, 1);
            orbLightning.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (orb == 3)
        {
            orbEarth.transform.localScale = new Vector3(2, 2, 2);
            orbFire.transform.localScale = new Vector3(1, 1, 1);
            orbWater.transform.localScale = new Vector3(1, 1, 1);
            orbNature.transform.localScale = new Vector3(1, 1, 1);
            orbLightning.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (orb == 4)
        {
            orbLightning.transform.localScale = new Vector3(2, 2, 2);
            orbFire.transform.localScale = new Vector3(1, 1, 1);
            orbWater.transform.localScale = new Vector3(1, 1, 1);
            orbNature.transform.localScale = new Vector3(1, 1, 1);
            orbEarth.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (orb == 5)
        {
            orbWater.transform.localScale = new Vector3(2, 2, 2);
            orbFire.transform.localScale = new Vector3(1, 1, 1);
            orbNature.transform.localScale = new Vector3(1, 1, 1);
            orbEarth.transform.localScale = new Vector3(1, 1, 1);
            orbLightning.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void reduceTime()
    {
        if (attacking && cooldownAttack > 0)
        {
            cooldownAttack -= Time.deltaTime;

            if (!spawnedProjectile && cooldownAttack < (cooldownAttackMax/2))
            {
                spawnedProjectile = true;
                spawnProjectile();
            }

            if (cooldownAttack <= 0)
            {
                cooldownAttack = cooldownAttackMax;
                attacking = false;
                spawnedProjectile = false;
            }
        }

        if (activeExtraStatus && cooldownExtraStatus > 0)
        {
            cooldownExtraStatus -= Time.deltaTime;
            cooldownImunity -= Time.deltaTime;

            if (cooldownExtraStatus <= 0)
            {
                activeExtraStatus = false;
                cooldownExtraStatus = cooldownExtraStatusMax;
                cooldownImunity = 0;
                shield = 0;
                velocity = normalVelocity;
                orbDown();
            }
        }

        if (firstElementType != null && cooldownSelectElement > 0)
        {
            cooldownSelectElement -= Time.deltaTime;

            if (secondElementType == null && cooldownSelectElement <= 0)
            {
                Vector3 orbPos = transform.position + new Vector3(1.5f, 2.5f, 0);

                if (firstElementType.Equals("Fire"))
                {
                    selectedOrbFire = true;
                    spawnSecondOrb(orbPos);
                    selectedOrbFire = false;
                }
                else if (firstElementType.Equals("Nature"))
                {
                    selectedOrbNature = true;
                    spawnSecondOrb(orbPos);
                    selectedOrbNature = false;
                }
                else if (firstElementType.Equals("Earth"))
                {
                    selectedOrbEarth = true;
                    spawnSecondOrb(orbPos);
                    selectedOrbEarth = false;
                }
                else if (firstElementType.Equals("Lightning"))
                {
                    selectedOrbLightning = true;
                    spawnSecondOrb(orbPos);
                    selectedOrbLightning = false;
                }
                else if (firstElementType.Equals("Water"))
                {
                    selectedOrbWater = true;
                    spawnSecondOrb(orbPos);
                    selectedOrbWater = false;
                }

            }
        }
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
        angleRotation += cam.eulerAngles.y;
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

    void die()
    {
        dead = true;
        animator.Play("death");
    }

    void spawnProjectile()
    {
        if (firstElement != null)
        {
            Vector3 projectilePos = transform.position + new Vector3(0, 3, 0);
            Quaternion projectileRot = transform.rotation * Quaternion.Euler(-90, 0, 0); ;
            GameObject projectile = null;
            string projectileType = null;

            if (fisrtAttack || secondElementType == null)
            {
                projectile = firstElement;
                projectileType = firstElementType;
            }
            else
            {
                projectile = secondElement;
                projectileType = secondElementType;
            }

            fisrtAttack = !fisrtAttack;

            Rigidbody rigidbodyProjectile = projectile.GetComponent<Rigidbody>();
            Rigidbody instantiatedProjectile = Instantiate(rigidbodyProjectile, projectilePos, projectileRot);
            instantiatedProjectile.transform.localScale = new Vector3(1, 1, 1);
            instantiatedProjectile.GetComponent<orbRotateClockwisePlayer>().enabled = false;
            instantiatedProjectile.GetComponent<orbRotateAntiClockwisePlayer>().enabled = false;
            instantiatedProjectile.GetComponent<ProjectileCollider>().enabled = true;

            statusProjectile(instantiatedProjectile, projectileType);

            // Set velocity
            instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, projectileSpeed));
            // Ignore collision with the character collider
            Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    // atribuir status ao projetil
    void statusProjectile(Rigidbody projectile, string projectileType)
    {
        projectile.gameObject.tag = "ProjectilePlayer";
        projectile.GetComponent<ProjectileCollider>().elementType = projectileType;

        if (activeExtraStatus && (firstElementType == "Fire" || secondElementType == "Fire"))
        {
            projectile.GetComponent<ProjectileCollider>().damage = damageAttack * 2;
        }
        else
        {
            projectile.GetComponent<ProjectileCollider>().damage = damageAttack;
        }
    }

    void attack()
    {
        attacking = true;
        animator.Play("attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("top") || other.gameObject.name.Equals("bottom"))
        {
            Vector3 diff = other.gameObject.transform.position - gameObject.transform.position;
            diff = new Vector3(0, 0, diff.z);
            gameObject.transform.position -= Vector3.Normalize(diff) * 0.8f;
        }
        else if (other.gameObject.name.Equals("right") || other.gameObject.name.Equals("left"))
        {
            Vector3 diff = other.gameObject.transform.position - gameObject.transform.position;
            diff = new Vector3(diff.x, 0, 0);
            gameObject.transform.position -= Vector3.Normalize(diff) * 0.8f;
        }


        if (other.gameObject.tag == "ProjectileEnemy")
        {
            print("life: " + life);
            print("shield: " + shield);
            float damage = other.gameObject.GetComponent<ProjectileCollider>().damage;
            string elementType = other.gameObject.GetComponent<ProjectileCollider>().elementType;

            // fraqueza 50%
            if (elementType == "Fire" && firstElementType == "Nature" && secondElementType == "Nature")
            {
                subLife(damage, 1.5f);
            }
            else if (elementType == "Nature" && firstElementType == "Earth" && secondElementType == "Earth")
            {
                subLife(damage, 1.5f);
            }
            else if (elementType == "Earth" && firstElementType == "Lightning" && secondElementType == "Lightning")
            {
                subLife(damage, 1.5f);
            }
            else if (elementType == "Ligthning" && firstElementType == "Water" && secondElementType == "Water")
            {
                subLife(damage, 1.5f);
            }
            else if (elementType == "Water" && firstElementType == "Fire" && secondElementType == "Fire")
            {
                subLife(damage, 1.5f);
            }
            // resistencia 50%
            else if (elementType == "Fire" && (firstElementType == "Water" || secondElementType == "Water"))
            {
                subLife(damage, 0.5f);
            }
            else if (elementType == "Water" && (firstElementType == "Lightning" || secondElementType == "Ligthning"))
            {
                subLife(damage, 0.5f);
            }
            else if (elementType == "lightning" && (firstElementType == "Earth" || secondElementType == "Earth"))
            {
                subLife(damage, 0.5f);
            }
            else if (elementType == "Earth" && (firstElementType == "Nature" || secondElementType == "Nature"))
            {
                subLife(damage, 0.5f);
            }
            else if (elementType == "Nature" && (firstElementType == "Fire" || secondElementType == "Fire"))
            {
                subLife(damage, 0.5f);
            }
            else
            {
                subLife(damage, 1f);
            }
        }
    }

    void subLife(float damage, float mul)
    {
        if (cooldownImunity > 0)
        {
            mul = 0;
        }

        if (shield > 0)
        {
            shield -= mul * damage;
        }
        else
        {
            life -= mul * damage;
        }

        
    }

    void updateAnimation()
    { 
        float angleY = transform.rotation.eulerAngles.y;

        if (input.x == 0 && input.z == 0)
        {
            float diffAngleY = previousAngleY - angleY;

            if (diffAngleY > 0.5f) // giro para direita
            {
                animator.Play("turn_left");
            }
            else if (diffAngleY < -0.5f) // 
            {
                animator.Play("turn_right");
            } 
            else
            {
                animator.Play("idle");
            }
        }
        
        previousAngleY = angleY;

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
                animator.Play("walk_right_backward");
            }
            else if (angleY > 158 && angleY <= 203)
            {
                animator.Play("walk_right");
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
        else if (input.z > 0 && input.x < 0)
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
