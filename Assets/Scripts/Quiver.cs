using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRBaseInteractable
{
    public GameObject arrowPrefab;

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        CreateAndSelectArrow(args);
    }

    // Quiver가 select된다면 화살을 생성하고 해당 interactor에 select되도록 함
    private void CreateAndSelectArrow(SelectEnterEventArgs args) {
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
    }

    // 화살 생성
    private Arrow CreateArrow(Transform orientation) {
        GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();
    }
}
