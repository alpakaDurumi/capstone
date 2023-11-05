using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponChanger : MonoBehaviour
{
    public XRInteractionManager interactionManager;

    public GameObject[] Weapons;
    private GameObject currentWeapon;

    private int currentWeaponIdx = -1;

    private XRDirectInteractor leftHand, rightHand;
    private IXRSelectInteractor targetInteractor;

    private GameObject quiver;

    private void Awake() {
        leftHand = GameObject.Find("LeftHand").GetComponentInChildren<XRDirectInteractor>();
        rightHand = GameObject.Find("RightHand").GetComponentInChildren<XRDirectInteractor>();

        quiver = transform.Find("Quiver").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeToRandomWeapon();
        }
    }

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

        // 무기 생성 후 지정된 손에 select
        XRGrabInteractable nextWeapon = CreateWeapon(nextWeaponIdx, targetInteractor.transform);
        interactionManager.SelectEnter(targetInteractor, nextWeapon);

        Destroy(currentWeapon);
        currentWeapon = nextWeapon.gameObject;
    }

    // weaponIdx에 따라 잡을 손(interactor)을 정해주는 함수
    private void SetTargetInteractor(int weaponIdx) {
        if (Weapons[weaponIdx].name.Equals("Sword")) {
            targetInteractor = rightHand;
            SetQuiverStatus(false);
        }
        else if (Weapons[weaponIdx].name.Equals("Pistol")) {
            targetInteractor = rightHand;
            SetQuiverStatus(false);
        }
        else if (Weapons[weaponIdx].name.Equals("Bow")) {
            targetInteractor = leftHand;
            SetQuiverStatus(true);     // 활의 경우, Quiver 활성화
        }
        else if (Weapons[weaponIdx].name.Equals("Axe")) {
            targetInteractor = rightHand;
            SetQuiverStatus(false);
        }
    }

    private XRGrabInteractable CreateWeapon(int weaponIdx, Transform hand) {
        GameObject weapon = Instantiate(Weapons[weaponIdx], hand.position, hand.rotation);
        return weapon.GetComponent<XRGrabInteractable>();
    }

    private void SetQuiverStatus(bool value) {
        quiver.SetActive(value);
    }
}
