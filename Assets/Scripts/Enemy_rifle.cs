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

    // ���� �Լ�
    private void Attack() {
        animator.SetTrigger("attack");
    }

    // ���� �ִϸ��̼��� ������ �� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void AttackEnd() {
        attacking = false;
        agent.isStopped = false;
    }

}
