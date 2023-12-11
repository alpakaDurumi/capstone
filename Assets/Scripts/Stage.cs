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
    [SerializeField] private Transform targetForNav;
    
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

        List<int> cand = new List<int>();

        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].gameObject.SetActive(true);
            enemies[i].target = targetForNav;       // 모든 enemy들에 대하여 target 설정

            if (enemies[i] is Enemy_rifle) {
                cand.Add(i);
            }
        }

        // 웨이브에 원거리 적이 하나 이상 있는 경우에만
        if(cand.Count > 0) {
            // Enemy_rifle들 중 하나를 랜덤으로 선택하여 수류탄을 소지하게 하기
            int selectedIdx = cand[UnityEngine.Random.Range(0, cand.Count)];
            Enemy_rifle selected = enemies[selectedIdx] as Enemy_rifle;
            selected.HaveGrenade = true;
        }
        return enemies;
    }
}

