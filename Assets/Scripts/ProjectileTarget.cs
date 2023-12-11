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
        enemy.GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // 적 사망 함수 호출
        enemy.Die(false);
        Destroy(gameObject);
    }

    public void Hit(Transform hitTransform, Projectile projectile) {
        // hit.point 에서 피격 이펙트 발생
        // ...

        string hitPart = hitTransform.name;    // 맞은 부위

        // 래그돌 생성
        enemy.GenerateRagdoll(projectile, hitPart);

        ApplyForce(projectile);
        DisableCollider(projectile);

        // 적 사망 함수 호출
        enemy.Die(false);
        Destroy(gameObject);
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
