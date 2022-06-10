using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    InputManager inputManager;
    NextGenWallRunning wallRunning;

    public LayerMask aimColliderLayerMask;
    //public Transform debugTransform;
    public Transform spawnBulletTransform;
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        wallRunning = GetComponent<NextGenWallRunning>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) 
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (inputManager.aim_Input)
        {
            wallRunning.enabled = false;
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (inputManager.shoot_Input)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletTransform.position).normalized;
                Instantiate(projectilePrefab, spawnBulletTransform.position, Quaternion.LookRotation(aimDir ,Vector3.up));
                inputManager.shoot_Input = false;
            }
        }
        else
        {
            wallRunning.enabled = true;
        }
    }
}
