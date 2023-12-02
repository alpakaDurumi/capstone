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
        if (SceneManager.GetActiveScene().name.Equals("StartScene")
            || SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            InitButtonSound();
        }
        musicAudioSource.clip = battleMusic;
        musicAudioSource.Play();
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
        musicAudioSource.pitch = 0.35f;
    }
    public void RestoreMusicNormalSpeed()
    {
        musicAudioSource.pitch = 1f;
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
    #endregion
}
