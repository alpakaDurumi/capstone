using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // 플레이어에게 적중 시 플레이어 사망
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.PlayerDie();
        }
        // 적에게 적중 시 적 사망
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy_Detecting_Explosion")) {
            Debug.Log("Enemy Die");

            Enemy enemy = other.GetComponent<Enemy>();

            // 래그돌 생성
            enemy.GenerateRagdoll();

            //ApplyForce(projectile);

            // 적 사망 함수 호출
            enemy.Die(true);
            Destroy(enemy.gameObject);
        }
    }
}
