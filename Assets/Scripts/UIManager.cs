using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.UI;
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
        SetNextStageButtonLocation(button,target);
        AddNextStageButtonAction(button);

    }
    public void OnClickStartButton()
    {
        GameManager.Instance.IncreaseStage();
        sceneTransition.GoToSceneAsync(GameManager.Instance.Stage);

        // 누른 스타트 오브젝트를 찾아서 button을 비활성화
        // 중복 누름 방지 
        EventSystem.current.currentSelectedGameObject
            .GetComponent<Button>().enabled = false;
        
    }
    public void OnClickHowToPlayButton()
    {
        GameObject canvas = GameObject.Find("Canvas");
        canvas.transform.Find("Video Panel").gameObject.SetActive(true);
    }

    public void OnClickCloseButton()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        clickedButton.GetComponentInParent<Nova.UIBlock2D>().gameObject.SetActive(false);
    }
    public void OnClickReturnButton()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().enabled = false;
        GameManager.Instance.ResetGame();
        sceneTransition.GoToSceneAsync(GameManager.Instance.Stage);
    }
#region 구현 세부사항
    private Canvas InstantiateButton()
    {
        Canvas button = Instantiate(Resources.Load<Canvas>("Prefabs/NextStageButton"));
        button.worldCamera = FindObjectOfType<Camera>();
        return button;
    }

    private void SetNextStageButtonLocation(Canvas canvas,Transform target)
    {
        Vector3 position = target.position;
        Vector3 rotation = target.rotation.eulerAngles;

        position.y += HEIGHT_OFFSET;
        rotation.y += AXIS_Y_ROTATION_OFFSET;

        canvas.gameObject.transform.position = position;
        canvas.gameObject.transform.rotation = Quaternion.Euler(rotation);

    }
    private void AddNextStageButtonAction(Canvas canvas)
    {
        Button button = canvas.GetComponentInChildren<Button>();
        GameManager.Instance.IncreaseStage();
        button.onClick.AddListener(() => sceneTransition.GoToSceneAsync(GameManager.Instance.Stage));
        button.onClick.AddListener(() => button.enabled = false);
    }
    #endregion
}
