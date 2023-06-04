using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_rifle : Enemy
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
        animator.SetTrigger("attack");
    }

    // 공격 애니메이션이 끝났을 때 호출되는 이벤트 함수
    private void AttackEnd() {
        attacking = false;
        agent.isStopped = false;
    }

}
