using UnityEngine;
using System.Collections;

namespace MOBA
{
    public class Flee : SteeringBehaviour
    {
        // This behavior requires a target
        public Transform target;

        public override Vector3 GetForce()
        {
            Vector3 force = Vector3.zero;

            // If there is no target, return zero force
            if (target == null) return force;

            Vector3 desiredVelocity;
            Vector3 direction = target.position - owner.transform.position;

            // Invert direction
            direction *= -1;

            // Check if the direction is valid
            if (direction != Vector3.zero)
            {
                desiredVelocity = direction.normalized * weighting;
                force = desiredVelocity - owner.velocity;
            }

            return force;
        }
    }
}