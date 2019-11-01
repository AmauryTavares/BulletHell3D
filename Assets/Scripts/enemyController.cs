using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    
    public GameObject projectile;
    public string element;


    public float velocity = 4;
    public float vision = 50f;
    public float life = 500f;
    public float damageAttack = 30f;
    public float cooldownAttack = 3f;
    public float cooldownAttackMax = 3f;

    private Quaternion targetRotation;
    private float cooldownDestroy = 4f;
    private float angleRotation;
    private float turnspeed = 100;
    private float projectileSpeed = 15;
    private bool attacking = false;
    private bool spawnedProjectile = false;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isDead();
        reduceTime();

        if (dead) return;

        float angle = CalculateDirectionRotation();
        Rotate();

        if (attacking) return;

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            die();
            return;
        }*/

        float dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (dist > 25f && dist <= vision)
        {
            if (angle <= 10)
            {
                animator.Play("Run");
                Vector3 diff = player.transform.position - gameObject.transform.position;
                gameObject.transform.position += Vector3.Normalize(diff) * velocity * Time.deltaTime;
            }

        }
        else if (dist <= 25f)
        {
            animator.Play("Idle");
            attack();
        } 
        else
        {
            animator.Play("Idle");
        }
    }

    void reduceTime()
    {
        if (!dead && attacking && cooldownAttack > 0)
        {
            cooldownAttack -= Time.deltaTime;

            if (!spawnedProjectile && cooldownAttack < (cooldownAttackMax / 2))
            {
                animator.Play("CastSpell");
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

        if (dead)
        {
            gameObject.transform.localScale = gameObject.transform.localScale - new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
            cooldownDestroy -= Time.deltaTime;

            if (cooldownDestroy <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    void spawnProjectile()
    {
        Vector3 projectilePos = transform.position + new Vector3(0, 3, 0);
        Quaternion projectileRot = transform.rotation * Quaternion.Euler(-90, 0, 0); ;

        Rigidbody rigidbodyProjectile = projectile.GetComponent<Rigidbody>();
        Rigidbody instantiatedProjectile = Instantiate(rigidbodyProjectile, projectilePos, projectileRot);
        instantiatedProjectile.transform.localScale = new Vector3(1, 1, 1);
        instantiatedProjectile.GetComponent<orbRotateClockwisePlayer>().enabled = false;
        instantiatedProjectile.GetComponent<orbRotateAntiClockwisePlayer>().enabled = false;
        instantiatedProjectile.GetComponent<ProjectileCollider>().enabled = true;

        statusProjectile(instantiatedProjectile);

        // Set velocity
        instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, projectileSpeed));
        // Ignore collision with the character collider
        Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // atribuir status ao projetil
    void statusProjectile(Rigidbody projectile)
    {
        projectile.gameObject.tag = "ProjectileEnemy";
        projectile.GetComponent<ProjectileCollider>().elementType = element;
        projectile.GetComponent<ProjectileCollider>().damage = damageAttack;
    }

    void attack()
    {
        attacking = true;
    }

    void isDead()
    {
        if (life <= 0)
        {
            die();
        }
    }

    void die()
    {
        dead = true;
        animator.Play("Die");
    }

    float CalculateDirectionRotation()
    {
        Vector3 diff = player.transform.position - transform.position;

        angleRotation = Mathf.Atan2(diff.x, diff.z);
        angleRotation = Mathf.Rad2Deg * angleRotation;
        //print(player.transform.position.x + " " + player.transform.position.z);
        return angleRotation;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angleRotation, 0);
        //print(transform.rotation + " " + targetRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnspeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ProjectilePlayer")
        {
            float damage = other.gameObject.GetComponent<ProjectileCollider>().damage;
            string elementType = other.gameObject.GetComponent<ProjectileCollider>().elementType;

            // fraqueza 50%
            if (elementType == "Fire" && element == "Nature")
            {
                life -= 1.5f * damage;
            } 
            else if (elementType == "Nature" && element == "Earth")
            {
                life -= 1.5f * damage;
            }
            else if (elementType == "Earth" && element == "Ligthning")
            {
                life -= 1.5f * damage;
            }
            else if (elementType == "Ligthning" && element == "Water")
            {
                life -= 1.5f * damage;
            }
            else if (elementType == "Water" && element == "Fire")
            {
                life -= 1.5f * damage;
            }
            // resistencia 50%
            else if (elementType == "Fire" && element == "Water")
            {
                life -= 0.5f * damage;
            }
            else if (elementType == "Water" && element == "lightning")
            {
                life -= 0.5f * damage;
            }
            else if (elementType == "lightning" && element == "Earth")
            {
                life -= 0.5f * damage;
            }
            else if (elementType == "Earth" && element == "Nature")
            {
                life -= 0.5f * damage;
            }
            else if (elementType == "Nature" && element == "Fire")
            {
                life -= 0.5f * damage;
            }
            else
            {
                life -= damage;
            }

        }
    }

}
