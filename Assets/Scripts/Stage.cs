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
    private void Awake()
    {
        GameManager.Instance.EndRound();
        GameManager.Instance.ResetStageInfo();
    }
    private void Start()
    {
        groupIdx = 0;
        GameManager.Instance.UpdateStageInfo(groups);
    }

    private void FixedUpdate()
    {
        // 게임이 시작되지 않았거나 
        // 현재 그룹에서 소환된 적들이 남아있을 때
        if (!GameManager.Instance.IsStartRound
            || GameManager.Instance.RemainEnemiesInGroup > 0)
        {
            return;
        }

        // 남은 그룹이 있고, 소환된 적이 0인 경우
        if (GameManager.Instance.RemainGroupsOnStage > 0)
        {
            SpawnEnemies();
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

