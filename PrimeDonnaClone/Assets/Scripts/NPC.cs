using UnityEngine;
using UnityEngine.AI;

namespace PrimeDonna.NPCSpace
{
    [RequireComponent(typeof(RagdollPhysics))]
    public class NPC : RagdollPhysics , IAddForceToRagdoll
    {
        [Header("References")]
        [SerializeField] Transform player;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody rb;
        Collider mainCollider;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            mainCollider = GetComponent<Collider>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            SetUp(animator, rb, mainCollider);
            RagdollAdjuster(false);
        }

        void Update()
        {
            if (TargetIsCloseEnough() && navMeshAgent.enabled == true)
            {
                navMeshAgent.SetDestination(player.position);
            }
        }

        public void RagdollAdjuster(bool ragdollStatus) // open = true // close = false
        {
            Ragdoll(ragdollStatus);
        }


        bool TargetIsCloseEnough()
        {
            float distance = (transform.position - player.position).magnitude;
            if (distance <= 50)
            {
                return true;
            }
            return false;
        }

        public void AddForceToRagdollObject(float power , float radius)
        {
            foreach (Rigidbody npcRB in subRigidbody)
            {
                npcRB.AddExplosionForce(power , player.position , radius , 3.0f);                
            }
        }
    }
}
