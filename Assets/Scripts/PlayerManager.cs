using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    //PlayerLocomotion playerLocomotion;
    PlayerMovementAdvanced playerMovement;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        //playerLocomotion = GetComponent<PlayerLocomotion>();
        playerMovement = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        //inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        //playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if (cameraManager != null)
        {
            cameraManager.HandleAllCameraMovement();
        }

        isInteracting = animator.GetBool("IsInteracting");
        //playerLocomotion.isJumping = animator.GetBool("IsJumping");
        if (playerMovement.state == PlayerMovementAdvanced.MovementState.air ||
            playerMovement.state == PlayerMovementAdvanced.MovementState.wallrunning)
        {
            animator.SetBool("IsGrounded", false);
        }
        else
        {
            animator.SetBool("IsGrounded", true);
        }
        
    }
}
