using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerMovementAdvanced playerMovement;
    //PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool glide_Input;
    public bool sprint_Input;
    public bool jump_Input;
    public bool crouch_Input;
    public bool slide_Input;
    public bool upward_run_Input;
    public bool downward_run_Input;
    public bool aim_Input;

    public PlayerInput playerInput;
    public string currentInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovementAdvanced>();
        //playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Glide.performed += i => glide_Input = !glide_Input;

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;

            playerControls.PlayerActions.Crouch.performed += i => crouch_Input = true;
            playerControls.PlayerActions.Crouch.canceled += i => crouch_Input = false;

            playerControls.PlayerActions.Slide.performed += i => slide_Input = true;
            playerControls.PlayerActions.Slide.canceled += i => slide_Input = false;

            playerControls.Wallrunning.UpwardsRun.performed += i => upward_run_Input = true;
            playerControls.Wallrunning.UpwardsRun.canceled += i => upward_run_Input = false;
            playerControls.Wallrunning.DownwardsRun.performed += i => downward_run_Input = true;
            playerControls.Wallrunning.DownwardsRun.canceled += i => downward_run_Input = false;

            playerControls.PlayerActions.Aim.performed += i => aim_Input = true;
            playerControls.PlayerActions.Aim.canceled += i => aim_Input = false;

            playerInput = FindObjectOfType<PlayerInput>();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCurrentScheme();
        //HandleSprintingInput();
        //HandleJumpingInput();
        //HandleCrouchingSlidingInput();
    }
    
    private void HandleCurrentScheme() {
        if (playerInput.currentControlScheme != null)
        {
            currentInput = playerInput.currentControlScheme;
        }
        
    }


    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        if (playerMovement.state == PlayerMovementAdvanced.MovementState.sprinting)
        {
            animatorManager.UpdateAnimatorValues(0, moveAmount, true);
        }
        else
        {
            animatorManager.UpdateAnimatorValues(0, moveAmount, false);
        }        
    }

    private void HandleSprintingInput()
    {
        //if (sprint_Input && moveAmount > 0.5f)
        //{
        //    //playerLocomotion.isSprinting = true;
        //}
        //else
        //{
        //    //playerLocomotion.isSprinting = false;
        //}
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
        //    //playerLocomotion.HandleJumping();
        }
    }

    private void HandleCrouchingInput()
    {
        if (crouch_Input)
        {
            crouch_Input = false;
        //    //playerLocomotion.HandleJumping();
        }
    }
    private void HandleSlidingInput()
    {
        if (slide_Input)
        {
            slide_Input = false;
        //    //playerLocomotion.HandleJumping();
        }
    }
}
