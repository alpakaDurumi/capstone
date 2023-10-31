using UnityEngine;
using Button = UnityEngine.UI.Button;

public class UIManager : MonoSingleton<UIManager>
{
    private const float HEIGHT_OFFSET = 1f;
    private const float AXIS_Y_ROTATION_OFFSET = 180;

    private SceneTransition sceneTransition;

    private void OnEnable()
    {
        if(sceneTransition == null)
        {
            sceneTransition = gameObject.AddComponent<SceneTransition>();
        }
    }
    public void CreateNextStageButton(Transform target) {
        if (target == null) return;
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
        rotation.y += AXIS_Y_ROTATION_OFFSET;

        canvas.gameObject.transform.position = position;
        canvas.gameObject.transform.rotation = Quaternion.Euler(rotation);

    }
    private void AddButtonAction(Canvas canvas)
    {
        Button button = canvas.GetComponentInChildren<Button>();
        GameManager.Instance.IncreaseStage();
        button.onClick.AddListener(() => sceneTransition.GoToSceneAsync(GameManager.Instance.Stage));
        button.onClick.AddListener(() => button.enabled = false);
    }
}
