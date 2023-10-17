using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public void CreateNextStageButton(Vector3 position,Quaternion rotation)
    {
        
       
        Canvas canvas = Instantiate<Canvas>(Resources.Load<Canvas>("Prefabs/NextStageButton"), position, rotation);
        canvas.worldCamera = FindObjectOfType<Camera>();

        Button button = canvas.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => Destroy(canvas.gameObject));
        button.onClick.AddListener(() => new SceneTransition().GoToSceneAsync(GameManager.Instance.Stage));
        GameManager.Instance.IncreaseStage();
    }
}
