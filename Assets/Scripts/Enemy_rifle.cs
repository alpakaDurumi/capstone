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
            // attack_waitingTime�� �� ���� �����ϵ��� �Ͽ���
            if(attack_timer >= attack_waitingTime) {
                attacking = true;
                Attack();
                attack_timer = 0.0f;
            }
            agent.isStopped = true;
        }
        // �����Ÿ��� �������� ������ ���� ���¿��� �ٽ� target�� �־����� �� �ٽ� �����̰� �ϱ� ����
        else {
            agent.isStopped = false;
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
