using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0.25f;

    public Bow Bow { get; private set; }
    public PullMeasurer PullMeasurer { get; private set; }

    // 0.25f 이상으로 당겼을 때에만 발사 가능
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

    // string을 놓았을 때 arrow를 notch에서 select exit시키는 함수
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

    // arrow가 attach된 위치가 시위를 당길 때 따라오도록 하는 함수
    public void UpdateAttach() {
        attachTransform.position = PullMeasurer.PullPosition;
    }

    public override bool CanSelect(IXRSelectInteractable interactable) {
        //
        return base.CanSelect(interactable) && CanHover(interactable) && interactable is Arrow && Bow.isSelected;
    }

    // notch가 arrow를 자동으로 grab하도록 한다
    private bool QuickSelect(IXRSelectInteractable interactable) {
        return !hasSelection || IsSelecting(interactable);
    }

    // socket이 arrow를 다시 select하는 것을 방지하기 위함
    private bool CanHover(IXRSelectInteractable interactable) {
        if(interactable is IXRHoverInteractable hoverInteractable) {
            return CanHover(hoverInteractable);
        }
        return false;
    }

}
