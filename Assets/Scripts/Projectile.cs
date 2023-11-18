using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Projectile : XRGrabInteractable
{
    protected new Rigidbody rigidbody;
    
    protected bool launched = false;

    // �ʱ�ȭ
    protected override void Awake() {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
    }

    // ���� Ȱ��ȭ
    protected void EnablePhysics() {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }

    // ���� ��Ȱ��ȭ
    protected void DisablePhysics() {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }
}
