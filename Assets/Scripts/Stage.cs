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
    private Group currentGroup;
    private Transform lastEnemy;
    private readonly Queue<Group> groupQueue = new();

    private void Start()
    {
        GameManager.Instance.EndRound();
        foreach (Group group in groups)
        {
            groupQueue.Enqueue(group);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsStartRound) return;
        
        // 현재 그룹에서 소환된 적들이 남아있을 때
        if(currentGroup != null && GetRemainEnemies().Length > 0)
        {
            Enemy[] enemies = GetRemainEnemies();
            lastEnemy = enemies[enemies.Length - 1].transform;
         
        }
        // 현재 그룹에서 소환된 적들이 다 죽었을 때
        else
        {
            if (TryGetNextEnemyGroup()) SpawnEnemies();
            else
            {
                UIManager.Instance.CreateNextStageButton(lastEnemy);
                GameManager.Instance.EndRound();
            }
        }
    }

    private Enemy[] GetRemainEnemies()
    {
        Enemy[] enemies = currentGroup.enemies;
        List<Enemy> remain = new();

        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                remain.Add(enemy);
            }
        }

        return remain.ToArray();
    }

    // 다음 EnemyGroup이 없을 시 false 을 반환한다. 
    public bool TryGetNextEnemyGroup()
    {
        if (groupQueue.Count <= 0) return false;

        currentGroup = groupQueue.Dequeue();
        return true;
    }

    public Enemy[] SpawnEnemies()
    {
        Enemy[] enemies = currentGroup.enemies;

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        return enemies;
    }
}

