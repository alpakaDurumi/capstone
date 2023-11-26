using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    public int Stage { get; private set; }
    public int RemainGroupsOnStage { get; private set; } // 해당 스테이지에 남은 그룹
    public int RemainEnemiesInGroup { get; private set; } // 해당 그룹에 남은 적의 수 
    public bool IsStartRound { get; private set; }


    private void Awake()
    {
        Stage = 0;
        IsStartRound = false;
        DontDestroyOnLoad(this.gameObject);
    }
    public void ResetGame()
    {
        Stage = 0;
        RemainEnemiesInGroup = 0;
        RemainGroupsOnStage = 0;

    }
    public void ResetStageInfo()
    {
        RemainEnemiesInGroup = 0;
        RemainGroupsOnStage = 0;
    }
    public void IncreaseStage()
    {
        Stage++;
    }

    public void StartRound()
    {
        IsStartRound = true;
    }

    public void PlayerDie()
    {
        Transform screenParent = FindObjectOfType<Camera>().transform;
        FadeScreen screen = screenParent.Find("Fader Screen").GetComponent<FadeScreen>();
        screen.SetColor(Color.red);
        SceneTransition.Instance.GoToSceneAsync(Stage);
    }

    public void EndRound()
    {
        IsStartRound = false;
    }
    public void UpdateStageInfo(Stage.Group[] groups)
    {
        // 초기화인 경우 (스테이지 처음 들어갔을 경우)
        if(RemainGroupsOnStage <= 0)
        {   // 전체 그룹의 수를 받아
            RemainGroupsOnStage = groups.Length;
        }
        else
        {
            // 그렇지 않은 경우 남은 그룹의 수를 감소시키고
            // 다음 그룹의 수로 갱신 
            int next= groups.Length - RemainGroupsOnStage;
            RemainEnemiesInGroup = groups[next].enemies.Length;
            RemainGroupsOnStage--;
        }
    }

    // die()에 이 함수를 호출하면 됨
    public void DecreaseEnemyCountOnStage()
    {
        RemainEnemiesInGroup--;
    }
}
