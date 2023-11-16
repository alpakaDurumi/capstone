using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;

    private float destroyTime = 0.2f;
    private float bulletForce = 20f;

    public void Shoot() {
        // ���� �÷��� ȿ��
        GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(flash, destroyTime);

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        // ���ѿ��� �߻��� ���, �÷��̾ �߻��� ��
        if(gameObject.name == "Pistol") {
            bullet.GetComponent<Bullet>().shotByPlayer = true;
        }
        else {
            bullet.GetComponent<Bullet>().shotByPlayer = false;
        }
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletForce * transform.forward, ForceMode.VelocityChange);

        // �繰�� �ε����ų� ������ ������ ��� �Ѿ��� �����ؾ� ��.
    }
}
