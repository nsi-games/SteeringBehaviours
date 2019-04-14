using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GGL;

namespace MOBA
{
    public class CollisionAvoidance : SteeringBehaviour
    {
        public float intersectForce = 1f;
        public float maxAvoidForce = 5f;
        public float maxSeeDistance = 10f;
        public Vector3 rayOffset = new Vector3(0, 0.5f, 0);
        public LayerMask collisionLayer;

        public override Vector3 GetForce()
        {
            Vector3 force = Vector3.zero;
            Vector3 centerPoint = transform.position + rayOffset;
            Vector3 ahead = centerPoint + transform.forward.normalized * maxSeeDistance;
            Ray ray = new Ray(centerPoint, transform.forward);

            //Line line = GizmosGL.AddLine(ray.origin, ray.origin + ray.direction * maxSeeDistance);
            //line.color = Color.cyan;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxSeeDistance, collisionLayer))
            {
                //line.color = Color.red;
                //Vector3 center = hit.point + -hit.normal * intersectForce;
                Vector3 center = hit.collider.transform.position;
                Vector3 direction = ahead - center;
                Vector3 avoidance = direction.normalized * maxAvoidForce;
                avoidance *= maxAvoidForce;
                Collider col = hit.collider;
                Bounds bounds = col.bounds;
                GizmosGL.AddCube(center, bounds.size *2);
                GizmosGL.AddSphere(center, 0.5f);
                GizmosGL.AddLine(transform.position, transform.position + avoidance * 10f, 1, 1, Color.red, Color.red);
                Vector3 desiredVelocity = avoidance.normalized;
                
                force = Vector3.zero;
                force += desiredVelocity;
            }

            return force;
        }
    }
}