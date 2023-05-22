using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 무기를 그랩했을 때 포즈를 지정하기 위한 스크립트
public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;


    void Start()
    {
        // 이벤트 리스너 설정
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // 무기를 select한 DirectInteractor, 즉 오른손의 HandData를 참조한다
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // select했을 때 애니메이터도 비활성화 
            handData.animator.enabled = false;

            // 현재 오른손의 데이터와 무기를 잡은 손모양의 HandData를 각각 받아온다.
            SetHandDataValues(handData, rightHandPose);
            // 값 설정하기
            SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // 무기를 select한 DirectInteractor, 즉 오른손의 HandData를 참조한다
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // select했을 때 애니메이터도 비활성화 
            handData.animator.enabled = true;

            // 값 설정하기
            SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
        }
    }


    // 시작과 끝으로 삼을 두 HandData를 설정하는 함수
    public void SetHandDataValues(HandData h1, HandData h2) {
        startingHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++) {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    // 새로운 값들을 HandData에 할당하는 함수
    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation) {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++) {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
