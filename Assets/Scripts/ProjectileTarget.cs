using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ProjectileTarget : MonoBehaviour, IProjectileHittable
{
    private Enemy enemy;

    public float forceAmount = 1.0f;

    public GameObject targetRagdollPrefab;

    private void Awake() {
        enemy = GetComponent<Enemy>();
    }

    public void Hit(RaycastHit hit, Projectile projectile) {
        // hit.point ���� �ǰ� ����Ʈ �߻�
        // ...

        string hitPart = hit.transform.name;    // ���� ����

        // ���׵� ����
        enemy.GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // �� ��� �Լ� ȣ��
        enemy.Die(false);
        Destroy(gameObject);
    }

    public void Hit(Transform hitTransform, Projectile projectile) {
        // hit.point ���� �ǰ� ����Ʈ �߻�
        // ...

        string hitPart = hitTransform.name;    // ���� ����

        // ���׵� ����
        enemy.GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // �� ��� �Լ� ȣ��
        enemy.Die(false);
        Destroy(gameObject);
    }

    private void ApplyForce(Projectile projectile) {
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(projectile.transform.forward * forceAmount);
    }

    // �ݶ��̴� ��Ȱ��ȭ �Լ�
    private void DisableCollider(Projectile projectile) {
        if (projectile.TryGetComponent(out Collider collider))
            collider.enabled = false;
    }
}
