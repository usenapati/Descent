using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour
{
    public CameraManager cameraManager;
    private Animator animator;
    private PlayerMovementAdvanced pm;
    private NextGenWallRunning wr;
    private SlidingDone sliding;
    private InputManager inputManager;
    private Rigidbody rb;


    float horizontalInput;
    float verticalInput;

    // Rotation
    public Vector3 rot;

    // Min speed, when the player is on 0 deg or whatever minAngle you have
    public float lowSpeed = 12.5f;
    // Max speed
    public float highSpeed = 13.8f;

    // Max drag, if the player is on 0 deg or minAngle
    public float maxDrag = 6;
    // Min drag
    public float minDrag = 2;

    // Here we will store the modified force and drag
    private float mod_force;
    private float mod_drag;

    // Min angle for the player to rotate on the x-axis
    public float minAngle = 0;
    // Max angle
    public float maxAngle = 45;

    public float minJumpHeight = 2f;
    public LayerMask whatIsGround;

    // Converting the x rotation from min angle to max, into a 0-1 format.
    // 0 means minAngle
    // 1 means maxAngle
    public float percentage;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        wr = GetComponent<NextGenWallRunning>();
        sliding = GetComponent<SlidingDone>();
        // Make sure the player has a Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Setting rotation variable to the current angles
        rot = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        if(AboveGround() && inputManager.glide_Input)
        {
            //
            animator.SetBool("IsGliding", true);
            pm.enabled = false;
            wr.enabled = false;
            sliding.enabled = false;

            rot = transform.rotation.eulerAngles;
            horizontalInput = inputManager.horizontalInput;
            verticalInput = inputManager.verticalInput;
            // Rotation
            // Y
            rot.y += 60 * horizontalInput * Time.deltaTime;
            // Z
            rot.z = -5 * horizontalInput;
            // Limiting the z-axis
            rot.z = Mathf.Clamp(rot.z, -5, 5);
            // X
            rot.x += 40 * verticalInput * Time.deltaTime;
            // Limiting x-axis
            rot.x = Mathf.Clamp(rot.x, minAngle, maxAngle);
            // Update rotation
            transform.rotation = Quaternion.Euler(rot);

            // Speed and drag based on angle
            // Get the percentage (minAngle = 0, maxAngle = 1)
            percentage = rot.x / maxAngle;
            // Update parameters
            // If 0, we'll have maxDrag and lowSpeed
            // If 1, we'll get minDrag and highSpeed
            mod_drag = 0.5f*(percentage * (minDrag - maxDrag)) + maxDrag;
            mod_force = 0.5f * ((percentage * (highSpeed - lowSpeed)) + lowSpeed);
            // Getting the local space of the velocity
            Vector3 localV = transform.InverseTransformDirection(rb.velocity);
            // Change z velocity to mod_force
            localV.z = mod_force;
            // Convert the local velocity back to world space and set it to the Rigidbody's velocity
            rb.velocity = transform.TransformDirection(localV);
            // Update drag to the modified one
            rb.drag = mod_drag;
        }
        else if(!AboveGround())
        {
            inputManager.glide_Input = false;
            pm.enabled = true;
            wr.enabled = true;
            sliding.enabled = true;
        }
        else if (!inputManager.glide_Input)
        {
            // Enable Movement
            animator.SetBool("IsGliding", false);
            pm.enabled = true;
            wr.enabled = true;
            sliding.enabled = true;
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }
}
