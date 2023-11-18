using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0.25f;

    public Bow Bow { get; private set; }
    public PullMeasurer PullMeasurer { get; private set; }

    // 0.25f �̻����� ����� ������ �߻� ����
    public bool CanRelease => PullMeasurer.PullAmount > releaseThreshold;

    protected override void Awake() {
        base.Awake();
        Bow = GetComponentInParent<Bow>();
        PullMeasurer = GetComponentInChildren<PullMeasurer>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        PullMeasurer.selectExited.AddListener(ReleaseArrow);
    }

    protected override void OnDisable() {
        base.OnDisable();
        PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
    }

    // string�� ������ �� arrow�� notch���� select exit��Ű�� �Լ�
    public void ReleaseArrow(SelectExitEventArgs args) {
        if (hasSelection) {
            interactionManager.SelectExit(this, firstInteractableSelected);
        }
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
        base.ProcessInteractor(updatePhase);
        if (Bow.isSelected){
            UpdateAttach();
        }
    }

    // arrow�� attach�� ��ġ�� ������ ��� �� ��������� �ϴ� �Լ�
    public void UpdateAttach() {
        attachTransform.position = PullMeasurer.PullPosition;
    }

    public override bool CanSelect(IXRSelectInteractable interactable) {
        //
        return base.CanSelect(interactable) && CanHover(interactable) && interactable is Arrow && Bow.isSelected;
    }

    // notch�� arrow�� �ڵ����� grab�ϵ��� �Ѵ�
    private bool QuickSelect(IXRSelectInteractable interactable) {
        return !hasSelection || IsSelecting(interactable);
    }

    // socket�� arrow�� �ٽ� select�ϴ� ���� �����ϱ� ����
    private bool CanHover(IXRSelectInteractable interactable) {
        if(interactable is IXRHoverInteractable hoverInteractable) {
            return CanHover(hoverInteractable);
        }
        return false;
    }

}
