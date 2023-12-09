using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // �÷��̾�� ���� �� �÷��̾� ���
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.PlayerDie();
        }
        // ������ ���� �� �� ���
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy_Detecting_Explosion")) {
            Debug.Log("Enemy Die");

            Enemy enemy = other.GetComponent<Enemy>();

            // ���׵� ����
            enemy.GenerateRagdoll();

            //ApplyForce(projectile);

            // �� ��� �Լ� ȣ��
            enemy.Die(true);
            Destroy(enemy.gameObject);
        }
    }
}
