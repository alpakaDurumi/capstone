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

        // ���� ���� �� ������ �տ� select
        XRGrabInteractable nextWeapon = CreateWeapon(nextWeaponIdx, targetInteractor.transform);
        interactionManager.SelectEnter(targetInteractor, nextWeapon);

        Destroy(currentWeapon);
        currentWeapon = nextWeapon.gameObject;
    }

    // weaponIdx�� ���� ���� ��(interactor)�� �����ִ� �Լ�
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
            SetQuiverStatus(true);     // Ȱ�� ���, Quiver Ȱ��ȭ
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
