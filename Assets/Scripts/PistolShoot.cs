using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform barrelLocation;

    private float bulletForce = 5f;

    void Start()
    {
        
    }

    void Update()
    {
       
    }

    public void Shoot() {
        Debug.Log("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletForce * transform.forward, ForceMode.Impulse);
    }
}
