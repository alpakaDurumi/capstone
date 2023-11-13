using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // ��ƼŬ �ý��� ������Ʈ ������

    Rigidbody bulletRigidbody;
    CapsuleCollider bulletCollider;

    GameObject bulletModel;
    GameObject slicer;
    BoxCollider slicerCollider;

    float trailTime;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();

        bulletModel = transform.GetChild(0).gameObject;
        slicer = transform.GetChild(1).gameObject;
        slicerCollider = slicer.gameObject.GetComponent<BoxCollider>();

        trailTime = GetComponentInChildren<TrailRenderer>().time;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            // �浹 ������ ���� ����
            Vector3 normal = collision.contacts[0].normal;
            // �浹 �������� ���� �������� ��ƼŬ ����
            GameObject firework = Instantiate(MetalHit, transform.position, Quaternion.LookRotation(normal));

            firework.GetComponent<ParticleSystem>().Emit(1);

            DisableBullet();

            Destroy(gameObject);
        }else if(collision.gameObject.tag == "Enemy") {
            // slicer Ȱ��ȭ �� �� ��Ȱ��ȭ
            slicer.SetActive(true);
            bulletModel.SetActive(false);

            DisableBullet();

            // Ʈ������ ����� ������ ��ٸ� �� ���ӿ�����Ʈ ����
            Destroy(gameObject, trailTime);
        }
    }

    private void DisableBullet() {
        // �ݶ��̴� ��Ȱ��ȭ
        bulletCollider.enabled = false;
        slicerCollider.enabled = false;

        // �Ѿ� ����
        bulletRigidbody.velocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;
    }
}
