using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Animator animator;
    
    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
       
        //if (playerMovement.state == PlayerMovementAdvanced.MovementState.air ||
        //    playerMovement.state == PlayerMovementAdvanced.MovementState.wallrunning)
        //{
        //    animator.SetBool("IsGrounded", false);
        //}
        //else
        //{
        //    animator.SetBool("IsGrounded", true);
        //}

    }
}
