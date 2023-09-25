using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // 파티클 시스템 오브젝트 프리팹

    Rigidbody bulletRigidbody;
    CapsuleCollider bulletCollider;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            // 충돌 지점의 법선 벡터
            Vector3 normal = collision.contacts[0].normal;
            // 충돌 지점에서 법선 방향으로 파티클 생성
            GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));

            firework.GetComponent<ParticleSystem>().Emit(1);

            // 총알 정지 후 게임오브젝트 제거
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            Destroy(gameObject);
        }else if(collision.gameObject.tag == "Enemy") {
            // 총알 정지 후 게임오브젝트 제거
            bulletCollider.enabled = false;
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            Destroy(gameObject, 0.1f);
        }
    }

}
