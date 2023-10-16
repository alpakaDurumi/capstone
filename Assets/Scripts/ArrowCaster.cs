using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArrowCaster : MonoBehaviour
{
    [SerializeField] private Transform tip;
    // �浹�� ���̾ �����ϸ� ~������ ���� ������ ���̾� �̿��� ���̾�� �����ϰ� �ȴ�
    [SerializeField] private LayerMask layerMask = ~0;

    private Vector3 lastPosition = Vector3.zero;

    // raycast�� ���� tip(ȭ����)�� ���� �浹�ߴ��� Ȯ��
    public bool CheckForCollision(out RaycastHit hit) {
        if(lastPosition == Vector3.zero) {
            lastPosition = tip.position;
        }
        // �浹 ���θ� Ȯ���ϸ� lastPosition�� ����ؼ� update
        bool collided = Physics.Linecast(lastPosition, tip.position, out hit, layerMask);
        lastPosition = collided ? lastPosition : tip.position;

        return collided;
    }
}
