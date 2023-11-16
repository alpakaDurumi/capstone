using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Axe_Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // XR Origin�� ���� �浹 ��
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.PlayerDie();
        }
    }
}
