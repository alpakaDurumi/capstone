using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public void CreateNextStageButton(Vector3 position,Quaternion rotation)
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/NextStageButton");
        Button button = obj.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => new SceneTransition().GoToSceneAsync(GameManager.Stage));
        button.onClick.AddListener(() => Destroy(button.gameObject));
        Instantiate(obj, position, rotation);
    }
}
