using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;


public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button returnButton;
    private const float HEIGHT_OFFSET = 1f;
    private const float AXIS_Y_ROTATION_OFFSET = 180;

    private SceneTransition sceneTransition;
    private void OnEnable()
    {
        if(sceneTransition == null)
        {
            sceneTransition = gameObject.AddComponent<SceneTransition>();
        }

        if (SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            PrintTotalPlayTimeText();
            returnButton = GameObject.Find("ReplayButton").GetComponent<Button>();
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

        // 중복 누름 방지 
        startButton.enabled = false;
        
    }
    public void OnClickHowToPlayButton()
    {
        videoPanel.SetActive(true);
    }

    public void OnClickCloseButton()
    {
        videoPanel.SetActive(false);
    }

    public void OnClickReturnButton()
    {
        returnButton.enabled = false;
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
    private void PrintTotalPlayTimeText()
    {
        string time = GameManager.Instance.GetTotalPlayTime();
        Nova.TextBlock text = GameObject.Find("ScoreText").GetComponent<Nova.TextBlock>();
        text.Text = time;
    }
    #endregion
}
