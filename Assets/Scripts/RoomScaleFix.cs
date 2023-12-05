using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RoomScaleFix : MonoBehaviour
{
    private CharacterController characterController;
    private XROrigin xrOrigin;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();
    }

    private void FixedUpdate() {
        // characterController의 height를 카메라 높이에 동기화
        // 0.15f는 hmd와 정수리 사이의 길이를 임의로 정한 것
        characterController.height = xrOrigin.CameraInOriginSpaceHeight +0.15f;

        // Camera의 월드좌표를 로컬 좌표로 변환
        var centerPoint = transform.InverseTransformPoint(xrOrigin.Camera.transform.position);
        // characterController가 Camera를 따라가도록 수정
        characterController.center = new Vector3(centerPoint.x, characterController.height / 2 + characterController.skinWidth, centerPoint.z);

        // 플레이어가 알아차리지 못할 만큼 position을 수정함으로써 physics검사가 매 프레임 이루어지도록 하여
        // 걸어서 벽을 통과하는 일이 없도록 함
        characterController.Move(new Vector3(0.001f, -0.001f, 0.001f));
        characterController.Move(new Vector3(-0.001f, 0.001f, -0.001f));
    }
}
