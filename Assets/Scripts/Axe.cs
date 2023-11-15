using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Axe : Projectile
{
    private IXRSelectInteractor thrownInteractor;   // ������ ���� �տ� �ش��ϴ� Direct Interactor

    private SlowMotion slowMotion;

    private float throwPower;
    private float spinPower;

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        // ������ select�� XR Origin�� slowMotion ������Ʈ ����
        slowMotion = args.interactorObject.transform.GetComponentInParent<SlowMotion>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // ���� ������ �ٽ� ���� Ȱ��ȭ
        launched = true;
        thrownInteractor = args.interactorObject;   // ���� interactor�� ���

        CalculatePowers(slowMotion.GetRightHandSpeed());
        rigidbody.AddForce(throwPower * thrownInteractor.transform.forward, ForceMode.Impulse);
    }

    private void FixedUpdate() {
        // ���ư��� ���ȿ� ȸ��
        if (launched) {
            SpinAxe();
        }
    }

    private void OnTriggerEnter(Collider other) {
        // ��ô�� ���¿��� �浹�ߴٸ� �浹 ������Ʈ�� ProjectileTarget ������Ʈ�� �������� �˻�
        if (launched && other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")) {
            launched = false;   // �ߺ� �浹�� �����ϱ� ����
            DisablePhysics();   // ���� ��Ȱ��ȭ(�����ִ� ȿ��)
            
            ProjectileTarget target = other.transform.GetComponentInParent<ProjectileTarget>();
            // ������ �ִٸ�
            if(target != null) {
                target.Hit(other.transform, this);
            }
            else {
                transform.SetParent(other.transform);
                // ������ ��� ���� �տ� �ٽ� ���ƿ���
                StartCoroutine(ReturnRoutine());
            }
        }
    }

    // 1.0�� �� ���� interactor�� �ٽ� select �ǵ��� �ϴ� �ڷ�ƾ
    private IEnumerator ReturnRoutine() {
        yield return new WaitForSeconds(1.0f);
        interactionManager.SelectEnter(thrownInteractor, this);
    }

    // ������ ���ư��� ���� ȸ���ϴ� �� ���
    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount, 0.0f, 2.0f);
        spinPower = Mathf.Clamp(amount / 10, 0.0f, 1.0f);
    }

    // ���� ȸ�� �Լ�
    private void SpinAxe() {
        transform.Rotate(spinPower, 0, 0);
    }
}
