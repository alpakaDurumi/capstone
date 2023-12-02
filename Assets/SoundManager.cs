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
    [SerializeField] AudioSource source;


    private void Awake()
    {
        if(SceneManager.GetActiveScene().name.Equals("StartScene")
            || SceneManager.GetActiveScene().name.Equals("EndScene"))
        {
            InitButtonSound();
        }

    }
    private void InitButtonSound()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach(Button button in buttons){
            EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
            if (eventTrigger == null) continue;
            AddEventTrigger(eventTrigger,EventTriggerType.PointerClick, OnButtonClickSound);
            AddEventTrigger(eventTrigger,EventTriggerType.PointerEnter, OnButtonEnterSound);
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
        // 여기에 버튼 클릭 시 수행할 작업을 작성하세요.
        Debug.Log("Button Clicked");
        source.PlayOneShot(btnClickSound);
    }
    private void OnButtonEnterSound(BaseEventData data)
    {
        Debug.Log("Button Entered!!");
        source.PlayOneShot(btnHoverSound);
        // 여기에 버튼 클릭 시 수행할 작업을 작성하세요.
    }
}
