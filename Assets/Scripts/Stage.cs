using System.Collections.Generic;
using UnityEngine;
using System;

public class Stage : MonoBehaviour
{
    [Serializable]
    public class Group
    {
        public Enemy[] enemies;
    }

    public Group[] groups;
    private int groupIdx;
    private Transform lastEnemy;

    private void Start()
    {
        GameManager.Instance.EndRound();
        GameManager.Instance.UpdateStageInfo(groups);
        groupIdx = 0;
     
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsStartRound) return;

        // 마지막 그룹일때의 마지막 적은 버튼의 위치 지정을 위해서 있어야함
        if(GameManager.Instance.RemainGroupsOnStage <= 0
            && GameManager.Instance.RemainEnemiesInGroup == 1)
        {
            UpdateLastEnemyPos();
            return;
        }

        // 현재 그룹에서 소환된 적들이 남아있을 때
        if (GameManager.Instance.RemainEnemiesInGroup > 0) return;

        // 남은 그룹이 있고, 소환된 적이 0인 경우
        if (GameManager.Instance.RemainGroupsOnStage > 0)
        {
            SpawnEnemies();
            return;
        }
        // 남은 그룹도 없고 , 소환된 적도 없는 경우 
        UIManager.Instance.CreateNextStageButton(lastEnemy);
        GameManager.Instance.EndRound();
    }

    private void UpdateLastEnemyPos()
    {
        int lastGroup = (groupIdx <= 0) ? 0 : groupIdx - 1;
        Enemy[] enemies = groups[lastGroup].enemies;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                lastEnemy = enemy.transform;
            }
        }
    }

    public Enemy[] SpawnEnemies()
    {
        GameManager.Instance.UpdateStageInfo(groups);
        Enemy[] enemies = groups[groupIdx++].enemies;

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        return enemies;
    }
}

