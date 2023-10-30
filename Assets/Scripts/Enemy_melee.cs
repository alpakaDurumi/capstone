using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_melee : Enemy
{
    protected override void Attack() {
        base.Attack();
        animator.SetInteger("attack index", Random.Range(0, 4));    // 4개의 공격 모션 중 랜덤 하나
    }
}
