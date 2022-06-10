using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    [Header("Camera Transform References")]
    public Transform targetTransform; // The object the camera will follow
    public Transform cameraPivotTransform; // The object the camera will pivot
    public CinemachineVirtualCamera cameraFollow; // The transform of the follow camera
    public CinemachineVirtualCamera cameraAim; // The transform of the aim camera
    
    public CinemachineVirtualCamera currentCamera; // The transform of the aim camera

    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    [Header("Camera Collision Values")]
    public LayerMask collisionLayers;
    public float cameraCollisionOffset = 0.2f; // How much the camera will jump off of objects its colliding with
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 0.2f;

    [Header("Camera Speed Values")]
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2; // sensX
    public float cameraPivotSpeed = 2; // sensY

    [Header("Camera Angle Values")]
    public float lookAngle; // Camera looking up and down
    public float pivotAngle; // Camera looking left and right
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;

    public bool isAiming;
    public bool onController;

    // Mouse Sensitivity
    public float mouseXSensitivity = 0.06f;
    public float mouseYSensitivity = 0.16f;
    // Controller Sensistivity
    public float controllerXSensitivity = 3f;
    public float controllerYSensitivity = 1f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        currentCamera = cameraFollow;
        defaultPosition = currentCamera.transform.localPosition.z;
        isAiming = false;
    }

    public void HandleAllCameraMovement()
    {
        UpdateCurrentCamera();
        UpdateCameraSpeed();
        UpdateCameraSensitivity();
        RotateCamera();
    }

    private void UpdateCurrentCamera()
    {
        if (isAiming)
        {
            currentCamera.Priority = 9;
            currentCamera = cameraAim;
            currentCamera.Priority = 10;
        }
        else
        {
            currentCamera.Priority = 9;
            currentCamera = cameraFollow;
            currentCamera.Priority = 10;
        }
    }

    private void UpdateCameraSpeed()
    {
        if (inputManager.currentInput.ToLower() == "controller")
        {
            onController = true;
        }
        else if (inputManager.currentInput.ToLower() == "keyboard and mouse")
        {
            onController = false;
        }
    }

    private void UpdateCameraSensitivity()
    {
        if (onController)
        {
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = controllerXSensitivity;
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = controllerYSensitivity;
        }
        else
        {
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseXSensitivity;
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseYSensitivity;
        }
    }

    //private void FollowTarget()
    //{
    //    Vector3 targetPosition = Vector3.SmoothDamp
    //        (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
    //    transform.position = targetPosition;
    //}

    private void RotateCamera()
    {
        //Vector3 rotation;
        //Quaternion targetRotation;
        //lookAngle += (inputManager.cameraInputX * cameraLookSpeed);
        //pivotAngle -= (inputManager.cameraInputY * cameraPivotSpeed);
        //pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        //rotation = Vector3.zero;
        //rotation.y = lookAngle;
        //targetRotation = Quaternion.Euler(rotation);
        //transform.rotation = targetRotation;

        //rotation = Vector3.zero;
        //rotation.x = pivotAngle;
        //targetRotation = Quaternion.Euler(rotation);
        //cameraPivotTransform.localRotation = targetRotation;

        
    }

    //private void HandleCameraCollisions()
    //{
    //    float targetPosition = defaultPosition;
    //    RaycastHit hit;
    //    Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
    //    direction.Normalize();

    //    if (Physics.SphereCast
    //        (cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
    //    {
    //        float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
    //        targetPosition = -(distance - cameraCollisionOffset);
    //    }

    //    if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
    //    {
    //        targetPosition = targetPosition - minimumCollisionOffset;
    //    }

    //    cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
    //    cameraTransform.localPosition = cameraVectorPosition;
    //}

    public void DoFov(float endValue)
    {
        currentCamera.m_Lens.FieldOfView = endValue;
    }

    public void DoTilt(float zTilt)
    {
        //transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        
        currentCamera.m_Lens.Dutch = zTilt;
    }
}
