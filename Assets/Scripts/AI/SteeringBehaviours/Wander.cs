using UnityEngine;
using System.Collections;

using GGL;

namespace MOBA
{
    public class Wander : SteeringBehaviour
    {
        // Public:
        public float offset = 1.0f;
        public float radius = 1.0f;
        public float jitter = 0.2f;

        // Private:
        private Vector3 targetDir;
        private Vector3 randomDir;

        public override Vector3 GetForce()
        {
            Vector3 force = Vector3.zero;

            /*
            -32767                0                     32767
            |---------------------|---------------------|
                      |_______________________|
                            Random Range
            */

            // 0x7fff = 32767
            float randX = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);
            float randZ = Random.Range(0, 0x7fff) - (0x7fff * 0.5f);

            #region Calculate RandomDir
            // Create the random directon vector
            randomDir = new Vector3(randX, 0, randZ);
            // Normalize the random direction
            randomDir = randomDir.normalized;
            // Multiply jitter to randomDir
            randomDir *= jitter;
            #endregion

            #region Calculate TargetDir
            // Append target dir with random dir
            targetDir += randomDir;
            // Normalize the target dir
            targetDir.Normalize();
            // Amplify it by the radius
            targetDir *= radius;
            #endregion

            // Calculate seek position using targetDir
            Vector3 seekPos = transform.position + targetDir;
            seekPos += transform.forward.normalized * offset;

            #region GizmosGL
            Vector3 forwardPos = transform.position + transform.forward.normalized * offset;
            Circle circle = GizmosGL.AddCircle(forwardPos + Vector3.up * 0.1f, radius, Quaternion.LookRotation(Vector3.down));
            circle.color = new Color(1, 0, 0, 0.5f);
            circle = GizmosGL.AddCircle(seekPos + Vector3.up * 0.15f, radius * 0.6f, Quaternion.LookRotation(Vector3.down));
            circle.color = new Color(0, 0, 1, 0.5f);
            #endregion

            // Calculate direction
            Vector3 desiredForce;
            Vector3 direction = seekPos - transform.position;

            // Check if direction is valid
            if (direction != Vector3.zero)
            {
                desiredForce = direction.normalized * weighting;
                force = desiredForce - owner.velocity;
            }

            return force;
        }
    }
}