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

    // Quiver�� select�ȴٸ� ȭ���� �����ϰ� �ش� interactor�� select�ǵ��� ��
    private void CreateAndSelectArrow(SelectEnterEventArgs args) {
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
    }

    // ȭ�� ����
    private Arrow CreateArrow(Transform orientation) {
        GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();
    }
}
