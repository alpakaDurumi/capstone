using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    [SerializeField] private float speed = 2000.0f;

    private new Rigidbody rigidbody;
    private ArrowCaster caster;

    private bool launched = false;

    private RaycastHit hit;

    // 초기화
    protected override void Awake() {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        caster = GetComponent<ArrowCaster>();
    }

    // notch(소켓)에 끼워졌다면
    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        if(args.interactorObject is Notch notch) {
            if (notch.CanRelease) {
                LaunchArrow(notch);
            }
        }
    }

    // 화살 발사
    private void LaunchArrow(Notch notch) {
        launched = true;
        ApplyForce(notch.PullMeasurer);
        StartCoroutine(LaunchRoutine());
    }

    private void ApplyForce(PullMeasurer pullMeasurer) {
        rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
    }

    private IEnumerator LaunchRoutine() {
        // 화살이 무언가와 충돌할 때까지 계속 확인
        while(!caster.CheckForCollision(out hit)) {
            SetDirection();
            yield return null;
        }
        // 충돌한다면 아래 함수들 실행
        DisablePhysics();
        CheckForHittable(hit);
    }

    private void SetDirection() {
        if(rigidbody.velocity.z > 0.5f) {
            transform.forward = rigidbody.velocity;
        }
    }

    // 화살 정지
    private void DisablePhysics() {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    // 화살에 맞은 오브젝트를 화살의 부모로 설정
    // 화살이 꽂혀 있는 효과
    private void ChildArrow(RaycastHit hit) {
        transform.SetParent(hit.transform);
    }

    private void CheckForHittable(RaycastHit hit) {
        ArrowTarget target = hit.transform.GetComponentInParent<ArrowTarget>();

        // ArrowTarget 컴포넌트를 찾은 경우에만
        // 추후에 태그를 통한 식별로 변경할 수 있음
        if(target != null) {
            target.Hit(hit, this);
        }
        // ArrowTarget이 아닌 경우 화살이 박히는 효과만 발생
        else {
            ChildArrow(hit);
        }
    }

    // 화살이 발사되지 않은 상태에만 화살을 select할 수 있음
    public override bool IsSelectableBy(IXRSelectInteractor interactor) {
        return base.IsSelectableBy(interactor) && !launched;
    }
}
