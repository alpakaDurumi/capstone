using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // 파티클 시스템 오브젝트 프리팹

    private Rigidbody bulletRigidbody;
    private CapsuleCollider bulletCollider;

    private GameObject bulletModel;

    private float trailTime;

    protected virtual void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();

        bulletModel = transform.GetChild(0).gameObject;

        trailTime = GetComponentInChildren<TrailRenderer>().time;
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            HitStage(collision);
        }
    }

    protected virtual void DisableBullet() {
        // 콜라이더 비활성화
        bulletCollider.enabled = false;
        // 총알 정지
        bulletRigidbody.isKinematic = true;
    }

    private void HitStage(Collision collision) {
        // 충돌 지점의 법선 벡터
        Vector3 normal = collision.contacts[0].normal;
        // 충돌 지점에서 법선 방향으로 파티클 생성
        GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));
        firework.GetComponent<ParticleSystem>().Emit(1);

        DisableBullet();

        Destroy(gameObject);
    }

    // 타겟(적 또는 플레이어)에 적중 시
    protected void HitTarget() {
        bulletModel.SetActive(false);
        DisableBullet();
        // 트레일이 사라질 때까지 기다린 후 게임오브젝트 제거
        Destroy(gameObject, trailTime);
    }
}
