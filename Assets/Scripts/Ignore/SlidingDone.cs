using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDone : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public CapsuleCollider playerObj;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;
    private InputManager inputManager;
    private CameraManager cameraManager;
    Camera camera;

    [Header("Sliding")]
    public float maxSlideTime = 0.75f;
    public float slideForce = 200f;
    public float slideTimer;

    public float slideYHeight = 0.5f;
    private float startYHeight;

    
    private float horizontalInput;
    private float verticalInput;
    private bool slidingInput;
    public bool readyToSlide;
    public float slideCooldown;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        inputManager = GetComponent<InputManager>();
        cameraManager = GetComponent<CameraManager>();
        startYHeight = playerObj.transform.localScale.y;
        camera = Camera.main;
        readyToSlide = true;
    }

    private void Update()
    {
        horizontalInput = inputManager.horizontalInput;
        verticalInput = inputManager.verticalInput;
        slidingInput = inputManager.slide_Input;

        if (slidingInput && (horizontalInput != 0 || verticalInput != 0) && readyToSlide)
        //if (slidingInput)
            StartSlide();

        if (!slidingInput && pm.sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    public void StartSlide()
    {
        if (pm.wallrunning) return;

        pm.sliding = true;

        readyToSlide = false;

        playerObj.height = slideYHeight;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        //Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 inputDirection;
        inputDirection = camera.transform.forward * verticalInput + camera.transform.right * horizontalInput;
        inputDirection.y = 0;

        // sliding normal
        //if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        if (!pm.OnSlope())
        {
            Debug.Log("Subtract Slide Time");
            slideTimer -= Time.deltaTime;

            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);

            // does that make any difference?
            // if (rb.velocity.y > 0) rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // stop sliding again
        if (slideTimer <= 0)
            StopSlide();
    }

    public void StopSlide()
    {
        Debug.Log("Stop Sliding");
        pm.sliding = false;

        slidingInput = false;

        pm.state = PlayerMovementAdvanced.MovementState.walking;
        Invoke(nameof(ResetSlide), slideCooldown);
        //inputManager.slide_Input = false;

        playerObj.height = startYHeight;
    }

    private void ResetSlide()
    {
        readyToSlide = true;

    }
}
