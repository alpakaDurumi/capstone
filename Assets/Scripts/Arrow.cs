using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : Projectile
{
    [SerializeField] private float speed = 15.0f;

    private ArrowCaster caster;

    protected RaycastHit hit;

    // �ʱ�ȭ
    protected override void Awake() {
        base.Awake();
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
        SoundManager.Instance.PlayBowShootSound();
        launched = true;
        ApplyForce(notch.PullMeasurer);
        StartCoroutine(LaunchRoutine());
    }

    private void ApplyForce(PullMeasurer pullMeasurer) {
        rigidbody.AddForce(pullMeasurer.PullAmount * speed * transform.forward, ForceMode.VelocityChange);
    }

    private IEnumerator LaunchRoutine() {
        // ȭ���� ���𰡿� �浹�� ������ ��� Ȯ��
        while (!caster.CheckForCollision(out hit)) {
            SetDirection();
            yield return null;
        }
        // �浹�Ѵٸ� �Ʒ� �Լ��� ����
        DisablePhysics();
        CheckForHittable(hit);
    }

    private void SetDirection() {
        if (rigidbody.velocity.z > 0.5f) {
            transform.forward = rigidbody.velocity;
        }
    }
    private void CheckForHittable(RaycastHit hit) {
        ProjectileTarget target = hit.transform.GetComponentInParent<ProjectileTarget>();

        // ArrowTarget ������Ʈ�� ã�� ��쿡��
        // ���Ŀ� �±׸� ���� �ĺ��� ������ �� ����
        if (target != null) {
            target.Hit(hit, this);
        }
        // ArrowTarget�� �ƴ� ��� ȭ���� ������ ȿ���� �߻�
        else {;
            // ����ü�� ���� ������Ʈ�� ����ü�� �θ�� �����Ͽ� ���� �ִ� ȿ�� ����
            transform.SetParent(hit.transform);
        }
    }

    // ȭ���� �߻���� ���� ���¿��� ȭ���� select�� �� ����
    public override bool IsSelectableBy(IXRSelectInteractor interactor) {
        return base.IsSelectableBy(interactor) && !launched;
    }
}
