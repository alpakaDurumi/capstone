using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // 파티클 시스템 오브젝트 프리팹

    Rigidbody bulletRigidbody;
    CapsuleCollider bulletCollider;

    GameObject bulletModel;
    BoxCollider slicerCollider;

    float trailTime;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();

        bulletModel = transform.GetChild(0).gameObject;
        slicerCollider = transform.GetChild(1).GetComponent<BoxCollider>();

        trailTime = GetComponentInChildren<TrailRenderer>().time;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            // 충돌 지점의 법선 벡터
            Vector3 normal = collision.contacts[0].normal;
            // 충돌 지점에서 법선 방향으로 파티클 생성
            GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));

            firework.GetComponent<ParticleSystem>().Emit(1);

            DisableBullet();

            Destroy(gameObject);
        }else if(collision.gameObject.tag == "Enemy") {
            // 모델 비활성화
            bulletModel.SetActive(false);

            DisableBullet();

            // 트레일이 사라질 때까지 기다린 후 게임오브젝트 제거
            Destroy(gameObject, trailTime);
        }
    }

    private void DisableBullet() {
        // 콜라이더 비활성화
        bulletCollider.enabled = false;
        slicerCollider.enabled = false;

        // 총알 정지
        bulletRigidbody.isKinematic = true;
    }
}
