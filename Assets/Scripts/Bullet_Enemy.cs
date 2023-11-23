using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : Bullet
{
    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        // 플레이어에게 적중 시
        if (collision.gameObject.tag == "Player") {
            GameManager.Instance.PlayerDie();
            HitTarget();
        }
    }
}
