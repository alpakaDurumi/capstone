using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoSingleton<SceneTransition>
{
    public void GoToSceneAsync(int sceneIndex)
    {
        // 해당 씬의 재실행일때 (죽을때) 
        if(SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            SoundManager.Instance.PlayPlayerDeadSound();
        }
        // 다음 스테이지일 때 
        else
        {
            SoundManager.Instance.PlayFadeOutSound();
        }

        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    private IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        GameManager.Instance.EndRound();
        FadeScreen fadeScreen = FindFadeScreen();
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

    private FadeScreen FindFadeScreen()
    {
        Transform screenParent = FindObjectOfType<Camera>().transform;
        return screenParent.Find("Fader Screen").GetComponent<FadeScreen>();
    }
}
