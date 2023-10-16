using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTarget : MonoBehaviour, IArrowHittable
{
    //public Material otherMaterial = null;

    public float forceAmount = 1.0f;

    public GameObject targetRagdollPrefab;

    public void Hit(RaycastHit hit, Arrow arrow) {
        // hit.point 에서 피격 이펙트 발생
        // ...

        string hitPart = hit.transform.name;    // 맞은 부위

        // 래그돌 생성
        GenerateRagdoll(arrow, hitPart);

        ApplyForce(arrow);
        DisableCollider(arrow);

        // 기존 적 오브젝트 삭제
        Destroy(gameObject);
    }

    private void GenerateRagdoll(Arrow arrow, string hitPart) {
        GameObject ragdoll = Instantiate(targetRagdollPrefab, transform.position, transform.rotation);  // 래그돌 생성

        // 각 transform 위치를 동일하게 수정
        Transform[] ragdollJoints = ragdoll.GetComponentsInChildren<Transform>();
        Transform[] currentJoints = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < ragdollJoints.Length; i++) {
            for (int j = 0; j < currentJoints.Length; j++) {
                if (ragdollJoints[i].name.Equals(currentJoints[j].name)) {
                    ragdollJoints[i].position = currentJoints[j].position;
                    ragdollJoints[i].rotation = currentJoints[j].rotation;
                    break;
                }
            }

            // 맞은 부위에 해당하는 래그돌의 부위에 화살을 고정
            if (ragdollJoints[i].name.Equals(hitPart)) {
                arrow.transform.SetParent(ragdollJoints[i]);
            }
        }
    }

    //private void ApplyMaterial() {
    //    if (TryGetComponent(out MeshRenderer meshRenderer))
    //        meshRenderer.material = otherMaterial;
    //}

    private void ApplyForce(Arrow arrow) {
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.AddForce(arrow.transform.forward * forceAmount);
    }

    private void DisableCollider(Arrow arrow) {
        if (arrow.TryGetComponent(out Collider collider))
            collider.enabled = false;
    }
}
