using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 무기를 그랩했을 때 손가락의 포즈를 지정하기 위한 스크립트
public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;


    void Start()
    {
        // 이벤트 리스너 설정
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        // Grab 시의 손 모양을 나타내는 오브젝트는 보이지 않게 해둠
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // 무기를 select한 DirectInteractor, 즉 오른손의 HandData를 참조한다
            // 런타임에 prefab으로 생성되는 손 모델은 DirectInteractor와 형제 관계
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // select했을 때 애니메이터도 비활성화 
            handData.animator.enabled = false;

            // 현재 오른손의 데이터와 무기를 잡은 손모양의 HandData를 각각 받아온다.
            GetFingerData(handData, rightHandPose);
            // 값 설정하기
            SetFingerData(handData, finalFingerRotations);
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg) {
        if (arg.interactorObject is XRDirectInteractor) {
            // 무기를 unselect한 DirectInteractor, 즉 오른손의 HandData를 참조한다
            HandData handData = arg.interactorObject.transform.parent.GetComponentInChildren<HandData>();
            // 비활성화 했던 애니메이터를 다시 활성화
            handData.animator.enabled = true;

            // 값 설정하기
            SetFingerData(handData, startingFingerRotations);
        }
    }


    // 시작과 끝으로 삼을 두 쌍의 손가락 본 회전 값들을 얻어오는 함수
    public void GetFingerData(HandData h1, HandData h2) {
        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++) {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    // 새로운 손가락 본 회전 값들을 HandData에 할당하는 함수
    public void SetFingerData(HandData h, Quaternion[] newBonesRotation) {
        for (int i = 0; i < newBonesRotation.Length; i++) {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
