using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponChanger : MonoBehaviour
{
    public XRInteractionManager interactionManager;

    public GameObject[] Weapons;
    private GameObject currentWeapon = null;

    private int currentWeaponIdx = -1;

    private XRDirectInteractor leftHand, rightHand;
    private IXRSelectInteractor targetInteractor;

    private GameObject quiver;

    private int killCnt = 0;
    private int cntToChange = 1;    // ���� ������ ���� �ʿ��� óġ ��

    private void Awake() {
        leftHand = GameObject.Find("LeftHand").GetComponentInChildren<XRDirectInteractor>();
        rightHand = GameObject.Find("RightHand").GetComponentInChildren<XRDirectInteractor>();

        quiver = transform.Find("Quiver").gameObject;
    }

    // ���� ���� �Լ�
    public void ChangeToRandomWeapon() {
        // ���� ���⸦ ������ ���� �� ���� ����
        int nextWeaponIdx = Random.Range(0, Weapons.Length);
        if(nextWeaponIdx == currentWeaponIdx) {
            nextWeaponIdx = (nextWeaponIdx + 1) % Weapons.Length;
        }

        // ���⿡ ���� select �� �� ����
        SetTargetInteractor(nextWeaponIdx);

        // ���⿡ ���� ���� �����Ǿ����� �ʴٸ� ������ ���
        if(targetInteractor == null) {
            Debug.LogError("Randomly selected Weapon's targetInteractor is NULL");
            return;
        }

        // ���� ���� ����
        if (currentWeapon != null) {
            Destroy(currentWeapon);
        }

        // ���� ���� �� ������ �տ� select
        XRGrabInteractable nextWeapon = CreateWeapon(nextWeaponIdx, targetInteractor.transform);
        interactionManager.SelectEnter(targetInteractor, nextWeapon);

        currentWeapon = nextWeapon.gameObject;
        currentWeaponIdx = nextWeaponIdx;
    }

    // weaponIdx�� ���� ���� ��(interactor)�� �����ִ� �Լ�
    private void SetTargetInteractor(int weaponIdx) {
        if (Weapons[weaponIdx].name.Equals("Bow")) {
            targetInteractor = leftHand;
            SetQuiverStatus(true);     // Ȱ�� ���, Quiver Ȱ��ȭ
            SetRightToPrimary(false);
        }
        else {
            if (Weapons[weaponIdx].name.Equals("Sword")) {
                targetInteractor = rightHand;
            }
            else if (Weapons[weaponIdx].name.Equals("Pistol")) {
                targetInteractor = rightHand;
            }
            else if (Weapons[weaponIdx].name.Equals("Axe")) {
                targetInteractor = rightHand;
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

    // value�� true�� ��� �������� Toggle��, �޼��� State�� �����ϴ� �Լ�
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

    // ų ī��Ʈ�� ������Ű�� �Լ�
    public void IncreaseKillCount() {
        killCnt++;
        // ��ǥ ī��Ʈ�� �����ϸ� ���� ����
        if(killCnt == cntToChange) {
            ChangeToRandomWeapon();
            killCnt = 0;
        }
    }
}
