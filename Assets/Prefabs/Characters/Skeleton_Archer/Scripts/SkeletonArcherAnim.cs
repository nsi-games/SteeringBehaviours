using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MOBA
{
    [RequireComponent(typeof(AIAgent))]
    public class SkeletonArcherAnim : MonoBehaviour
    {
        public Animator anim;

        private AIAgent aiAgent;

        // Use this for initialization
        void Start()
        {
            aiAgent = GetComponent<AIAgent>();
            aiAgent.updatePosition = false;
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("spawn"))
            {
                aiAgent.updatePosition = true;
                float moveSpeed = aiAgent.velocity.magnitude;
                anim.SetFloat("MoveSpeed", moveSpeed);
            }
        }
    }
}