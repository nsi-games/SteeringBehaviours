using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using GGL;

namespace MOBA
{
    [RequireComponent(typeof(Camera))]
    public class AIAgentDirector : MonoBehaviour
    {
        public LayerMask hitLayers;
        public float rayDistance = 1000f;
        public AIAgent[] agentsToDirect;
        private Camera cam;
        private Transform selectionPoint;

        private bool isMouseDown = false;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void Start()
        {
            GameObject newGameObject = new GameObject("Target Location");
            selectionPoint = newGameObject.transform;
        }
        
        // Update is called once per frame
        void Update()
        {
            //GizmosGL.AddSphere(selectionPoint.position, .1f, null, new Color(1, 0, 0, 0.2f));

            if (Input.GetMouseButtonDown(0))
            {
                Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit rayHit;
                if (Physics.Raycast(camRay, out rayHit, rayDistance, hitLayers))
                {
                    int areaMask = NavMesh.GetAreaFromName("Walkable");
                    NavMeshHit navHit;
                    // Check Nav position
                    if (NavMesh.SamplePosition(rayHit.point, out navHit, rayDistance, -1))
                    {
                        selectionPoint.position = navHit.position;
                        AssignTargetToAllAgents(selectionPoint);
                    }
                }
            }
        }
        
        // Assigns target to all agents in 'agentsToDirect'
        void AssignTargetToAllAgents(Transform target)
        {
            foreach (var agent in agentsToDirect)
            {
                Seek seek = agent.GetComponent<Seek>();
                PathFollowing pathFollowing = agent.GetComponent<PathFollowing>();
                // If agent has seek attached
                if (seek != null)
                {
                    // Set seek target
                    seek.target = target;
                }
                // If agent has pathfollowing attached
                if(pathFollowing != null)
                {
                    // Set pathfollowing target
                    pathFollowing.target = target;
                }
            }
        }
    }
}