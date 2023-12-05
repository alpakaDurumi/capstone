using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : Projectile
{
    private SlowMotion slowMotion;

    private float throwPower;
    //private float spinPower;

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // 손을 떠나면 다시 물리 활성화
        launched = true;
        CalculatePowers(slowMotion.GetRightHandSpeed());
        rigidbody.AddForce(throwPower * transform.forward, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        // 투척된 상태에서 충돌했다면 충돌 오브젝트가 ProjectileTarget 컴포넌트를 가지는지 검사
        if (launched) {
            launched = false;   // 중복 충돌을 방지하기 위함

            // explode
        }
    }

    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount * 2, 0.0f, 5.0f);
        //spinPower = Mathf.Lerp(0, 1.0f, throwPower / 5.0f);
    }
}
