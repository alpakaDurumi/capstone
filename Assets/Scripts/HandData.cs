using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ���� �� �հ��� ������ ���� �������� ���� ��ũ��Ʈ
public class HandData : MonoBehaviour
{
    public enum HandModelType { Left, Right };

    public HandModelType handType;
    public Animator animator;
    public Transform[] fingerBones;

}
