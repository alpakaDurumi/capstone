using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ProjectileTarget : MonoBehaviour, IProjectileHittable
{
    //public Material otherMaterial = null;

    public float forceAmount = 1.0f;

    public GameObject targetRagdollPrefab;

    public void Hit(RaycastHit hit, Projectile projectile) {
        // hit.point ���� �ǰ� ����Ʈ �߻�
        // ...

        string hitPart = hit.transform.name;    // ���� ����

        // ���׵� ����
        GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // ���� �� ������Ʈ ����
        Destroy(gameObject);
    }

    public void Hit(Transform hitTransform, Projectile projectile) {
        // hit.point ���� �ǰ� ����Ʈ �߻�
        // ...

        string hitPart = hitTransform.name;    // ���� ����

        // ���׵� ����
        GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // ���� �� ������Ʈ ����
        Destroy(gameObject);
    }

    private void GenerateRagdoll(Projectile projectile, string hitPart) {
        GameObject ragdoll = Instantiate(targetRagdollPrefab, transform.position, transform.rotation);  // ���׵� ����

        // �� transform ��ġ�� �����ϰ� ����
        Transform[] ragdollJoints = ragdoll.GetComponentsInChildren<Transform>();
        Transform[] currentJoints = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < ragdollJoints.Length; i++) {
            for (int j = 0; j < currentJoints.Length; j++) {
                if (ragdollJoints[i].name.Equals(currentJoints[j].name)) {
                    ragdollJoints[i].position = currentJoints[j].position;
                    ragdollJoints[i].rotation = currentJoints[j].rotation;
                    break;
                }
            }

            // ���� ������ �ش��ϴ� ���׵��� ������ ȭ���� ����
            if (ragdollJoints[i].name.Equals(hitPart)) {
                projectile.transform.SetParent(ragdollJoints[i]);
            }
        }
    }

    //private void ApplyMaterial() {
    //    if (TryGetComponent(out MeshRenderer meshRenderer))
    //        meshRenderer.material = otherMaterial;
    //}

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
