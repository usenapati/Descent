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

    [Header("Camera Follow Sensitivity")]
    // Mouse Sensitivity
    public float mouseXFollowSensitivity = 0.06f;
    public float mouseYFollowSensitivity = 0.16f;
    // Controller Sensistivity
    public float controllerXFollowSensitivity = 3f;
    public float controllerYFollowSensitivity = 1f;

    [Header("Camera Aim Sensitivity")]
    // Mouse Sensitivity
    public float mouseXAimSensitivity = 0.06f;
    public float mouseYAimSensitivity = 0.16f;
    // Controller Sensistivity
    public float controllerXAimSensitivity = 3f;
    public float controllerYAimSensitivity = 1f;

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
            cameraFollow.enabled = false;
            cameraAim.enabled = true;
            //cameraFollow.gameObject.SetActive(false);
            //cameraAim.gameObject.SetActive(true);
            currentCamera = cameraAim;
            currentCamera.Priority = 10;
        }
        else
        {

            currentCamera.Priority = 9;
            cameraAim.enabled = false;
            cameraFollow.enabled = true;
            //cameraAim.gameObject.SetActive(false);
            //cameraFollow.gameObject.SetActive(true);
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
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = controllerXFollowSensitivity;
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = controllerYFollowSensitivity;
        }
        else
        {
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseXFollowSensitivity;
            cameraFollow.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseYFollowSensitivity;
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
