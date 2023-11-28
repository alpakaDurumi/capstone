using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float PlayTime { get; private set; }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsStartRound) return;

        PlayTime += Time.fixedDeltaTime;
    }
    public void ResetTimer()
    {
        PlayTime = 0f;
    }
    public string GetFormattedPlayTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(PlayTime);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return formattedTime;
    }
}
