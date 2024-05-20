using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class Teammate_AI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform enemyContainer;
    public Transform player;
    public Transform target;
    private Transform shotPoint;
    private Transform enemy;

    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy, whatIsWall;

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
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        enemyContainer = GameObject.Find("Enemy Container").transform;
    }

    private void Update()
    {
        if (enemyContainer.transform.childCount != 0)
        {
            getClosestEnemy();
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
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        // Inside Update function, modify the conditions for chasing the player
        if (enemyInAttackRange && !enemyBehindWall)
        {
            AttackEnemy();
        }
        else
        {
            ChasePlayer();
        }
    }

    public void getClosestEnemy()
    {
        enemy = enemyContainer.GetChild(0);
        for (int i = 0; i < enemyContainer.transform.childCount; i++)
        {
            if (Vector3.Distance(transform.position, enemy.position) > Vector3.Distance(transform.position, enemyContainer.GetChild(i).position))
            {
                enemy = enemyContainer.GetChild(i);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Destroy(this.gameObject);
            //Debug.Log("brrrrr");
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }
    private void AttackEnemy()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(enemy);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, shotPoint.transform.position, transform.rotation * projectile.transform.rotation, GameObject.Find("Bullets").transform).GetComponentInChildren<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
