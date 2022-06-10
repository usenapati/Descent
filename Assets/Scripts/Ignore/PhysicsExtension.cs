using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dave
{
    namespace PhysicsExtension
    {
        public static class PhysicsExtension
        {
            public static Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
            {
                float gravity = Physics.gravity.y;
                float displacementY = endPoint.y - startPoint.y;
                Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

                Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
                Vector3 velocityXZ;
                float val = (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));
                if (val == 0)
                {
                    val = 0.0001f;
                }
                velocityXZ = displacementXZ / val;
                //Debug.Log("Velocity Y: " + velocityY);
                Debug.Log("Velocity XZ: " + velocityXZ);
                return velocityXZ + velocityY;
            }
        }

    }
}
