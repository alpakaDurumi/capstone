using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Axe_Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // 플레이어와 무기 충돌 시
        if(other.gameObject.tag == "Player") {
            GameManager.Instance.PlayerDie();
        }
    }
}
