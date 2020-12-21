using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    ObjectFront,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
}


[System.Serializable] //custom클래스는 인스펙터 창에서 쓰이기 위해서는 반드시 이 문장이 있어야함
public class Dialogue 
{
    [Header("카메라가 타겟팅할 대상")]
    public CameraType cameraType;
    public Transform tf_Target;

    [HideInInspector]
    public string name;

    [HideInInspector]
    public string[] contexts;

    [HideInInspector]
    public string[] spriteName;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}