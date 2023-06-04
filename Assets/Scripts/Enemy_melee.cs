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

    // ���� �Լ�
    private void Attack() {
        animator.SetInteger("attack index", Random.Range(0, 4));
        animator.SetTrigger("attack");
    }

    // ���� �ִϸ��̼��� ������ �� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void AttackEnd() {
        attacking = false;
        agent.isStopped = false;
    }

}
