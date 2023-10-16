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

    // �ʱ�ȭ
    protected override void Awake() {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        caster = GetComponent<ArrowCaster>();
    }

    // notch(����)�� �������ٸ�
    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        if(args.interactorObject is Notch notch) {
            if (notch.CanRelease) {
                LaunchArrow(notch);
            }
        }
    }

    // ȭ�� �߻�
    private void LaunchArrow(Notch notch) {
        launched = true;
        ApplyForce(notch.PullMeasurer);
        StartCoroutine(LaunchRoutine());
    }

    private void ApplyForce(PullMeasurer pullMeasurer) {
        rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
    }

    private IEnumerator LaunchRoutine() {
        // ȭ���� ���𰡿� �浹�� ������ ��� Ȯ��
        while(!caster.CheckForCollision(out hit)) {
            SetDirection();
            yield return null;
        }
        // �浹�Ѵٸ� �Ʒ� �Լ��� ����
        DisablePhysics();
        CheckForHittable(hit);
    }

    private void SetDirection() {
        if(rigidbody.velocity.z > 0.5f) {
            transform.forward = rigidbody.velocity;
        }
    }

    // ȭ�� ����
    private void DisablePhysics() {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    // ȭ�쿡 ���� ������Ʈ�� ȭ���� �θ�� ����
    // ȭ���� ���� �ִ� ȿ��
    private void ChildArrow(RaycastHit hit) {
        transform.SetParent(hit.transform);
    }

    private void CheckForHittable(RaycastHit hit) {
        ArrowTarget target = hit.transform.GetComponentInParent<ArrowTarget>();

        // ArrowTarget ������Ʈ�� ã�� ��쿡��
        // ���Ŀ� �±׸� ���� �ĺ��� ������ �� ����
        if(target != null) {
            target.Hit(hit, this);
        }
        // ArrowTarget�� �ƴ� ��� ȭ���� ������ ȿ���� �߻�
        else {
            ChildArrow(hit);
        }
    }

    // ȭ���� �߻���� ���� ���¿��� ȭ���� select�� �� ����
    public override bool IsSelectableBy(IXRSelectInteractor interactor) {
        return base.IsSelectableBy(interactor) && !launched;
    }
}
