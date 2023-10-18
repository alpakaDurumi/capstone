using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    public int Stage { get; private set; }

    public bool IsStartRound { get; private set; }


    private void Awake()
    {
        Stage = 0;
        IsStartRound = true;
        DontDestroyOnLoad(this.gameObject);
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
   
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode load)
    {

    }

    public void IncreaseStage()
    {
        Stage++;
    }
}
