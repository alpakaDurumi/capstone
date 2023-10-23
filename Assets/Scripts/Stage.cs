using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Enemy[] enemies;
    private Transform lastEnemy;
    private bool isEnemyClear = false;

    private void FixedUpdate()
    {
        if (isEnemyClear) return;

        isEnemyClear = true;
        foreach(Enemy enemy in enemies){
            if (enemy.gameObject.activeSelf)
            {
                lastEnemy = enemy.transform;
                isEnemyClear = false;
            }
        }

        if (isEnemyClear)
        {
            UIManager.Instance.CreateNextStageButton(lastEnemy);
        }
    }
}
