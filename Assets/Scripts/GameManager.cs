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
        IsStartRound = false;
        DontDestroyOnLoad(this.gameObject);
        
    }

    public void IncreaseStage()
    {
        Stage++;
    }

    public void StartRound()
    {
        IsStartRound = true;
    }

    public void EndRound()
    {
        IsStartRound = false;
    }       
}
