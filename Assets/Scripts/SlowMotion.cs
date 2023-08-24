using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR;

public class SlowMotion : MonoBehaviour
{
    /*
     * 현재는 XR Simulator로 동작하는 함수로 설정
     * Update 문에 바로 위에 조건문을 ~VRDevice 함수로 바꾸면 됨 
     */


    [SerializeField] const float SMOOTH_OFFSET = 0.02f;
    [SerializeField] const float MOVE_DETECT_SPEED = 0.1f;
    [SerializeField] const float NORMAL_SPEED = 1f;
    [SerializeField] const float SLOWMOTION_SPEED = 0.05f;


    private List<XRNodeState> hands;
    private XRNodeState grip;

    private void Start()
    {
        InitVRDevice();
    }


    private void InitVRDevice()
    {
        XRNodeState leftHand = new();
        XRNodeState rightHand = new();

        leftHand.nodeType = XRNode.LeftHand;
        rightHand.nodeType = XRNode.RightHand;

        new List<XRNodeState>()
        {
            leftHand,
            rightHand,
        };
    }

    private void Update()
    {
        if (InputXRSimulator())
        {
            // 정상 속도 
            Time.timeScale = NORMAL_SPEED;
        }
        else
        {
            // 슬로우 모션 
            Time.timeScale = SLOWMOTION_SPEED ;
            Time.fixedDeltaTime = Time.timeScale * SMOOTH_OFFSET;
        }
    }

    
    private bool InputXRSimulator()
    {
        return DetectHandsMove() || DetectButtonAction();
    }


    // 손의 움직임을 감지(회전도 포함)
    private bool DetectHandsMove()
    {
        bool leftHandHold = XRDeviceSimulator.instance.manipulatingLeftController;
        bool rightHandHold = XRDeviceSimulator.instance.manipulatingRightController;
        object obj = XRDeviceSimulator.instance.mouseDeltaAction.action.ReadValueAsObject();

        if (obj == null) return false;

        Vector2 mouseVector = (Vector2)obj;
        float mouseSpeed = mouseVector.magnitude;

        return (leftHandHold || rightHandHold) && mouseSpeed >= MOVE_DETECT_SPEED;

        
    }

    // 왼쪽 클릭과 그립 버튼 감지
    private bool DetectButtonAction()
    {
        bool gripActive = XRDeviceSimulator.instance.gripAction.action.WasPerformedThisFrame();
        bool leftClickActive = XRDeviceSimulator.instance.triggerAction.action.WasPressedThisFrame();

        return gripActive || leftClickActive;
        
    }

    // VR 기기 연결했을 때의 입력 구분
    private bool InputVRDevice()
    {
        return (DetectHandsMoveByDevice() || DetectButtonActionByDevice());
    }

    // 손 움직임을 감지
    private bool DetectHandsMoveByDevice()
    {
        foreach (XRNodeState hand in hands)
        {
            Vector3 velocity = new();

            if (hand.TryGetVelocity(out velocity))
            {
                if (velocity.magnitude > MOVE_DETECT_SPEED) return true;
            }
        }

        return false;
    }

    // 그립 버튼과 트리거 버튼 감지 
    private bool DetectButtonActionByDevice()
    {
        return CommonUsages.gripButton.Equals(true) ||
            CommonUsages.triggerButton.Equals(true) ;
    }
}
