using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeScreen : MonoBehaviour
{
    public float fadeDuration = 3;
    public Color fadeColor;
    private Renderer rend;

    public WeaponChanger weaponChanger;

    private void Start()
    {
        gameObject.SetActive(true);
        rend = GetComponent<Renderer>();
        fadeDuration = 3f;
        FadeIn();
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
        StartCoroutine(GameStartAfterFade(alphaIn,alphaOut));
    }
    // FadeRoutine 코루틴 함수가 끝나면 IsStartRound = true 상태로
    private IEnumerator GameStartAfterFade(float alphaIn, float alphaOut)
    {
        yield return StartCoroutine(FadeRoutine(alphaIn, alphaOut));

        if(!SceneManager.GetActiveScene().name.Equals("StartScene")
            && !SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            weaponChanger.ChangeToRandomWeapon();
            GameManager.Instance.StartRound();
        }
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
    }
    #endregion
}
