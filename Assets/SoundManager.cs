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
    [SerializeField] private AudioClip gunFireSound;
    [SerializeField] private AudioClip throwingSound;
    [SerializeField] private AudioClip bowShootSound;
    [SerializeField] private AudioClip swordSliceSound;


    [SerializeField] AudioSource effectAudioSource;
    [SerializeField] AudioSource musicAudioSource;

    private void Awake()
    {
        InitAudioSetting();
    }

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
        InitAudioSetting();
    }

    public void PlayFadeInSound()
    {
        if (musicAudioSource.isPlaying) return;
        if (fadeInSound == null)
        {
            fadeInSound = Resources.Load("Sounds/fadein_sound") as AudioClip;
        }
        effectAudioSource.enabled = false;
        musicAudioSource.pitch = 1.0f;
        musicAudioSource.volume = 0.6f;
        musicAudioSource.PlayOneShot(fadeInSound);
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
        musicAudioSource.pitch = Mathf.Lerp(musicAudioSource.pitch, 0.4f, 0.2f);
    }
    public void RestoreMusicNormalSpeed()
    {
        musicAudioSource.pitch = Mathf.Lerp(musicAudioSource.pitch, 0.99f, 0.2f);
    }

    public void PlayPlayerDeadSound()
    {
        if (musicAudioSource.clip.Equals(deadSound) && musicAudioSource.isPlaying) return;
        effectAudioSource.enabled = false;
        if(deadSound == null)
        {
            deadSound = Resources.Load("Sounds/die_sound") as AudioClip;
        }
        musicAudioSource.Stop();
        musicAudioSource.clip = deadSound;
        musicAudioSource.Play();
        musicAudioSource.pitch = 1.2f;
        musicAudioSource.volume = 0.8f;
    }
    public void PlayFadeOutSound()
    {
        if(fadeOutSound == null)
        {
            fadeOutSound = Resources.Load("Sounds/fadeout_sound") as AudioClip;
        }
        musicAudioSource.Stop();
        musicAudioSource.pitch = 1.3f;
        musicAudioSource.volume = 0.7f;
        musicAudioSource.PlayOneShot(fadeOutSound);
    }
    public void PlayOffBattleMusic()
    {
        effectAudioSource.enabled = true;
        if(battleOffMusic == null)
        {
            battleOffMusic = Resources.Load("Sounds/offBattle_sound") as AudioClip;
        }
        musicAudioSource.clip = battleOffMusic;
        musicAudioSource.pitch = 1f;
        musicAudioSource.volume = 0.1f;
        musicAudioSource.Play();
    }
    public void PlayGunFireSound()
    {
        if (gunFireSound == null)
        {
            gunFireSound = Resources.Load("Sounds/gun_fire_sound") as AudioClip;
        }
        effectAudioSource.PlayOneShot(gunFireSound);
    }
    public void PlayThrowingSound()
    {
        if(throwingSound == null)
        {
            throwingSound = Resources.Load("Sounds/throwing_sound") as AudioClip;
        }
        effectAudioSource.PlayOneShot(throwingSound);
    }
    public void PlayBowShootSound()
    {
        if(bowShootSound == null)
        {
            bowShootSound = Resources.Load("Sounds/bow_shoot_sound") as AudioClip;
        }
        effectAudioSource.PlayOneShot(bowShootSound);
    }
    public void PlaySwordSliceSound()
    {
        if(swordSliceSound == null)
        {
            swordSliceSound = Resources.Load("Sounds/sword_slice_sound") as AudioClip;
        }
        if (effectAudioSource.isPlaying) return;
        effectAudioSource.PlayOneShot(swordSliceSound);
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
        if(btnClickSound == null)
        {
            btnClickSound = Resources.Load("Sounds/btn_click_sound") as AudioClip;
        }
        effectAudioSource.PlayOneShot(btnClickSound);
    }
    private void OnButtonEnterSound(BaseEventData data)
    {
        if (btnHoverSound == null)
        {
            btnHoverSound = Resources.Load("Sounds/btn_hover_sound") as AudioClip;
        }
        effectAudioSource.PlayOneShot(btnHoverSound);
    }
    
    private void PlayOnBattleMusic()
    {
        if (battleMusic == null)
        {
            battleMusic = Resources.Load("Sounds/onBattle_sound") as AudioClip;
        }
        effectAudioSource.enabled = true;
        musicAudioSource.clip = battleMusic;
        musicAudioSource.volume = 0.6f;
        musicAudioSource.pitch = 1.0f;
        musicAudioSource.Play();
    }
    private void InitAudioSetting()
    {
        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }
        if (effectAudioSource == null)
        {
            effectAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (SceneManager.GetActiveScene().name.Equals("StartScene")
            || SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            InitButtonSound();
            Invoke("PlayOffBattleMusic", 3.0f);
        }
        else
        {
            Invoke("PlayOnBattleMusic", 3.0f);
        }

    }
    #endregion
}
