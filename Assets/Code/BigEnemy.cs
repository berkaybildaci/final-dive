using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class BigEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    private Transform shotPoint;

    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;
    private Animator anim;
    //States
    private float sightRange = 15, attackRange = 5;
    public bool playerInSightRange, playerInAttackRange, playerBehindWall;
    private bool kill, started = false;
    private float killTime = 1f;
    private float timeTillDeath = 0f;

    //useless and delete later
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        shotPoint = gameObject.transform.GetChild(0);
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        anim = GetComponentInChildren<Animator>();
        transform.forward = -transform.forward;
    }

    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        RaycastHit hit;
        Debug.DrawRay(transform.position, (player.position - transform.position), Color.red, Time.deltaTime);
        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, 20, whatIsWall))
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("WhatIsWall"))
            {
                //Debug.Log("Player is behind a wall");
                playerBehindWall = true;
            }
            else
            {
                //Debug.Log("Player is within vision");
                playerBehindWall = false;
            }
        }
        else
        {
            //Debug.Log("No obstacles between enemy and player");
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
            agent.SetDestination(transform.position);
        }
        if(kill == true && started == false)
        {
            started = true;
            timeTillDeath = Time.time;
        }
        else if(started && Time.time - timeTillDeath >= killTime)
        {
            player.GetComponent<EntityData>().damage(100);
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player.transform);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        anim.SetTrigger("attack");
        if(Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            kill = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
