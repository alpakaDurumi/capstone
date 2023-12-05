using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_rifle : Enemy
{
    private bool reloading = false;

    private int attack_cnt = 0;
    private int magazine = 4;

    private BulletShooter shooter;

    private Transform aimStart;                             // target���� ������ ����ϱ� ���� ������

    [SerializeField] private LayerMask layerMask_canSee;    // �þ� ���̾��ũ : �⺻������ Player�� Stage ����

    private float angle_horizontal;
    private float angle_vertical;

    private bool turning = false;

    public GameObject grenadePrefab;
    public GameObject rightHand;

    public bool haveGrenade = false;
    private int grenadeLayerIndex = 5;

    private float animationPercentToRelease = 0.57f;

    protected override void Awake() {
        base.Awake();
        shooter = GetComponentInChildren<BulletShooter>();
    }

    protected override void Start() {
        base.Start();
        attackLayerIndex = 3;
        aimStart = transform.GetChild(1);
    }

    protected override void Update()
    {
        base.Update();
        // �Ѿ� ��� �Ҹ� �� ������
        if (attack_cnt == magazine) {
            Reload();
            StartCoroutine(WaitReloadEnd((b) => { reloading = b; }));
        }
        // target �������� aim
        AimTarget();
    }

    // target�� ���� �������� ����
    private bool CanSee() {
        bool isHit = Physics.Raycast(aimStart.position, target.position - aimStart.position, out RaycastHit hitInfo, attackDistance, layerMask_canSee);
        //Debug.DrawRay(aimStart.position, (target.position - aimStart.position).normalized * attackDistance, Color.red);

        // ������ �ݶ��̴��� ���� ���
        if (isHit) {
            // �÷��̾ �� �� �ִٸ�
            if (hitInfo.transform.tag == "Player") {
                return true;
            }
            else {
                return false;
            }
        }
        // ������ �ƹ� �ݶ��̴��� ������ ���� ���
        else {
            return false;
        }
    }

    // ���� ���� ����
    protected override bool CanAttack() {
        // target�� �� �� �ִٸ� ������ ���� �Ǵ�
        if (CanSee()) {
            if (currentDistance <= attackDistance) {
                // ���� ������ ���� ���� target�� ������, ���̹� ���� ������ ��� ��쿡�� ȸ��
                if (turning) {
                    TurnBody();
                }

                if (!attacking && attack_timer >= attack_waitingTime && !reloading) {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    protected override void Attack() {
        if (haveGrenade) {
            SwingGrenade();
            haveGrenade = false;
        }
        else {
            base.Attack();
            attack_cnt++;
            shooter.Shoot();
        }
    }

    // ������ �Լ�
    private void Reload() {
        reloading = true;
        animator.SetTrigger("reload");
        attack_cnt = 0;
    }

    private IEnumerator WaitReloadEnd(System.Action<bool> callback) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(attackLayerIndex).IsName("Reload") && animator.GetCurrentAnimatorStateInfo(attackLayerIndex).normalizedTime >= 1.0f);
        callback(false);
    }

    // target �������� �����ϱ� ���� �Լ�
    private void AimTarget() {
        // target ���������� ���� ������ ���� ������ ���
        Vector3 dir = aimStart.InverseTransformPoint(target.position);
        angle_horizontal = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        angle_vertical = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;

        float[] horizontal_limit = new float[2];
        float[] vertical_limit = new float[2];

        // �Ϲݽű��� �������� �ִ�� ������ ����������, ��Ÿ�� �� �ƹ�Ÿ ����ũ�� ������ �������� ������� ����
        // ����ũ�� ����� ���̾�� ������� ���� ���̾ ���� �ΰ�, ���� ���ο� ���� weight�� ������ ���� ���

        // ������ ���¶��
        if (!animator.GetBool("move")) {
            animator.SetLayerWeight(1, 1);      // Upper Layer Stopped�� Weight�� 1�� ����
            animator.SetLayerWeight(2, 0);      // Upper Layer Moving�� Weight�� 0���� ����

            // �Ϲݽű��� ������� �� �ִ� ����
            horizontal_limit[0] = -50;
            horizontal_limit[1] = 66;
            vertical_limit[0] = -54;
            vertical_limit[1] = 43;
        }
        // �����̴� ���¶��
        else {
            animator.SetLayerWeight(1, 0);      // Upper Layer Stopped�� Weight�� 0�� ����
            animator.SetLayerWeight(2, 1);      // Upper Layer Moving�� Weight�� 1���� ����

            // ��ݽŸ� ������� �� �ִ� ����
            horizontal_limit[0] = -52;
            horizontal_limit[1] = 53;
            vertical_limit[0] = -33;
            vertical_limit[1] = 43;
        }

        // ���� ȸ������ �ʰ� ������ ������ �������
        if (horizontal_limit[0] <= angle_horizontal & angle_horizontal <= horizontal_limit[1]) {
            // ����
            if (angle_horizontal < 0) {
                var t = Scale(horizontal_limit[0], 0, -1, 0, angle_horizontal);
                animator.SetFloat("body_horizontal", t);
            }
            // ������
            else {
                var t = Scale(0, horizontal_limit[1], 0, 1, angle_horizontal);
                animator.SetFloat("body_horizontal", t);
            }
        }
        // ȸ���� �ʿ��� ���
        else {
            turning = true;          
        }


        // ������ ������ �������
        if (vertical_limit[0] <= angle_vertical && angle_vertical <= vertical_limit[1]) {
            // �Ʒ�
            if (angle_vertical < 0) {
                var t = Scale(vertical_limit[0], 0, -1, 0, angle_vertical);
                animator.SetFloat("body_vertical", t);
            }
            // ��
            else {
                var t = Scale(0, vertical_limit[1], 0, 1, angle_vertical);
                animator.SetFloat("body_vertical", t);
            }
        }
        else {
            // �ڷ� �������� �þ� Ȯ���ϱ�?
        }
    }

    // ���� range�� �����ϴ� �Լ�
    private float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue) {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    // �� ȸ�� �Լ�
    private void TurnBody() {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(target.position.x - aimStart.position.x, 0, target.position.z - aimStart.position.z));
        transform.rotation = Quaternion.RotateTowards(aimStart.rotation, targetRotation, 120f * Time.deltaTime);

        // ���� ȸ��
        if (angle_horizontal < 0) {
            animator.SetInteger("turn", 1);
        }
        // ������ ȸ��
        else {
            animator.SetInteger("turn", 2);
        }

        // ��ǥ ���⿡ �����ߴٸ�
        if (-1.0f < angle_horizontal && angle_horizontal < 1.0f) {
            animator.SetInteger("turn", 0);
            turning = false;
        }
    }

    // ����ź ������ �Լ�
    private void SwingGrenade() {
        // �ִϸ��̼� ���
        animator.SetTrigger("grenade");
        // ����ź ������Ʈ ���� �� �� ��ġ�� ����
        GameObject grenade = Instantiate(grenadePrefab, rightHand.transform.position, rightHand.transform.rotation);
        grenade.transform.SetParent(rightHand.transform);
        
        StartCoroutine(WaitGrenadeRelease(grenade));
    }

    // �ִϸ��̼� �� �տ��� ����ź�� ������ ������ ���ϱ� ���� �ڷ�ƾ
    private IEnumerator WaitGrenadeRelease(GameObject grenade) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(grenadeLayerIndex).IsName("Grenade") && animator.GetCurrentAnimatorStateInfo(grenadeLayerIndex).normalizedTime >= animationPercentToRelease);
        ReleaseGrenade(grenade);
    }

    private void ReleaseGrenade(GameObject grenade) {        
        // �տ��� �и�
        rightHand.transform.DetachChildren();
        grenade.transform.position = rightHand.transform.position;
        // ��ǥ ���� & release
        grenade.GetComponent<Grenade>().targetPosition = target.transform.position;
        grenade.GetComponent<Grenade>().Release();
    }
}
