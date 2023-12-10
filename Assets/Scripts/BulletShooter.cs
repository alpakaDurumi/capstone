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
        SoundManager.Instance.PlayGunFireSound();
        // 머즐 플래시 효과
        GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(flash, destroyTime);

        // 총알 발사
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletForce * transform.forward, ForceMode.VelocityChange);
    }
}
