using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private SceneTransition sceneTransition;
    private void OnEnable()
    {
        if(sceneTransition == null)
        {
            sceneTransition = gameObject.AddComponent<SceneTransition>();
        }
    }
    public void CreateNextStageButton(Vector3 position,Quaternion rotation)
    {
        Canvas canvas = Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/NextStageButton"), position, rotation);
        canvas.worldCamera = FindObjectOfType<Camera>();

        Button button = canvas.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => sceneTransition.GoToSceneAsync(GameManager.Instance.Stage));
        button.onClick.AddListener(() => button.enabled = false);
        GameManager.Instance.IncreaseStage();
    }
}
