using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Axe_Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // �÷��̾�� ���� �浹 ��
        if(other.gameObject.tag == "Player") {
            GameManager.Instance.PlayerDie();
        }
    }
}
