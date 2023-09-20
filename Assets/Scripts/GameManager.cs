using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    [SerializeField] int stage;
    private int remainEnemy;

    private void Awake()
    {
        stage = 1;
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void DecreaseRemainEnemy()
    {
        Debug.Log(remainEnemy--);
        // 모든 적을 해치우면 다음 스테이지로 이동
        if(remainEnemy <= 0)
        {
            SceneController.Instance.GoToScene(++stage);
        }
    }
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode load)
    {
        remainEnemy = ScanAllEnemy();
    }


    private int ScanAllEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Debug.Log(enemies.Length);
        return enemies.Length;
    }
    
}
