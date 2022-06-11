using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Transform playerTransform;
    public Transform muzzleTransform;
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

    Vector3 worldDeltaPosition;
    Vector2 groundDeltaPosition;
    Vector2 smoothDeltaPosition = Vector2.zero;
    public Vector2 velocity = Vector2.zero;

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
        agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        worldDeltaPosition = agent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.up, worldDeltaPosition);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, groundDeltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.025f && agent.remainingDistance > agent.radius;
        //velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector3.zero;

        animatorManager.animator.SetBool("IsMoving", shouldMove);
        animatorManager.animator.SetFloat("Horizontal", Mathf.Clamp(velocity.x, -1, 1));
        animatorManager.animator.SetFloat("Vertical", Mathf.Clamp(velocity.y, 0, 6.5f));


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

    private void OnAnimatorMove()
    {
        transform.position = agent.nextPosition;
        //Vector3 position = animatorManager.animator.rootPosition;
        //position.y = agent.nextPosition.y;
        //transform.position = position;
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

        if (!alreadyAttacked)
        {
            Vector3 offset = muzzleTransform.position;
            //offset.z += 0.5f;
            //offset.y -= 0.25f;

            // Play Shooting Animation
            animatorManager.PlayTargetAnimation("FireRifle", false);

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
