using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
public class SlowMotion : MonoBehaviour
{
    /*
     * 해당 스크립트를 VR 손 모델이나 자체에 넣으면 됨
     * VR 키 입력을 받아야하는데, 아무리해도 안받아져서 
     * 일단은 공백으로 비어두고 주석처리했음
     * 
     * 입력조건만 넣으면 정상적으로 작동함
     * 
     */
    [SerializeField] const float SMOOTH_OFFSET = 0.02f;
    [SerializeField] const float MOUSE_DETECT_SPEED = 0.1f;
    [SerializeField] const float NORMAL_SPEED = 1f;
    [SerializeField] const float SLOWMOTION_SPEED = 0.05f;
   

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

        return (leftHandHold || rightHandHold) && mouseSpeed >= MOUSE_DETECT_SPEED;

        
    }

    // 왼쪽 클릭과 그립 버튼 감지
    private bool DetectButtonAction()
    {
        bool gripActive = XRDeviceSimulator.instance.gripAction.action.WasPerformedThisFrame();
        bool leftClickActive = XRDeviceSimulator.instance.triggerAction.action.WasPressedThisFrame();

        return gripActive || leftClickActive;
        
    }
}
