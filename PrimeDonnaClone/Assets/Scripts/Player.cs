using UnityEngine;
using PrimeDonna.NPCSpace;
using UnityEngine.AI;

namespace PrimeDonna.Player
{
    [RequireComponent(typeof(RagdollPhysics))]
    public class Player : RagdollPhysics
    {
        [Header("Movement Variables")]
        [SerializeField] float speed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float lerpMult;

        [Header("Adding Force To NPC Variables")]
        [SerializeField] float radius;
        [SerializeField] float power;

        [Header("References")]
        [SerializeField] FloatingJoystick joystick;


        Rigidbody rb;
        Animator animator;
        Collider mainCollider;

        TriggerProcessor triggerProcessor;

        LayerMask layerMask;

        bool once;

        void Start()
        {
            triggerProcessor = GetComponent<TriggerProcessor>();

            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            mainCollider = GetComponent<Collider>();

            layerMask = LayerMask.GetMask("Enemy");

            SetUp(animator, rb, mainCollider);
            RagdollAdjust(false);
        }

        void Update()
        {
            rb.velocity = transform.forward * Time.deltaTime * speed;

            if (Input.GetMouseButton(0))
            {
                if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                    if (!once) once = true;

                    Vector3 newDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

                    //transform.forward = Vector3.Lerp(transform.forward, newDirection , Time.deltaTime * lerpMult); // another way to move with joystick

                    Rotate(newDirection);
                }
            }
            else
            {
                if (once)
                {
                    once = false;
                    AddForce();
                    triggerProcessor.ResetAngle();
                }
            }
        }

        void Rotate(Vector3 direction)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, .1f);
            Quaternion _newRotation = Quaternion.LookRotation(desiredForward);
            transform.rotation = _newRotation;
        }

        public void RagdollAdjust(bool ragdollStatus) // open = true // close = false
        {
            Ragdoll(ragdollStatus);
        }

        void AddForce()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, layerMask);
            foreach (Collider hit in colliders)
            {
                Rigidbody npcRB = hit.GetComponent<Rigidbody>();

                if (npcRB != null && TargetCanSeeMe(npcRB))
                {
                    NPC npc = npcRB.GetComponent<NPC>();
                    if (npc == null) return;

                    npcRB.GetComponent<NavMeshAgent>().enabled = false;
                                   
                    npc.RagdollAdjuster(true);
                    float desiredPower = triggerProcessor.circle.fillAmount * power; 
                    npc.AddForceToRagdollObject(desiredPower, radius);

                }
            }
        }
        bool TargetCanSeeMe(Rigidbody npcRB) // check target is in angle 
        {
            Vector3 neededNPCpos = new Vector3(npcRB.transform.position.x, transform.position.y, npcRB.transform.position.z);
            Vector3 toAgent = neededNPCpos - transform.position;
            float lookingAngle = Vector3.Angle(transform.forward, toAgent);
            float desiredAngle = (triggerProcessor.circle.fillAmount * 360) * .5f;

            Debug.Log(lookingAngle + "::::::::::::::::" + desiredAngle);

            if (Mathf.Abs(lookingAngle) < desiredAngle)
            {
                return true;
            }
            return false;
        }
    }
}
