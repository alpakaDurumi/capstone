using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoSingleton<SceneTransition>
{
    private FadeScreen fadeScreen;

    private void Awake()
    {
        fadeScreen = FindObjectOfType<FadeScreen>();
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    private IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        // sceneIndex 씬으로 이동
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        float timer = 0;

        operation.allowSceneActivation = false;
        while(timer < fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
    public void TestMethod()
    {
        //Debug.Log("Do test");
    }

}
