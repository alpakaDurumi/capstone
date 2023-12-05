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

    private Transform aimStart;                             // target과의 각도를 계산하기 위한 기준점

    [SerializeField] private LayerMask layerMask_canSee;    // 시야 레이어마스크 : 기본적으로 Player와 Stage 선택

    private float angle_horizontal;
    private float angle_vertical;

    private bool turning = false;

    public GameObject grenadePrefab;
    public GameObject rightHand;

    public bool haveGrenade = false;
    private int grenadeLayerIndex = 5;

    private float animationPercentToRelease = 0.57f;

    protected override void Awake() {
        base.Awake();
        shooter = GetComponentInChildren<BulletShooter>();
    }

    protected override void Start() {
        base.Start();
        attackLayerIndex = 3;
        aimStart = transform.GetChild(1);
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
        bool isHit = Physics.Raycast(aimStart.position, target.position - aimStart.position, out RaycastHit hitInfo, attackDistance, layerMask_canSee);
        //Debug.DrawRay(aimStart.position, (target.position - aimStart.position).normalized * attackDistance, Color.red);

        // 광선이 콜라이더를 만난 경우
        if (isHit) {
            // 플레이어를 볼 수 있다면
            if (hitInfo.transform.tag == "Player") {
                return true;
            }
            else {
                return false;
            }
        }
        // 광선이 아무 콜라이더도 만나지 못한 경우
        else {
            return false;
        }
    }

    // 공격 가능 여부
    protected override bool CanAttack() {
        // target을 볼 수 있다면 나머지 조건 판단
        if (CanSee()) {
            if (currentDistance <= attackDistance) {
                // 공격 가능한 범위 내에 target이 있으며, 에이밍 가능 범위를 벗어난 경우에만 회전
                if (turning) {
                    TurnBody();
                }

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
        if (haveGrenade) {
            SwingGrenade();
            haveGrenade = false;
        }
        else {
            base.Attack();
            attack_cnt++;
            shooter.Shoot();
        }
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
        // target 방향으로의 가로 각도와 세로 각도를 계산
        Vector3 dir = aimStart.InverseTransformPoint(target.position);
        angle_horizontal = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        angle_vertical = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;

        float[] horizontal_limit = new float[2];
        float[] vertical_limit = new float[2];

        // 하반신까지 움직여야 최대로 가동이 가능하지만, 런타임 중 아바타 마스크의 변경을 엔진에서 허용하지 않음
        // 마스크가 적용된 레이어와 적용되지 않은 레이어를 각각 두고, 정지 여부에 따라 weight를 조절해 가며 사용

        // 정지한 상태라면
        if (!animator.GetBool("move")) {
            animator.SetLayerWeight(1, 1);      // Upper Layer Stopped의 Weight를 1로 설정
            animator.SetLayerWeight(2, 0);      // Upper Layer Moving의 Weight를 0으로 설정

            // 하반신까지 사용했을 때 최대 각도
            horizontal_limit[0] = -50;
            horizontal_limit[1] = 66;
            vertical_limit[0] = -54;
            vertical_limit[1] = 43;
        }
        // 움직이는 상태라면
        else {
            animator.SetLayerWeight(1, 0);      // Upper Layer Stopped의 Weight를 0로 설정
            animator.SetLayerWeight(2, 1);      // Upper Layer Moving의 Weight를 1으로 설정

            // 상반신만 사용했을 때 최대 각도
            horizontal_limit[0] = -52;
            horizontal_limit[1] = 53;
            vertical_limit[0] = -33;
            vertical_limit[1] = 43;
        }

        // 몸을 회전하지 않고 조준이 가능한 각도라면
        if (horizontal_limit[0] <= angle_horizontal & angle_horizontal <= horizontal_limit[1]) {
            // 왼쪽
            if (angle_horizontal < 0) {
                var t = Scale(horizontal_limit[0], 0, -1, 0, angle_horizontal);
                animator.SetFloat("body_horizontal", t);
            }
            // 오른쪽
            else {
                var t = Scale(0, horizontal_limit[1], 0, 1, angle_horizontal);
                animator.SetFloat("body_horizontal", t);
            }
        }
        // 회전이 필요한 경우
        else {
            turning = true;          
        }


        // 조준이 가능한 각도라면
        if (vertical_limit[0] <= angle_vertical && angle_vertical <= vertical_limit[1]) {
            // 아래
            if (angle_vertical < 0) {
                var t = Scale(vertical_limit[0], 0, -1, 0, angle_vertical);
                animator.SetFloat("body_vertical", t);
            }
            // 위
            else {
                var t = Scale(0, vertical_limit[1], 0, 1, angle_vertical);
                animator.SetFloat("body_vertical", t);
            }
        }
        else {
            // 뒤로 물러나서 시야 확보하기?
        }
    }

    // 값의 range를 변경하는 함수
    private float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue) {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    // 몸 회전 함수
    private void TurnBody() {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(target.position.x - aimStart.position.x, 0, target.position.z - aimStart.position.z));
        transform.rotation = Quaternion.RotateTowards(aimStart.rotation, targetRotation, 120f * Time.deltaTime);

        // 왼쪽 회전
        if (angle_horizontal < 0) {
            animator.SetInteger("turn", 1);
        }
        // 오른쪽 회전
        else {
            animator.SetInteger("turn", 2);
        }

        // 목표 방향에 도달했다면
        if (-1.0f < angle_horizontal && angle_horizontal < 1.0f) {
            animator.SetInteger("turn", 0);
            turning = false;
        }
    }

    // 수류탄 던지기 함수
    private void SwingGrenade() {
        // 애니메이션 재생
        animator.SetTrigger("grenade");
        // 수류탄 오브젝트 생성 후 손 위치에 부착
        GameObject grenade = Instantiate(grenadePrefab, rightHand.transform.position, rightHand.transform.rotation);
        grenade.transform.SetParent(rightHand.transform);
        
        StartCoroutine(WaitGrenadeRelease(grenade));
    }

    // 애니메이션 중 손에서 수류탄이 나가는 순간을 구하기 위한 코루틴
    private IEnumerator WaitGrenadeRelease(GameObject grenade) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(grenadeLayerIndex).IsName("Grenade") && animator.GetCurrentAnimatorStateInfo(grenadeLayerIndex).normalizedTime >= animationPercentToRelease);
        ReleaseGrenade(grenade);
    }

    private void ReleaseGrenade(GameObject grenade) {        
        // 손에서 분리
        rightHand.transform.DetachChildren();
        grenade.transform.position = rightHand.transform.position;
        // 목표 지정 & release
        grenade.GetComponent<Grenade>().targetPosition = target.transform.position;
        grenade.GetComponent<Grenade>().Release();
    }
}
