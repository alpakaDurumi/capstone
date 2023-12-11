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

    private float noiseAmount = 0.05f;

    public void Shoot() {
        SoundManager.Instance.PlayGunFireSound();
        // 머즐 플래시 효과
        GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(flash, destroyTime);

        // 총알 발사
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        Vector3 direction = transform.forward;
        // 적이 발사한 총알이라면 발사 방향에 약간의 오차 적용
        if (bullet.TryGetComponent<Bullet_Enemy>(out _)) {
            direction = new Vector3(direction.x + Random.Range(-noiseAmount, noiseAmount), direction.y + Random.Range(-noiseAmount, noiseAmount), direction.z + Random.Range(-noiseAmount, noiseAmount));
        }

        bulletRigidbody.AddForce(bulletForce * direction, ForceMode.VelocityChange);
    }
}
