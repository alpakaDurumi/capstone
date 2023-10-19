using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Enemy[] enemies;
    private Vector3 lastEnemyPos;
    private Quaternion lastEnemyRot;
    private bool isEnemyClear = false;
    // Start is called before the first frame update
    void Start()
    {
        lastEnemyPos = Vector3.zero;
        lastEnemyRot = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (isEnemyClear) return;

        isEnemyClear = true;
        foreach(Enemy enemy in enemies){
            if (enemy.gameObject.activeSelf)
            {
                lastEnemyPos = enemy.transform.position;
                lastEnemyRot = enemy.transform.rotation;
                isEnemyClear = false;
            }
        }

        if (isEnemyClear)
        {
            // 플레이어 방향에서 똑바로 보이도록 반대로 뒤집음
            lastEnemyRot = Quaternion.Inverse(lastEnemyRot);
            UIManager.Instance.CreateNextStageButton(lastEnemyPos, lastEnemyRot);
        }
    }
}
