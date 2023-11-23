using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player : Bullet 
{
    private BoxCollider[] slicerCollider = null;        // ���� ���� 2���� slicer

    protected override void Awake() {
        base.Awake();
        slicerCollider = new BoxCollider[2];
        slicerCollider[0] = transform.GetChild(1).GetComponent<BoxCollider>();
        slicerCollider[1] = transform.GetChild(2).GetComponent<BoxCollider>();
    }

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        // ������ ���� ��
        if (collision.gameObject.tag == "Enemy") {
            HitTarget();
        }
    }

    protected override void DisableBullet() {
        base.DisableBullet();
        // �߰��� slicer���� ��Ȱ��ȭ
        slicerCollider[0].enabled = false;
        slicerCollider[1].enabled = false;
    }
}
