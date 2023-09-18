using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_rifle : Enemy
{
    private bool attacking = false;
    private bool reloading = false;

    float attack_timer = 0.0f;
    float attack_waitingTime = 2.0f;

    int attack_cnt = 0;
    int magazine = 4;

    protected override void Update()
    {
        base.Update();

        // ������ �����ְ� ���� ���� �ƴ� ������ Ÿ�̸Ӹ� ����
        if (!attacking && !reloading) {
            attack_timer += Time.deltaTime;
        }

        // �Ѿ� ��� �Ҹ� �� ������
        if(attack_cnt == magazine) {
            Reload();
        }

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
        attack_cnt++;
        Debug.Log(attack_cnt);
    }

    // ���� �ִϸ��̼��� ������ �� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void AttackEnd() {
        attacking = false;
        agent.isStopped = false;
    }

    // ������ �Լ�
    private void Reload() {
        animator.SetTrigger("reload");
        attack_cnt = 0;
    }

    // ������ �ִϸ��̼��� ������ �� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void ReloadEnd() {
        reloading = false;
    }

}
