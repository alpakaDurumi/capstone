using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : Projectile
{
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform attachPoint_left;
    [SerializeField] private Transform attachPoint_right;

    private SlowMotion slowMotion;

    private float throwPower;
    //private float spinPower;

    private bool released = false;

    private float timer = 0.0f;
    private float timeToExplode = 3.0f;

    public Vector3 targetPosition;

    private bool collidedWithStage = false;

    [SerializeField] private GameObject ExplosionEffect;    // ���� ��ƼŬ ����Ʈ

    private bool exploded = false;

    private void Start() {
        DisablePhysics();
    }

    private void Update() {
        // �������� �ʾҰ�
        if (!exploded) {
            // �������ٸ� Ÿ�̸� ����
            if (released) {
                timer += Time.deltaTime;
            }

            if(timer >= timeToExplode) {
                Explode();
            }
        }

        // �� �տ� ���� ������ �´� attachPoint ����
        if (GameManager.Instance.isPrimaryHandRight) {
            attachPoint.transform.position = attachPoint_left.position;
            attachPoint.transform.rotation = attachPoint_left.rotation;
        }
        else {
            attachPoint.transform.position = attachPoint_right.position;
            attachPoint.transform.rotation = attachPoint_right.rotation;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stage")) {
            collidedWithStage = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        EnablePhysics();    // ���� ������ �ٽ� ���� Ȱ��ȭ
        launched = true;
        CalculatePowers(slowMotion.GetRightHandSpeed());
        rigidbody.AddForce(throwPower * transform.forward, ForceMode.Impulse);
    }

    private void CalculatePowers(float amount) {
        throwPower = Mathf.Clamp(amount * 2, 0.0f, 5.0f);
        //spinPower = Mathf.Lerp(0, 1.0f, throwPower / 5.0f);
    }

    public void Release() {
        released = true;
        StopAllCoroutines();                // Ȯ�� �ʿ�

        Vector3 direction = targetPosition - transform.position;
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);

        targetPosition.z = 0;
        float height = targetPosition.y + targetPosition.magnitude / 2f;    // �ʿ信 ���� �����ϱ�
        height = Mathf.Max(0.01f, height);
        float angle;
        float v0;
        float time;
        CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);

        EnablePhysics();        // ���� ��Ȱ��ȭ

        // ������ �������κ��� ��ô
        Vector3 releasedPosition = transform.position;
        StartCoroutine(Movement(groundDirection.normalized, v0, angle, time, releasedPosition));
    }

    private void Explode() {
        exploded = true;

        GameObject explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        gameObject.SetActive(false);

        // ���� �Ŀ��� ������Ʈ ������ �ʿ�
    }

    // ���� ����
    private float QuadraticEquation(float a, float b, float c, float sign) {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    // ��ǥ ������ ������ �ִ� ���̰��� ���� initialVelocity, angle, time�� ���ϴ� �Լ�
    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time) {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus: tmin;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);
    }

    // ��ǥ ������ ������ ���� initialVelocity�� time�� ���ϴ� �Լ�
    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time) {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    // �̵� �ڷ�ƾ
    private IEnumerator Movement(Vector3 direction, float v0, float angle, float time, Vector3 startPosition) {
        float t = 0;
        // Stage ���̾�� �浹�ϱ� ������ �����̱�
        while(t < time && !collidedWithStage) {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = startPosition + direction * x + Vector3.up * y;

            t += Time.deltaTime;
            yield return null;
        }
    }
}
