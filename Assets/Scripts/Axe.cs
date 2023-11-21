using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Axe : Projectile
{
    private IXRSelectInteractor thrownInteractor;   // 도끼를 던진 손에 해당하는 Direct Interactor

    private SlowMotion slowMotion;

    private float throwPower;
    private float spinPower;

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        // 도끼를 select한 XR Origin의 slowMotion 컴포넌트 접근
        slowMotion = args.interactorObject.transform.GetComponentInParent<SlowMotion>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // 손을 떠나면 다시 물리 활성화
        launched = true;
        thrownInteractor = args.interactorObject;   // 던진 interactor를 기억

        CalculatePowers(slowMotion.GetRightHandSpeed());
        rigidbody.AddForce(throwPower * thrownInteractor.transform.forward, ForceMode.Impulse);
    }

    private void FixedUpdate() {
        // 날아가는 동안에 회전
        if (launched) {
            SpinAxe();
        }
    }

    private void OnTriggerEnter(Collider other) {
        // 투척된 상태에서 충돌했다면 충돌 오브젝트가 ProjectileTarget 컴포넌트를 가지는지 검사
        if (launched && other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")) {
            launched = false;   // 중복 충돌을 방지하기 위함
            DisablePhysics();   // 물리 비활성화(꽂혀있는 효과)
            
            ProjectileTarget target = other.transform.GetComponentInParent<ProjectileTarget>();
            // 가지고 있다면
            if(target != null) {
                target.Hit(other.transform, this);
            }
            else {
                transform.SetParent(other.transform);
                // 빗나간 경우 던진 손에 다시 돌아오기
                StartCoroutine(ReturnRoutine());
            }
        }
    }

    // 1.0초 후 던진 interactor로 다시 select 되도록 하는 코루틴
    private IEnumerator ReturnRoutine() {
        yield return new WaitForSeconds(1.0f);
        interactionManager.SelectEnter(thrownInteractor, this);
    }

    // 도끼가 날아가는 힘과 회전하는 힘 계산
    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount, 0.0f, 2.0f);
        spinPower = Mathf.Clamp(amount / 10, 0.0f, 1.0f);
    }

    // 도끼 회전 함수
    private void SpinAxe() {
        transform.Rotate(spinPower, 0, 0);
    }
}
