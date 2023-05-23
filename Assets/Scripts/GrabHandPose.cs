using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���⸦ �׷����� �� �հ����� ��� �����ϱ� ���� ��ũ��Ʈ
public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;


    void Start()
    {
        // �̺�Ʈ ������ ����
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        // Grab ���� �� ����� ��Ÿ���� ������Ʈ�� ������ �ʰ� �ص�
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // ���⸦ select�� DirectInteractor, �� �������� HandData�� �����Ѵ�
            // ��Ÿ�ӿ� prefab���� �����Ǵ� �� ���� DirectInteractor�� ���� ����
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // select���� �� �ִϸ����͵� ��Ȱ��ȭ 
            handData.animator.enabled = false;

            // ���� �������� �����Ϳ� ���⸦ ���� �ո���� HandData�� ���� �޾ƿ´�.
            GetFingerData(handData, rightHandPose);
            // �� �����ϱ�
            SetFingerData(handData, finalFingerRotations);
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // ���⸦ unselect�� DirectInteractor, �� �������� HandData�� �����Ѵ�
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // ��Ȱ��ȭ �ߴ� �ִϸ����͸� �ٽ� Ȱ��ȭ
            handData.animator.enabled = true;

            // �� �����ϱ�
            SetFingerData(handData, startingFingerRotations);
        }
    }


    // ���۰� ������ ���� �� ���� �հ��� �� ȸ�� ������ ������ �Լ�
    public void GetFingerData(HandData h1, HandData h2) {
        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++) {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    // ���ο� �հ��� �� ȸ�� ������ HandData�� �Ҵ��ϴ� �Լ�
    public void SetFingerData(HandData h, Quaternion[] newBonesRotation) {
        for (int i = 0; i < newBonesRotation.Length; i++) {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
