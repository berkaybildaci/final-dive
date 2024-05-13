using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
  public NavMeshAgent agent;

    public Transform player;
    private Transform shotPoint;

    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;

    
    public float health;


    //Patroling
    public UnityEngine.Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerBehindWall;

    //useless and delete later
    public TextMeshProUGUI text;
    private void Awake()
    {
        text.enabled = false;
        player = GameObject.Find("Player").transform;
        shotPoint = gameObject.transform.GetChild(0);
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        RaycastHit hit;
        Debug.DrawRay(transform.position, (player.position - transform.position), Color.red, Time.deltaTime);
        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, 20, whatIsWall))
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("WhatIsWall"))
            {
                Debug.Log("Player is behind a wall");
                playerBehindWall = true;
            }
            else
            {
                Debug.Log("Player is within vision");
                playerBehindWall = false;
            }
        }
        else
        {
            Debug.Log("No obstacles between enemy and player");
            playerBehindWall = false;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        // Inside Update function, modify the conditions for chasing the player
        if (playerInSightRange && !playerInAttackRange && !playerBehindWall)
        {
            ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange && !playerBehindWall)
        {
            AttackPlayer();
        }
        else if ((!playerInSightRange && !playerInAttackRange) || playerBehindWall)
        {
            Patroling();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
            text.enabled = true;
            Debug.Log("brrrrr");
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        UnityEngine.Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new UnityEngine.Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb  = Instantiate(projectile, shotPoint.transform.position, UnityEngine.Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*32f, ForceMode.Impulse);
            rb.AddForce(transform.up*8f, ForceMode.Impulse);
            ///

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
