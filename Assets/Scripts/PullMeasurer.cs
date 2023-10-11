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
            // grab하는 동안에 pull values를 계산한다
            if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
                UpdatePull();
            }
        }
    }

    private void UpdatePull() {
        // 시위를 잡은 손의 position
        Vector3 interactorPosition = firstInteractorSelecting.transform.position;
        // 얼마나 당겼는지 계산
        PullAmount = CalculatePull(interactorPosition);
    }

    private float CalculatePull(Vector3 PullPosition) {
        // 당긴 방향 벡터
        Vector3 pullDirection = PullPosition - start.position;
        // string의 시작 위치와 최대 위치를 계산한 기준 벡터
        Vector3 targetDirection = end.position - start.position;

        // string이 늘어날 수 있는 최대 길이
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // 얼마나 당겼는지 0에서 1사이 값으로 나타낸다
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0.0f, 1.0f);
    }

    private void OnDrawGizmos() {
        if(start && end) {
            Gizmos.DrawLine(start.position, end.position);
        }
    }
}
