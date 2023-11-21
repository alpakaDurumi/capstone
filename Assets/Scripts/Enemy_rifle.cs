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

    [SerializeField] private Transform from;                // target과의 각도를 계산하기 위한 기준점

    [SerializeField] private LayerMask layerMask_canSee;    // 시야 레이어마스크 : 기본적으로 Player와 Stage 선택

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

        // 총알 모두 소모 시 재장전
        if (attack_cnt == magazine) {
            Reload();
            StartCoroutine(WaitReloadEnd((b) => { reloading = b; }));
        }

        // target 방향으로 aim
        AimTarget();
    }

    // target을 포착 가능한지 여부
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

    // 공격 가능 여부
    protected override bool CanAttack() {
        // target을 볼 수 있다면 나머지 조건 판단
        if (CanSee()) {
            if (currentDistance <= attackDistance) {
                // 사정거리 안이라면 정지하고 target 방향으로 몸 회전하기
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

    // 재장전 함수
    private void Reload() {
        reloading = true;
        animator.SetTrigger("reload");
        attack_cnt = 0;
    }

    private IEnumerator WaitReloadEnd(System.Action<bool> callback) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(attackLayerIndex).IsName("Reload") && animator.GetCurrentAnimatorStateInfo(attackLayerIndex).normalizedTime >= 1.0f);
        callback(false);
    }

    // target 방향으로 조준하기 위한 함수
    private void AimTarget() {
        Vector3 dir = from.InverseTransformPoint(target.position).normalized;

        // dir : -1 ~ 1 구간. -90도부터 90도까지 나타냄.
        // 애니메이션으로 표현 가능한 각도
        //      좌측은 45도
        //      우측은 70도

        if(dir.x < 0) {
            animator.SetFloat("body_horizontal", Mathf.Clamp(dir.x * 90 / 45, -1, 1));
        }
        else {
            animator.SetFloat("body_horizontal", Mathf.Clamp(dir.x * 90 / 70, -1, 1));
        }
        animator.SetFloat("body_vertical", Mathf.Clamp(dir.y * 2, -1, 1));
    }
}
