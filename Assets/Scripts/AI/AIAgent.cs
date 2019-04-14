using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace MOBA
{
    public class AIAgent : MonoBehaviour
    {
        public float maxSpeed = 10f;
        public float maxDistance = 5f;
        public bool updatePosition = false;
        public bool updateRotation = false;

        [HideInInspector] public Vector3 velocity;

        private Vector3 force;
        private List<SteeringBehaviour> behaviours;
        private NavMeshAgent nav;

        // Use this for initialization
        void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            behaviours = new List<SteeringBehaviour>(GetComponents<SteeringBehaviour>());
        }

        void ComputeForces()
        {
            // SET force = Vector3.zero
            force = Vector3.zero;
            // FOR i := 0 < behaviours.Count
            for (int i = 0; i < behaviours.Count; i++)
            {
                // LET behaviour = behaviours[i]
                SteeringBehaviour behaviour = behaviours[i];
                // IF behaviour.isActiveAndEnabled == false
                if (!behaviour.isActiveAndEnabled)
                {
                    // CONTINUE
                    continue;
                }
                // SET force = force + behaviour.GetForce() * behaviour.weighting
                force += behaviour.GetForce() * behaviour.weighting;
                // IF force.magnitude > maxSpeed
                if (force.magnitude > maxSpeed)
                {
                    // SET force = force.normalized * maxSpeed
                    force = force.normalized * maxSpeed;
                    // BREAK
                    break;
                }
            }
        }

        void ApplyVelocity()
        {
            // SET velocity = velocity + force * deltaTime
            velocity += force * Time.deltaTime;
            nav.speed = velocity.magnitude;

            // IF velocity.magnitude > maxSpeed
            if (velocity.magnitude > maxSpeed)
            {
                // SET velocity = velocity.normalized * maxSpeed
                velocity = velocity.normalized * maxSpeed;
            }
            // IF velocity.magnitude > 0
            if(velocity.magnitude > 0)
            {
                // LET pos = transform.position + velocity * deltaTime
                Vector3 pos = transform.position + velocity;
                // LET navHit;
                NavMeshHit navHit;
                // CALL NavMesh.SamplePosition(pos, out navHit, maxDistance, -1);
                if (NavMesh.SamplePosition(pos, out navHit, maxDistance, -1))
                {
                    // CALL nav.SetDestination(navHit.position)
                    nav.SetDestination(navHit.position);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            nav.updatePosition = updatePosition;
            nav.updateRotation = updateRotation;
            ComputeForces();
            ApplyVelocity();
        }
    }
}