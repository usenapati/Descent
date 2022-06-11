using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Transform playerTransform;
    private Rigidbody rb;
    private AnimatorManager animatorManager;
    NavMeshAgent agent;
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    public GameObject projectile;

    public float health;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = playerTransform.position;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);



        if(!playerInSightRange && !playerInAttackRange)
        {
            //Debug.Log("Patrol");
            Patrolling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            //Debug.Log("Chase");
            ChasePlayer();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            //Debug.Log("Attack");
            AttackPlayer();
        }


    }

    private void LateUpdate()
    {
        //Debug.Log(agent.velocity.magnitude);
        //float verticalInput = agent.velocity.z;
        //float horizontalInput = agent.velocity.x;

        ////float moveAmount = Mathf.Clamp(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput), 0, 5);
        //float moveAmount = agent.velocity.magnitude;
        ////Debug.Log(moveAmount);
        //animatorManager.UpdateEnemyAnimatorValues(0, Mathf.Abs(moveAmount), false);

        float time = Time.deltaTime;
        Vector3 currentPosition = transform.position;

        Vector3 movement = lastPosition - currentPosition;
        Vector3 localMovement = transform.InverseTransformVector(movement);
        Vector3 local3DSpeed = localMovement / time;


        animatorManager.UpdateEnemyAnimatorValues(0, Mathf.Abs(local3DSpeed.x) + Mathf.Abs(local3DSpeed.z), false);
        //animator.SetFloat("LocalY", local3DSpeed.y); // Up / Down movement

        //
        // Update the lastPosition at the end
        lastPosition = currentPosition;


    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint,-transform.up,2f,whatIsGround))
        {
            walkPointSet = true;
        }
        
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if(!alreadyAttacked)
        {
            Vector3 offset = transform.position;
            offset.z += 0.5f;
            offset.y -= 0.25f;

            // Play Shooting Animation

            GameObject p = Instantiate(projectile, offset + (transform.up*0.5f), Quaternion.identity);
            Rigidbody rb = p.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32.0f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8.0f, ForceMode.Impulse);
            Destroy(p, 0.75f);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
