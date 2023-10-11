using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    public float PullAmount { get; private set; } = 0.0f;

    public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount);

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        PullAmount = 0.0f;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
        base.ProcessInteractable(updatePhase);

        if (isSelected) {
            // grab�ϴ� ���ȿ� pull values�� ����Ѵ�
            if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
                UpdatePull();
            }
        }
    }

    private void UpdatePull() {
        // ������ ���� ���� position
        Vector3 interactorPosition = firstInteractorSelecting.transform.position;
        // �󸶳� ������ ���
        PullAmount = CalculatePull(interactorPosition);
    }

    private float CalculatePull(Vector3 PullPosition) {
        // ��� ���� ����
        Vector3 pullDirection = PullPosition - start.position;
        // string�� ���� ��ġ�� �ִ� ��ġ�� ����� ���� ����
        Vector3 targetDirection = end.position - start.position;

        // string�� �þ �� �ִ� �ִ� ����
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // �󸶳� ������ 0���� 1���� ������ ��Ÿ����
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0.0f, 1.0f);
    }

    private void OnDrawGizmos() {
        if(start && end) {
            Gizmos.DrawLine(start.position, end.position);
        }
    }
}
