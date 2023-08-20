using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    [SerializeField] public Transform target;       // target은 XR Origin으로 설정할 것
    protected NavMeshAgent agent;
    protected Animator animator;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    // 참조는 awake()로 옮길 것.
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
}
