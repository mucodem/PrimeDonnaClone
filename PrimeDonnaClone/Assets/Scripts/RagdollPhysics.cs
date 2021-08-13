using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPhysics : MonoBehaviour
{
    Collider[] subCollider;
    protected Rigidbody[] subRigidbody { get; private set; }

    Animator animatorOfTransform;
    Rigidbody rigidbodyOfTransform;
    Collider mainColliderOfTransform;

    protected void SetUp(Animator anim, Rigidbody rbTransform, Collider colliderMain)
    {
        animatorOfTransform = anim;
        rigidbodyOfTransform = rbTransform;
        mainColliderOfTransform = colliderMain;

        subCollider = GetComponentsInChildren<Collider>(true);
        subRigidbody = GetComponentsInChildren<Rigidbody>(true);
    }

    protected void Ragdoll(bool ragdollState)
    {
        foreach (Collider collider in subCollider)
        {
            collider.enabled = ragdollState;
        }

        foreach (Rigidbody npcRB in subRigidbody)
        {
            npcRB.isKinematic = !ragdollState;
        }

        rigidbodyOfTransform.isKinematic = ragdollState; // main rigidbody on player

        mainColliderOfTransform.enabled = !ragdollState; // main collider on player
        animatorOfTransform.enabled = !ragdollState; //
    }

}
