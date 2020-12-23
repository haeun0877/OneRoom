using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    Dialogue[] dialogues;

    bool isDIalogue = false;
    bool isNext = false; //특정 키입력이 들어올때까지 대기

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay; // 텍스트가 타자치는거처럼 올라가는효과

    int lineCount = 0; //대화카운트
    int contextCount = 0; //대사카운트(봉란이 한명이 여러번을 말할 수 있음)

    InteractionController theIC;
    CameraController theCam;
    SpriteManager theSpriteManager;
    SplashManager theSplashManager;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        theCam = FindObjectOfType<CameraController>();
        theSpriteManager = FindObjectOfType<SpriteManager>();
        theSplashManager = FindObjectOfType<SplashManager>();
    }

    private void Update()
    {
        if (isDIalogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    if (++contextCount < dialogues[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if (++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDIalogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.SettingUI(false);
        dialogues = p_dialogues;
        theCam.CamOriginSetting();
        StartCoroutine(CameraTargettingType());
    }

    IEnumerator CameraTargettingType()
    {
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.FadeIn: SettingUI(false); SplashManager.isfinished = false;  StartCoroutine(theSplashManager.FadeIn(false, true)); yield return new WaitUntil(() => SplashManager.isfinished); break;
            case CameraType.FadeOut: SettingUI(false); SplashManager.isfinished = false; StartCoroutine(theSplashManager.FadeOut(false, true)); yield return new WaitUntil(() => SplashManager.isfinished); break;
            case CameraType.FlashIn: SettingUI(false); SplashManager.isfinished = false; StartCoroutine(theSplashManager.FadeIn(true, true)); yield return new WaitUntil(() => SplashManager.isfinished); break;
            case CameraType.FlashOut: SettingUI(false); SplashManager.isfinished = false; StartCoroutine(theSplashManager.FadeOut(true, true)); yield return new WaitUntil(() => SplashManager.isfinished); break;
            case CameraType.ObjectFront: theCam.CameraTargetting(dialogues[lineCount].tf_Target); break;
            case CameraType.Reset:theCam.CameraTargetting(null, 0.05f, true, false); break;
        }
        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        isDIalogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);

        SettingUI(false);
    }

    void ChaneSprite()
    {
        if (dialogues[lineCount].spriteName[contextCount] != "")
        {
            StartCoroutine(theSpriteManager.SpriteChangeSoroutine(dialogues[lineCount].tf_Target, dialogues[lineCount].spriteName[contextCount].Trim()));
        }
    }

    void PlaySound()
    {
        if (dialogues[lineCount].voiceName[contextCount] != "")
        {
            SoundManager.instance.PlaySound(dialogues[lineCount].voiceName[contextCount], 2);

        }
    }

    IEnumerator TypeWriter() // 텍스트 출력 코루틴
    {
        SettingUI(true);
        ChaneSprite();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ","); // 특정 문자열을바꿈 ( '를 ,로 바꿈)
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n");

        bool t_white = false;
        bool t_yellow = false;
        bool t_cyan = false;
        bool t_ignore = false;

        for (int i= 0; i<t_ReplaceText.Length; i++)
        {
            switch (t_ReplaceText[i])
            {
                case 'ⓦ': t_white = true;  t_yellow = false; t_cyan = false; t_ignore = true;break;
                case 'ⓨ': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case 'ⓒ': t_white = false; t_yellow = false; t_cyan = true; t_ignore = true; break;
                case '①': SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '②': SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString();

            if (!t_ignore)
            {
                if (t_white) { t_letter = "<color=#ffffff>" + t_letter + "</color>"; }
                else if (t_yellow) { t_letter = "<color=#F6ED00>" + t_letter + "</color>"; }
                else if (t_cyan) { t_letter = "<color=#919191>" + t_letter + "</color>"; }
                txt_Dialogue.text += t_letter;
            }
            t_ignore = false;

            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
        yield return null;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);

        if (p_flag)
        {
            if(dialogues[lineCount].name == "")
            {
                go_DialogueNameBar.SetActive(false);
            }
            else
            {
                go_DialogueNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name;
            }
        }

    }
}
