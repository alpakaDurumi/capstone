using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] int stage;

    private void Awake()
    {
        sceneTransition = gameObject.AddComponent<SceneTransition>();
        stage = 0;
    }

    public void MoveTargetStage(int target)
    {
        sceneTransition.GoToSceneAsync(target);
    }
}
