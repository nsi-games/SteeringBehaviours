using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using GGL;

namespace MOBA
{
    public class PathFollowing : SteeringBehaviour
    {
        public Transform target;
        public float nodeRadius = .1f;
        public float targetRadius = 3f;
        private int currentNode = 0;
        private bool isAtTarget = false;
        private NavMeshAgent nav;

        private NavMeshPath path;

        protected override void Awake()
        {
            base.Awake();
            nav = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (path != null)
            {
                Vector3[] corners = path.corners;
                if (corners.Length > 0)
                {
                    Vector3 targetPos = corners[corners.Length - 1];
                    
                    // Draw target
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(targetPos, targetRadius);

                    GizmosGL.color = new Color(1, 0, 0, 0.3f);
                    GizmosGL.AddSphere(targetPos, targetRadius);

                    float distance = Vector3.Distance(transform.position, targetPos);
                    if (distance >= targetRadius)
                    {
                        GizmosGL.color = Color.cyan;
                        for (int i = 0; i < corners.Length - 1; i++)
                        {
                            GizmosGL.AddLine(corners[i], corners[i + 1], 0.1f, 0.1f);
                            GizmosGL.AddSphere(corners[i + 1], 1f);


                            GizmosGL.color = Color.red;
                        }
                    }
                }
            }
        }

        #region SEEK
        // Performs seek operation using target to pass
        Vector3 Seek(Vector3 target)
        {
            // SET force to zero
            Vector3 force = Vector3.zero;

            // SET desiredForce to target - transform's position
            Vector3 desiredForce = target - transform.position;

            // SET distance to zero
            // IF isAtTarget
            // SET distance to targetRadius
            // ELSE
            // SET distance to nodeRadius
            float distance = isAtTarget ? targetRadius : nodeRadius;

            // IF desiredForce's length is greater than distance
            if (desiredForce.magnitude > distance)
            {
                // SET desiredForce to desiredForce.normalized * weighting
                desiredForce = desiredForce.normalized * weighting;
                // SET force to desiredForce - owner's velocity
                force = desiredForce - owner.velocity;
            }

            // Return force
            return force;
        }
        #endregion

        // Calculates force for behaviour
        public override Vector3 GetForce()
        {
            // SET force to zero
            Vector3 force = Vector3.zero;

            if (target == null)
                return force;

            // IF path is not null AND path count is greater than zero
            if (nav.CalculatePath(target.position, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    Vector3[] corners = path.corners;
                    if (corners.Length > 0)
                    {
                        // IF currentNode is greater than or equal to path.Count
                        if (currentNode >= corners.Length)
                        {
                            // SET currentNode to path.Count - 1
                            currentNode = corners.Length - 1;
                        }

                        // SET currPos to current path node's position
                        Vector3 currPos = corners[currentNode];
                        // SET isAtTarget to currentNode == path.Count - 1
                        isAtTarget = currentNode == corners.Length - 1;
                        // IF distance between transform's position and currentPos is less than or equal to nodeDistance
                        if (Vector3.Distance(transform.position, currPos) <= nodeRadius)
                        {
                            // Increment currentNode
                            currentNode++;
                        }

                        // SET force to Seek() and pass currPos
                        force = Seek(currPos);
                    }
                }
            }

            // RETURN force
            return force;
        }
    }
}