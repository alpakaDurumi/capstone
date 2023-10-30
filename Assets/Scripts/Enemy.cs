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

    protected bool attacking = false;

    protected float attack_timer = 0.0f;            // 공격 타이머
    protected float attack_waitingTime = 2.0f;      // 공격 간격

    public GameManager manager;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
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

        //FaceTarget(target.position);

        // 공격 중이 아닌 경우 타이머를 증가
        if (!attacking) {
            attack_timer += Time.deltaTime;
        }

        if (!agent.isStopped) {
            StopMoveAgent();
        }

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
    private void FaceTarget(Vector3 dest) {
        Vector3 lookpos = dest - transform.position;
        lookpos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookpos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    }

    // target 도달 여부에 따라 agent를 중지시키는 함수
    private void StopMoveAgent() {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            agent.isStopped = true;
        }
    }

    // 공격 가능 여부
    protected virtual bool CanAttack() {
        if (agent.isStopped && !attacking && attack_timer >= attack_waitingTime) {
            return true;
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
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        callback(false);
    }

    public void Die() {
        //manager.DecreaseRemainEnemy();
    }
}
