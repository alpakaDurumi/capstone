using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    public static int Stage { get; private set; }

    public static bool IsStartRound { get; private set; }

    public static int remains { get; private set; }

    private void Awake()
    {
        Stage = 1;
        IsStartRound = true;
        DontDestroyOnLoad(this.gameObject);
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
   
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode load)
    {
        InvokeRepeating("ScanAllEnemy", 1f, 1.5f);
    }


    private void ScanAllEnemy()
    {
        if (!IsStartRound) return;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        remains = enemies.Length;
        Debug.Log(remains);
        if (remains <= 0)
        {
            SceneController.Instance.GoToScene(Stage++);
            IsStartRound = false;
            CancelInvoke();
        }
    }
    
}
