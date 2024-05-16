using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class Teammate_AI : MonoBehaviour
{
    public NavMeshAgent agent1;

    public Transform player;
    public Transform enemy;
    private Transform shotPoint;

    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy, whatIsWall;


    public float health;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, enemyInAttackRange, enemyBehindWall;

    //useless and delete later
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        shotPoint = gameObject.transform.GetChild(0);
        agent1 = GetComponent<NavMeshAgent>();
        agent1.autoBraking = false;
    }

    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        RaycastHit hit;
        Debug.DrawRay(transform.position, (enemy.position - transform.position), Color.red, Time.deltaTime);
        if (Physics.Raycast(transform.position, (enemy.position - transform.position), out hit, 20, whatIsWall))
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("WhatIsWall"))
            {
                //Debug.Log("Player is behind a wall");
                enemyBehindWall = true;
            }
            else
            {
                //Debug.Log("Player is within vision");
                enemyBehindWall = false;
            }
        }
        else
        {
            //Debug.Log("No obstacles between enemy and player");
            enemyBehindWall = false;
        }
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        // Inside Update function, modify the conditions for chasing the player
        ChasePlayer();
        if (enemyInAttackRange && !enemyBehindWall)
        {
            AttackEnemy();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
            //Debug.Log("brrrrr");
        }
    }
    private void ChasePlayer()
    {
        agent1.SetDestination(player.position);
    }
    private void AttackEnemy()
    {
        //Make sure enemy doesn't move
        agent1.SetDestination(transform.position);

        transform.LookAt(enemy);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, shotPoint.transform.position, transform.rotation * projectile.transform.rotation).GetComponentInChildren<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);


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
