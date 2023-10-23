using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Unity.VisualScripting;

public class UIManager : MonoSingleton<UIManager>
{
    private const float HEIGHT_OFFSET = 1f;
<<<<<<< HEAD
    private const float AXIS_Y_ROTATION_OFFSET = 180;
=======
>>>>>>> 37868fb4bb8cde4629be434f21103fe88a03389f

    private SceneTransition sceneTransition;

    private void OnEnable()
    {
        if(sceneTransition == null)
        {
            sceneTransition = gameObject.AddComponent<SceneTransition>();
        }
    }
    public void CreateNextStageButton(Transform target)
    {
        Canvas button = InstantiateButton();
        SetButtonLocation(button,target);
        AddButtonAction(button);

    }

    private Canvas InstantiateButton()
    {
        Canvas button = Instantiate(Resources.Load<Canvas>("Prefabs/NextStageButton"));
        button.worldCamera = FindObjectOfType<Camera>();
        return button;
    }

    private void SetButtonLocation(Canvas canvas,Transform target)
    {
        Vector3 position = target.position;
        Vector3 rotation = target.rotation.eulerAngles;

        position.y += HEIGHT_OFFSET;
<<<<<<< HEAD
        rotation.y += AXIS_Y_ROTATION_OFFSET;
=======
        rotation.y += 180;
>>>>>>> 37868fb4bb8cde4629be434f21103fe88a03389f

        canvas.gameObject.transform.position = position;
        canvas.gameObject.transform.rotation = Quaternion.Euler(rotation);

    }
    private void AddButtonAction(Canvas canvas)
    {
        Button button = canvas.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => sceneTransition.GoToSceneAsync(GameManager.Instance.Stage));
        button.onClick.AddListener(() => button.enabled = false);
    }
}
