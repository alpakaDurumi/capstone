using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeDuration = 3;
    public Color fadeColor;
    private Renderer rend;

    private void Start()
    {
        gameObject.SetActive(true);
        rend = GetComponent<Renderer>();
        fadeDuration = 3f;
        FadeIn();
    }
    // 페이드 인이 끝나고 해당 오브젝트가 비활성화되면 IsStartRound = true 상태로
    private void OnDisable()
    {
        GameManager.Instance.StartRound();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        Fade(0, 1);
    }

    public void SetColor(Color color)
    {
        fadeColor = color;
        fadeDuration = 0.5f;
    }
    #region 기능 세부사항
    
    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;

        while(timer < fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        if(alphaIn > alphaOut)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
