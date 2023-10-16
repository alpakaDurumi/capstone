using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    private FadeScreen fadeScreen;


    public void GoToScene(int sceneIndex)
    {
        fadeScreen = GameObject.Find("Main Camera").transform.Find("FadeScreen").GetComponent<FadeScreen>();
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }
    
    #region 구현 세부사항 
    private IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        // sceneIndex 씬으로 이동
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        float timer = 0;

        operation.allowSceneActivation = false;
        while (timer < fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
    #endregion 
}
