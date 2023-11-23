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

    [SerializeField] private Transform from;                // target���� ������ ����ϱ� ���� ������

    [SerializeField] private LayerMask layerMask_canSee;    // �þ� ���̾��ũ : �⺻������ Player�� Stage ����

    private Transform targetForAim;

    protected override void Awake() {
        base.Awake();
        shooter = GetComponentInChildren<BulletShooter>();
    }

    protected override void Start() {
        base.Start();
        attackLayerIndex = 2;
        targetForAim = target.GetChild(2);
    }

    protected override void Update()
    {
        base.Update();

        // �Ѿ� ��� �Ҹ� �� ������
        if (attack_cnt == magazine) {
            Reload();
            StartCoroutine(WaitReloadEnd((b) => { reloading = b; }));
        }

        // target �������� aim
        AimTarget();
    }

    // target�� ���� �������� ����
    private bool CanSee() {
        Physics.Raycast(from.position, targetForAim.position - from.position, out RaycastHit hitInfo, attackDistance, layerMask_canSee);
        //Debug.DrawRay(from.position, (targetForAim.position - from.position).normalized * attackDistance, Color.red);

        if (hitInfo.transform.tag == "Player") {
            return true;
        }
        else {
            return false;
        }
    }

    // ���� ���� ����
    protected override bool CanAttack() {
        // target�� �� �� �ִٸ� ������ ���� �Ǵ�
        if (CanSee()) {
            if (currentDistance <= attackDistance) {
                // �����Ÿ� ���̶�� �����ϰ� target �������� �� ȸ���ϱ�
                // ...
                //agent.isStopped = true;
                //FaceTarget(target.position);
                if (!attacking && attack_timer >= attack_waitingTime && !reloading) {
                    return true;
                }
                return false;
            }
            return false;
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
    private void AimTarget() {
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
