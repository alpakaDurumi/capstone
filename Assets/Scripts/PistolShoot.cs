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
        // 머즐 플래시 효과
        GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(flash, destroyTime);

        // 총알 발사
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletForce * transform.forward, ForceMode.VelocityChange);

        // 사물에 부딪히거나 적에게 적중한 경우 총알을 제거해야 함.
    }
}
