using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class SlowMotion : MonoBehaviour
{
    /*
     * 해당 스크립트를 VR 손 모델이나 자체에 넣으면 됨
     * VR 키 입력을 받아야하는데, 아무리해도 안받아져서 
     * 일단은 공백으로 비어두고 주석처리했음
     * 
     * 입력조건만 넣으면 정상적으로 작동함
     * 
     */
   
    private const float SMOOTH_OFFSET = 0.02f;

    
    private float slowTime = 0.1f;
    private float normalTime = 1f;
    private bool slowMode = false;


    // Update is called once per frame
    void Update()
    {
        //if(/*키 입력을 받는 함수*/){
        //    if (slowMode)
        //    {
        //        Time.timeScale = normalTime;
        //        Time.fixedDeltaTime = SMOOTH_OFFSET * Time.deltaTime;
        //        slowMode = false;

        //    }
        //}
        //else
        //{
        //    Time.timeScale = slowTime;
        //    Time.fixedDeltaTime = SMOOTH_OFFSET * Time.deltaTime;
        //    slowMode = true;
        //}
    
    }
}
