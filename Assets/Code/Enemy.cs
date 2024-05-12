using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
  public NavMeshAgent agent;

    public Transform player;

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

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Physics.SphereCast(transform.position, sightRange, whatIsGround
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        LayerMask combinedMask = whatIsPlayer | whatIsWall;
        RaycastHit hit;
        Debug.DrawRay(transform.position, new Vector3((player.position.x - transform.position.x), player.position.y, (player.position.z - transform.position.z)), Color.red, Time.deltaTime);
        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, 20, combinedMask))
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform == player)
            {
                Debug.Log("Player is within vision");
                playerBehindWall = false;
            } 
            else
            {
                Debug.Log("Player is out of vision");
                playerBehindWall = true;
            }
        } 
        else
        {
            Debug.Log("raycast hit naada");
            playerBehindWall = false;
        }

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

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
            Rigidbody rb  = Instantiate(projectile, transform.position, UnityEngine.Quaternion.identity).GetComponent<Rigidbody>();
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
