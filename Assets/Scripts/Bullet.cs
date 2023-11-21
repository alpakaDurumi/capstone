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
    BoxCollider[] slicerCollider;   // ���� ���� 2���� slicer

    float trailTime;

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();

        bulletModel = transform.GetChild(0).gameObject;

        slicerCollider = new BoxCollider[2];
        slicerCollider[0] = transform.GetChild(1).GetComponent<BoxCollider>();
        slicerCollider[1] = transform.GetChild(2).GetComponent<BoxCollider>();

        trailTime = GetComponentInChildren<TrailRenderer>().time;
    }

    private void Start()
    {
        // Bullet�� ��ü �ݶ��̴��� �浹���� �ʰ� slice�� �Ǵ� ��Ȳ�� ���ϱ� ����
        slicerCollider[0].enabled = false;
        slicerCollider[1].enabled = false;
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
            // �� ��Ȱ��ȭ
            bulletModel.SetActive(false);

            // ��ü �ݶ��̴��� �浹�ϸ� slicer collider���� Ȱ��ȭ
            slicerCollider[0].enabled = true;
            slicerCollider[1].enabled = true;

            DisableBullet();

            // Ʈ������ ����� ������ ��ٸ� �� ���ӿ�����Ʈ ����
            Destroy(gameObject, trailTime);
        }
    }

    private void DisableBullet() {
        // �ݶ��̴� ��Ȱ��ȭ
        bulletCollider.enabled = false;

        // �Ѿ� ����
        bulletRigidbody.isKinematic = true;
    }
}
