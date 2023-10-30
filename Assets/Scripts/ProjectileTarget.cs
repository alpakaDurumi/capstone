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
        // hit.point 에서 피격 이펙트 발생
        // ...

        string hitPart = hit.transform.name;    // 맞은 부위

        // 래그돌 생성
        GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // 적 사망 함수 호출
        enemy.Die();
    }

    public void Hit(Transform hitTransform, Projectile projectile) {
        // hit.point 에서 피격 이펙트 발생
        // ...

        string hitPart = hitTransform.name;    // 맞은 부위

        // 래그돌 생성
        GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // 적 사망 함수 호출
        enemy.Die();
    }

    private void GenerateRagdoll(Projectile projectile, string hitPart) {
        GameObject ragdoll = Instantiate(targetRagdollPrefab, transform.position, transform.rotation);  // 래그돌 생성

        // 각 transform 위치를 동일하게 수정
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

            // 맞은 부위에 해당하는 래그돌의 부위에 화살을 고정
            if (ragdollJoints[i].name.Equals(hitPart)) {
                projectile.transform.SetParent(ragdollJoints[i]);
            }
        }
    }

    private void ApplyForce(Projectile projectile) {
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(projectile.transform.forward * forceAmount);
    }

    // 콜라이더 비활성화 함수
    private void DisableCollider(Projectile projectile) {
        if (projectile.TryGetComponent(out Collider collider))
            collider.enabled = false;
    }
}
