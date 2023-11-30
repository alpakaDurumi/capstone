using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Axe : Projectile
{
    private IXRSelectInteractor thrownInteractor;   // ������ ���� �տ� �ش��ϴ� Direct Interactor

    private SlowMotion slowMotion;

    private Transform axeModel;                     // ��ô �� ȸ���� ���� ��

    private float throwPower;
    private float spinPower;

    private Vector3 initialRotation;                // ��ô ���� �ʱ� ȸ����

    protected override void Awake() {
        base.Awake();
        axeModel = transform.GetChild(0);
    }

    private void Start() {
        initialRotation = axeModel.localEulerAngles;
    }

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
        rigidbody.AddForce(throwPower * transform.forward, ForceMode.Impulse);
    }

    private void FixedUpdate() {
        // ���ư��� ���ȿ� ȸ��
        if (launched) {
            SpinAxe();
        }
    }

    private void OnTriggerEnter(Collider other) {
        // ��ô�� ���¿��� �浹�ߴٸ� �浹 ������Ʈ�� ProjectileTarget ������Ʈ�� �������� �˻�
        if (launched) {
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
        axeModel.localEulerAngles = initialRotation;                // spinAxe()�� ȸ���� �𵨿� �ٽ� �ʱ� rotation ����
    }

    // ������ ���ư��� ���� ȸ���ϴ� �� ���
    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount * 2, 0.0f, 5.0f);
        spinPower = Mathf.Lerp(0, 1.0f, throwPower / 5.0f);     // throwPower�� ������ ���� spinPower ����
    }

    // ���� ȸ�� �Լ�
    private void SpinAxe() {
        axeModel.Rotate(0, -spinPower, 0);
    }
}

// update ���� select �Է� �� ���ƿ��� �Լ�? select�ϰ� ���� ���� ������ ����
