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

    [SerializeField] private Transform from;    // target���� ������ ����ϱ� ���� ������

    protected override void Awake() {
        base.Awake();
        shooter = GetComponentInChildren<BulletShooter>();
    }

    protected override void Start() {
        base.Start();
        attackLayerIndex = 2;
    }

    protected override void Update()
    {
        base.Update();

        // �Ѿ� ��� �Ҹ� �� ������
        if(attack_cnt == magazine) {
            Reload();
            StartCoroutine(WaitReloadEnd((b) => { reloading = b; }));
        }

        // target �ٶ󺸱�
        FaceTarget();
    }

    // ���� ���� ����
    protected override bool CanAttack() {
        if (base.CanAttack() && !reloading) {
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
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(attackLayerIndex).IsName("Reload") && animator.GetCurrentAnimatorStateInfo(attackLayerIndex).normalizedTime >= 1.0f);
        callback(false);
    }

    // target �������� �����ϱ� ���� �Լ�
    private void FaceTarget() {
        Vector3 dir = from.InverseTransformPoint(target.position).normalized;

        // dir : -1 ~ 1 ����. -90������ 90������ ��Ÿ��.
        // �ִϸ��̼����� ǥ�� ������ ����
        //      ������ 45��
        //      ������ 70��

        if(dir.x < 0) {
            animator.SetFloat("body_horizontal", Mathf.Clamp(dir.x * 90 / 45, -1, 1));
        }
        else {
            animator.SetFloat("body_horizontal", Mathf.Clamp(dir.x * 90 / 70, -1, 1));
        }
        animator.SetFloat("body_vertical", Mathf.Clamp(dir.y * 2, -1, 1));
    }
}
