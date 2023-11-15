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
    [SerializeField] const float MOVE_DETECT_SPEED = 0.2f;
    [SerializeField] const float MOVE_DETECT_THREDHOLD = 0.2f;
    [SerializeField] const float NORMAL_SPEED = 1f;
    [SerializeField] const float SLOWMOTION_SPEED = 0.05f;

    public InputDevice rightController;
    public InputDevice leftController;
    public InputDevice hmd;

    private void Start()
    {
        InitDevice();
    }

    private void Update()
    {
        // 해당 앞쪽 부분을 InputVRDevice와 InputXRSimulator 함수로 바꿔가면서 쓰면
        if (InputVRDevice()|| !GameManager.Instance.IsStartRound)
        {
            // 정상 속도 
            Time.timeScale = NORMAL_SPEED;
        }
        else 
        {
            // 슬로우 모션 
            Time.timeScale = SLOWMOTION_SPEED;
            Time.fixedDeltaTime = Time.timeScale * SMOOTH_OFFSET;
        }
    }
/*
 * 시뮬레이터 전용 함/
 */
    
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


/*
* 오큘러스 2 전용 세팅 관련 함수
* 
*/

    private void InitDevice()
    {
        if (!rightController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        }
        if (!leftController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
        }
        if (!hmd.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref hmd);
        }
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }

    // VR 기기 연결했을 때의 입력 구분
    private bool InputVRDevice()
    {
        return DetectHandsMoveByDevice();
    }

    // 손 움직임을 감지
    private bool DetectHandsMoveByDevice()
    {
        float leftSpeed = 0f;
        float rightSpeed = 0f;

        if (leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            leftSpeed = leftVelocity.magnitude;
        }
        if (rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVeleocity))
        {
            rightSpeed = rightVeleocity.magnitude;
        }

        return (leftSpeed >= MOVE_DETECT_THREDHOLD
            || rightSpeed > MOVE_DETECT_THREDHOLD); 

    }
}
