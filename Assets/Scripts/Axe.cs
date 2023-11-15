using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Axe : Projectile
{
    private IXRSelectInteractor thrownInteractor;   // ������ ���� �տ� �ش��ϴ� Direct Interactor

    //private SlowMotion slowMotion;

    //private float handSpeedAmount;

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        //slowMotion = args.interactorObject.transform.GetComponentInParent<SlowMotion>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // ���� ������ �ٽ� ���� Ȱ��ȭ
        launched = true;
        thrownInteractor = args.interactorObject;   // ���� interactor�� ���

        //handSpeedAmount = slowMotion.GetRightHandSpeed();
        //rigidbody.AddForce(handSpeedAmount * thrownInteractor.transform.forward, ForceMode.VelocityChange); 
        rigidbody.AddForce(10 * thrownInteractor.transform.forward, ForceMode.VelocityChange);
    }

    private void FixedUpdate() {
        // ���ư��� ���ȿ� ȸ��
        if (launched) {
            RotateAxe();
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

    // ���� ȸ�� �Լ�
    private void RotateAxe() {
        transform.Rotate(3, 0, 0);
    }
}
