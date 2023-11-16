using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Axe_Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // XR Origin과 무기 충돌 시
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.PlayerDie();
        }
    }
}
