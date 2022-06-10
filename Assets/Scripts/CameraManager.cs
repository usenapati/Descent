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
        isAiming = false;
    }

    public void HandleAllCameraMovement()
    {
        UpdateCurrentCamera();
        UpdateCameraSpeed();
        UpdateCameraSensitivity();
    }

    private void UpdateCurrentCamera()
    {
        isAiming = inputManager.aim_Input;
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
