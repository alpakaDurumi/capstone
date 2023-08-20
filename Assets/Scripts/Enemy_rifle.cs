using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_rifle : Enemy
{
    private bool attacking = false;

    float attack_timer = 0.0f;
    float attack_waitingTime = 2.0f;

    protected override void Update()
    {
        base.Update();

        attack_timer += Time.deltaTime;

        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !attacking) {
            // attack_waitingTime에 한 번씩 공격하도록 하였음
            if(attack_timer >= attack_waitingTime) {
                attacking = true;
                Attack();
                attack_timer = 0.0f;
            }
            agent.isStopped = true;
        }
        // 사정거리에 들어왔지만 공격은 못한 상태에서 다시 target이 멀어졌을 때 다시 움직이게 하기 위해
        else {
            agent.isStopped = false;
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
