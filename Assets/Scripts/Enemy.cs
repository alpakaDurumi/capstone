using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    [SerializeField] public Transform target;       // target은 XR Origin으로 설정할 것
    protected NavMeshAgent agent;
    protected Animator animator;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    [SerializeField] protected float attackDistance;    // 공격 사정거리
    protected float currentDistance;                    // 현재 target과의 거리

    protected bool attacking = false;

    protected float attack_timer = 0.0f;            // 공격 타이머
    protected float attack_waitingTime = 2.0f;      // 공격 간격

    WeaponChanger weaponChanger;

    protected int attackLayerIndex;                 // 공격 애니메이션이 위치한 애니메이션 레이어

    protected virtual void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        weaponChanger = target.GetComponent<WeaponChanger>();
    }

    protected virtual void Start()
    {
        // agent와 transform의 position 동기화를 수동으로 처리하도록 함
        // 아래의 OnAnimatorMove에서 처리하였음
        agent.updatePosition = false;
    }

    protected virtual void Update()
    {
        agent.SetDestination(target.position);
        // 이동경로에서 다음 위치로의 방향 계산
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // 가변적인 프레임에 맞춰 DeltaPosition을 smoothing
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // 시간 변화에 따라 velocity 업데이트
        if(Time.deltaTime > 1e-5f) {
            velocity = smoothDeltaPosition / Time.deltaTime;
        }

        // 애니메이션 여부를 결정
        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // 애니메이터 파라미터 설정
        animator.SetBool("move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);

        // 공격 중이 아닌 경우 타이머를 증가
        if (!attacking) {
            attack_timer += Time.deltaTime;
        }

        currentDistance = Vector3.Distance(transform.position, target.position);

        if (CanAttack()) {
            Attack();
            attack_timer = 0.0f;
            StartCoroutine(WaitAttackEnd((b) => { attacking = b; }));
        }
    }

    protected void OnAnimatorMove() {
        // position을 agent의 position으로 업데이트
        transform.position = agent.nextPosition;

        //Vector3 position = animator.rootPosition;
        //position.y = agent.nextPosition.y;
        //transform.position = position;
    }

    // target 바라보기 기능
    // 보간 정도를 점검할 것
    //private void FaceTarget(Vector3 dest) {
    //    Vector3 lookpos = dest - transform.position;
    //    lookpos.y = 0f;
    //    Quaternion rotation = Quaternion.LookRotation(lookpos);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    //}

    // 공격 가능 여부
    protected virtual bool CanAttack() {
        if (currentDistance <= attackDistance) {
            // 사정거리 안이라면 정지하고 target 방향으로 몸 회전하기
            // ...
            //agent.isStopped = true;
            //FaceTarget(target.position);
            if (!attacking && attack_timer >= attack_waitingTime) {
                return true;
            }
            return false;
        }
        return false;
    }

    // 공격 함수
    protected virtual void Attack() {
        attacking = true;
        animator.SetTrigger("attack");
    }

    // 공격이 끝나면 attacking을 false로 update하는 코루틴
    protected virtual IEnumerator WaitAttackEnd(System.Action<bool> callback) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(attackLayerIndex).IsTag("Attack") && animator.GetCurrentAnimatorStateInfo(attackLayerIndex).normalizedTime >= 1.0f);
        callback(false);
    }

    // Enemy 사망 시 GamaManager의 적 수 감소
    public void Die() {
        GameManager.Instance.DecreaseEnemyCountOnStage();
        weaponChanger.IncreaseKillCount();
    }
}
