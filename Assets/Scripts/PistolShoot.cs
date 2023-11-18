using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;

    private float destroyTime = 0.2f;
    private float bulletForce = 20f;

    void Start()
    {
        
    }

    void Update()
    {
       
    }

    public void Shoot() {
        // ���� �÷��� ȿ��
        GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(flash, destroyTime);

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletForce * transform.forward, ForceMode.VelocityChange);

        // �繰�� �ε����ų� ������ ������ ��� �Ѿ��� �����ؾ� ��.
    }
}
