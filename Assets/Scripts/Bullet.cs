using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject MetalHit; // ��ƼŬ �ý��� ������Ʈ ������

    private Rigidbody bulletRigidbody;
    private CapsuleCollider bulletCollider;

    private GameObject bulletModel;
    private BoxCollider slicerCollider = null;  // �÷��̾ �߻��� ��쿡�� �Ҵ�

    private float trailTime;

    // �Ѿ��� �÷��̾ �߻��� ������ �Ǵ� ���� �߻��� ������ ����
    public bool shotByPlayer;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<CapsuleCollider>();

        bulletModel = transform.GetChild(0).gameObject;

        if (shotByPlayer) {
            slicerCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        }

        trailTime = GetComponentInChildren<TrailRenderer>().time;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Stage") {
            HitStage(collision);
        }
        else {
            // �÷��̾ �߻��� �Ѿ��̶��
            if (shotByPlayer) {
                if(collision.gameObject.tag == "Enemy") {
                    HitTarget();
                }
            }
            // ���� �߻��� �Ѿ��̶��
            else {
                if (collision.gameObject.tag == "Player") {
                    GameManager.Instance.PlayerDie();
                    HitTarget();
                }
            }
        }
    }

    private void DisableBullet() {
        // �ݶ��̴� ��Ȱ��ȭ
        bulletCollider.enabled = false;
        if (shotByPlayer) {
            slicerCollider.enabled = false;
        }

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
    private void HitTarget() {
        bulletModel.SetActive(false);
        DisableBullet();
        // Ʈ������ ����� ������ ��ٸ� �� ���ӿ�����Ʈ ����
        Destroy(gameObject, trailTime);
    }
}
