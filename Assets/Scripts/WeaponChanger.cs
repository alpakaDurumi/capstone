using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] private XRInteractionManager interactionManager;

    [SerializeField] private GameObject[] Weapons;
    private GameObject currentWeapon = null;

    private int currentWeaponIdx = -1;

    private XRDirectInteractor leftHand, rightHand;
    private IXRSelectInteractor targetInteractor;

    private GameObject quiver;

    private int killCnt = 0;
    private int cntToChange = 1;    // 무기 변경을 위해 필요한 처치 수

    [SerializeField] private InputActionManager inputActionManager;   // 왼손과 오른손의 select를 제한하기 위하여 접근

    private InputAction leftHandSelect;
    private InputAction rightHandSelect;

    private void Awake() {
        leftHand = GameObject.Find("LeftHand").GetComponentInChildren<XRDirectInteractor>();
        rightHand = GameObject.Find("RightHand").GetComponentInChildren<XRDirectInteractor>();

        quiver = transform.GetChild(2).gameObject;  // 인덱스 수정
    }

    private void Start() {
        // 왼손과 오른손의 select action을 각각 할당
        leftHandSelect = inputActionManager.actionAssets[0].FindAction("XRI LeftHand Interaction/Select");
        rightHandSelect = inputActionManager.actionAssets[0].FindAction("XRI RightHand Interaction/Select");
    }

    // 무기 변경 함수
    public void ChangeToRandomWeapon() {
        // 현재 무기를 제외한 무기 중 랜덤 선택
        int nextWeaponIdx = Random.Range(0, Weapons.Length);
        if(nextWeaponIdx == currentWeaponIdx) {
            nextWeaponIdx = (nextWeaponIdx + 1) % Weapons.Length;
        }

        // 무기에 따라 select 될 손 지정
        SetTargetInteractor(nextWeaponIdx);

        // 무기에 따른 손이 지정되어있지 않다면 에러문 출력
        if(targetInteractor == null) {
            Debug.LogError("Randomly selected Weapon's targetInteractor is NULL");
            return;
        }

        // 이전 무기 제거
        if (currentWeapon != null) {
            Destroy(currentWeapon);
        }

        // 무기 생성 후 지정된 손에 select
        XRGrabInteractable nextWeapon = CreateWeapon(nextWeaponIdx, targetInteractor.transform);
        interactionManager.SelectEnter(targetInteractor, nextWeapon);

        currentWeapon = nextWeapon.gameObject;
        currentWeaponIdx = nextWeaponIdx;
    }

    // weaponIdx에 따라 잡을 손(interactor)을 정해주는 함수
    private void SetTargetInteractor(int weaponIdx) {
        if (Weapons[weaponIdx].name.Equals("Bow")) {
            targetInteractor = leftHand;
            SetQuiverStatus(true);     // 활의 경우, Quiver 활성화
            SetRightToPrimary(false);

            SetSelectInput(false, true);
        }
        else {
            if (Weapons[weaponIdx].name.Equals("Sword")) {
                targetInteractor = rightHand;
                SetSelectInput(true, false);
            }
            else if (Weapons[weaponIdx].name.Equals("Pistol")) {
                targetInteractor = rightHand;
                SetSelectInput(true, false);
            }
            else if (Weapons[weaponIdx].name.Equals("Axe")) {
                targetInteractor = rightHand;
                SetSelectInput(true, true);     // 도끼는 투척을 해야하므로 두 손 모두 true
            }
            SetQuiverStatus(false);
            SetRightToPrimary(true);
        }
    }

    private XRGrabInteractable CreateWeapon(int weaponIdx, Transform hand) {
        GameObject weapon = Instantiate(Weapons[weaponIdx], hand.position, hand.rotation);
        return weapon.GetComponent<XRGrabInteractable>();
    }

    private void SetQuiverStatus(bool value) {
        quiver.SetActive(value);
    }

    // value가 true인 경우 오른손을 Toggle로, 왼손을 State로 변경하는 함수
    private void SetRightToPrimary(bool value) {
        if (value) {
            leftHand.selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.StateChange;
            rightHand.selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Sticky;
        }
        else {
            leftHand.selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Sticky;
            rightHand.selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.StateChange;
        }
    }

    // 킬 카운트를 증가시키는 함수
    public void IncreaseKillCount() {
        killCnt++;
        // 목표 카운트에 도달하면 무기 변경
        if(killCnt == cntToChange) {
            ChangeToRandomWeapon();
            killCnt = 0;
        }
    }

    // 왼손과 오른손의 select input 활성화 여부를 세팅하는 함수
    private void SetSelectInput(bool leftHandValue, bool rightHandValue) {
        if (leftHandValue) {
            leftHandSelect.Enable();
        }
        else {
            leftHandSelect.Disable();
        }

        if (rightHandValue) {
            rightHandSelect.Enable();
        }
        else {
            rightHandSelect.Disable();
        }
    }
}