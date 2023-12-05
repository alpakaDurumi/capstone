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
        EnablePhysics();    // ���� ������ �ٽ� ���� Ȱ��ȭ
        launched = true;
        CalculatePowers(slowMotion.GetRightHandSpeed());
        rigidbody.AddForce(throwPower * transform.forward, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        // ��ô�� ���¿��� �浹�ߴٸ� �浹 ������Ʈ�� ProjectileTarget ������Ʈ�� �������� �˻�
        if (launched) {
            launched = false;   // �ߺ� �浹�� �����ϱ� ����

            // explode
        }
    }

    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount * 2, 0.0f, 5.0f);
        //spinPower = Mathf.Lerp(0, 1.0f, throwPower / 5.0f);
    }
}
