using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // ��ƼŬ �ý��� ������Ʈ ������

    Rigidbody bulletRigidbody;
    CapsuleCollider bulletCollider;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            // �浹 ������ ���� ����
            Vector3 normal = collision.contacts[0].normal;
            // �浹 �������� ���� �������� ��ƼŬ ����
            GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));

            firework.GetComponent<ParticleSystem>().Emit(1);

            // �Ѿ� ���� �� ���ӿ�����Ʈ ����
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            Destroy(gameObject);
        }else if(collision.gameObject.tag == "Enemy") {
            // �Ѿ� ���� �� ���ӿ�����Ʈ ����
            bulletCollider.enabled = false;
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            Destroy(gameObject, 0.1f);
        }
    }

}
