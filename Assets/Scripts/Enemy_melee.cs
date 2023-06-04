using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_melee : Enemy
{
    private bool attacking = false;

    protected override void Update()
    {
        base.Update();

        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !attacking) {
            attacking = true;
            agent.isStopped = true;
            Attack();
        }

    }

    // 공격 함수
    private void Attack() {
        animator.SetInteger("attack index", Random.Range(0, 4));
        animator.SetTrigger("attack");
    }

    // 공격 애니메이션이 끝났을 때 호출되는 이벤트 함수
    private void AttackEnd() {
        attacking = false;
        agent.isStopped = false;
    }

}
