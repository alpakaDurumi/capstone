using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArrowCaster : MonoBehaviour
{
    [SerializeField] private Transform tip;
    // 충돌할 레이어를 지정하면 ~연산을 통해 지정한 레이어 이외의 레이어는 무시하게 된다
    [SerializeField] private LayerMask layerMask = ~0;

    private Vector3 lastPosition = Vector3.zero;

    // raycast를 통해 tip(화살촉)에 무언가 충돌했는지 확인
    public bool CheckForCollision(out RaycastHit hit) {
        if(lastPosition == Vector3.zero) {
            lastPosition = tip.position;
        }
        // 충돌 여부를 확인하며 lastPosition을 계속해서 update
        bool collided = Physics.Linecast(lastPosition, tip.position, out hit, layerMask);
        lastPosition = collided ? lastPosition : tip.position;

        return collided;
    }
}
