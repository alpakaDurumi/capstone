using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 손 모델의 각 손가락 본들의 값을 가져오기 위한 스크립트
public class HandData : MonoBehaviour
{
    public enum HandModelType { Left, Right };

    public HandModelType handType;
    public Animator animator;
    public Transform[] fingerBones;

}
