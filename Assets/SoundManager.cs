using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioClip btnClickSound;
    [SerializeField] private AudioClip btnHoverSound;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private AudioClip fadeOutSound;
    [SerializeField] private AudioClip fadeInSound;
    [SerializeField] private AudioClip battleOffMusic;
    [SerializeField] private AudioClip deadSound;

    [SerializeField] AudioSource effectAudioSource;
    [SerializeField] AudioSource musicAudioSource;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayFadeInSound();
        Invoke("PlayOnBattleMusic", 3.0f);
        if (SceneManager.GetActiveScene().name.Equals("StartScene")
            || SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            InitButtonSound();
        }
    }

    public void AddButtonSound(Button button)
    {
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
        if (eventTrigger == null) return;
        AddEventTrigger(eventTrigger, EventTriggerType.PointerClick, OnButtonClickSound);
        AddEventTrigger(eventTrigger, EventTriggerType.PointerEnter, OnButtonEnterSound);
    }
    public void ReduceMusicSpeed()
    {
        if (battleMusic == null) return;
        musicAudioSource.pitch = Mathf.Lerp(musicAudioSource.pitch, 0.4f, 0.2f);
    }
    public void RestoreMusicNormalSpeed()
    {
        if (battleMusic == null) return;
        musicAudioSource.pitch = Mathf.Lerp(musicAudioSource.pitch, 0.99f, 0.2f);
    }

    public void PlayPlayerDeadSound()
    {
        if (deadSound == null || musicAudioSource.clip.Equals(deadSound)) return;

        musicAudioSource.Stop();
        musicAudioSource.clip = deadSound;
        musicAudioSource.Play();
        musicAudioSource.pitch = 1.2f;
        musicAudioSource.volume = 0.8f;
    }
    public void PlayFadeOutSound()
    {
        musicAudioSource.Stop();
        musicAudioSource.pitch = 1.3f;
        musicAudioSource.volume = 0.7f;
        musicAudioSource.PlayOneShot(fadeOutSound);
    }

    #region 세부 구현 사항
    private void InitButtonSound()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach(Button button in buttons){
            AddButtonSound(button);
        }
    }

    private void AddEventTrigger(
        EventTrigger eventTrigger,
        EventTriggerType eventType,
        UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        eventTrigger.triggers.Add(entry);
    }

    private void OnButtonClickSound(BaseEventData data)
    {
        effectAudioSource.PlayOneShot(btnClickSound);
    }
    private void OnButtonEnterSound(BaseEventData data)
    {
        
        effectAudioSource.PlayOneShot(btnHoverSound);
    }
    private void PlayFadeInSound()
    {
        musicAudioSource.pitch = 1.0f;
        musicAudioSource.volume = 0.7f;
        musicAudioSource.PlayOneShot(fadeInSound);
    }
    private void PlayOnBattleMusic()
    {
        musicAudioSource.clip = battleMusic;
        musicAudioSource.volume = 0.1f;
        musicAudioSource.pitch = 1.0f;
        musicAudioSource.Play();
    }
    #endregion
}
