using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_rifle : Enemy
{
    private bool reloading = false;

    private int attack_cnt = 0;
    private int magazine = 4;

    private BulletShooter shooter;

    protected override void Awake() {
        base.Awake();
        shooter = GetComponentInChildren<BulletShooter>();
    }

    protected override void Update()
    {
        base.Update();

        // �Ѿ� ��� �Ҹ� �� ������
        if(attack_cnt == magazine) {
            Reload();
            StartCoroutine(WaitReloadEnd((b) => { reloading = b; }));
        }
    }

    // ���� ���� ����
    protected override bool CanAttack() {
        if (agent.isStopped && !attacking && attack_timer >= attack_waitingTime && !reloading) {
            return true;
        }
        return false;
    }

    protected override void Attack() {
        base.Attack();
        attack_cnt++;
        shooter.Shoot();
    }

    // ������ �Լ�
    private void Reload() {
        reloading = true;
        animator.SetTrigger("reload");
        attack_cnt = 0;
    }

    private IEnumerator WaitReloadEnd(System.Action<bool> callback) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Reload") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        callback(false);
    }
}
