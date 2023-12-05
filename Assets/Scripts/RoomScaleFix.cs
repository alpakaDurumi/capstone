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
        // characterController�� height�� ī�޶� ���̿� ����ȭ
        // 0.15f�� hmd�� ������ ������ ���̸� ���Ƿ� ���� ��
        characterController.height = xrOrigin.CameraInOriginSpaceHeight +0.15f;

        // Camera�� ������ǥ�� ���� ��ǥ�� ��ȯ
        var centerPoint = transform.InverseTransformPoint(xrOrigin.Camera.transform.position);
        // characterController�� Camera�� ���󰡵��� ����
        characterController.center = new Vector3(centerPoint.x, characterController.height / 2 + characterController.skinWidth, centerPoint.z);

        // �÷��̾ �˾������� ���� ��ŭ position�� ���������ν� physics�˻簡 �� ������ �̷�������� �Ͽ�
        // �ɾ ���� ����ϴ� ���� ������ ��
        characterController.Move(new Vector3(0.001f, -0.001f, 0.001f));
        characterController.Move(new Vector3(-0.001f, 0.001f, -0.001f));
    }
}
