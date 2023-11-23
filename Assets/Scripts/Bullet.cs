using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // ��ƼŬ �ý��� ������Ʈ ������

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
        // �ݶ��̴� ��Ȱ��ȭ
        bulletCollider.enabled = false;
        // �Ѿ� ����
        bulletRigidbody.isKinematic = true;
    }

    private void HitStage(Collision collision) {
        // �浹 ������ ���� ����
        Vector3 normal = collision.contacts[0].normal;
        // �浹 �������� ���� �������� ��ƼŬ ����
        GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));
        firework.GetComponent<ParticleSystem>().Emit(1);

        DisableBullet();

        Destroy(gameObject);
    }

    // Ÿ��(�� �Ǵ� �÷��̾�)�� ���� ��
    protected void HitTarget() {
        bulletModel.SetActive(false);
        DisableBullet();
        // Ʈ������ ����� ������ ��ٸ� �� ���ӿ�����Ʈ ����
        Destroy(gameObject, trailTime);
    }
}
