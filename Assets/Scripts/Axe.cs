using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Axe : Projectile
{
    private IXRSelectInteractor thrownInteractor;   // 도끼를 던진 손에 해당하는 Direct Interactor

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // 손을 떠나면 다시 물리 활성화
        launched = true;
        thrownInteractor = args.interactorObject;   // 던진 interactor를 기억
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
}
