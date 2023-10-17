using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Projectile : XRGrabInteractable
{
    protected new Rigidbody rigidbody;
    
    protected bool launched = false;

    // 초기화
    protected override void Awake() {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
    }

    // 물리 활성화
    protected void EnablePhysics() {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }

    // 물리 비활성화
    protected void DisablePhysics() {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }
}
