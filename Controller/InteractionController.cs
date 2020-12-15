using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair; //crosshair의 부모객체
    [SerializeField] GameObject go_Cursor;
    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] Text targetName;

    bool isContact = false;
    public static bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;

    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;

    DialogueManager theDM;

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
        go_TargetNameBar.SetActive(false);
    }

    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>(); // 게임 오브젝트중 DialogueManger을 가지고 있는 오브젝트들을 찾는다.
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteract)
        {
            CheckObject();
        }
    }

    void CheckObject()
    {
        Vector3 t_MousPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Physics.Raycast(cam.ScreenPointToRay(t_MousPos), out hitInfo, 100))
        {
            Contact();
            ClickLeftButton();
        }
        else
        {
            NotContact();
        }
    }

    void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            go_TargetNameBar.SetActive(true);
            targetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (!isContact)
            {
                isContact = true;
                go_InteractiveCrosshair.SetActive(true);
                go_NomalCrosshair.SetActive(false);
                StopCoroutine("Interaction");
                StopCoroutine("InteractionEffect");
                StartCoroutine("Interaction", true);
                StartCoroutine("InteractionEffect");
            }
        }
        else
        {
            NotContact();
        }
    }

    void NotContact()
    {
        if (isContact)
        {
            go_TargetNameBar.SetActive(false);
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NomalCrosshair.SetActive(true);
            StopCoroutine("Interaction");
            StartCoroutine("Interaction", false);
        }
    }

    void ClickLeftButton()
    {
        if (!isInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }
    }

    IEnumerator Interaction(bool p_Appear)
    {
        Color color = img_Interaction.color;
        if (p_Appear)
        {
            color.a = 0;
            while (color.a < 1)
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
        else
        {
            while (color.a > 0)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
    }

    IEnumerator InteractionEffect()
    {
        while(isContact && !isInteract)
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f; //반투명

            img_InteractionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 t_scale = img_InteractionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.01f;
                img_InteractionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_scale;
                yield return null; // 한프레임 대기
            }
            yield return null;
        }
    }


    void Interact()
    {
        isInteract = true;

        StopCoroutine("Interaction");
        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QuestionEffect>().SetTarget(t_targetPos);
        ps_QuestionEffect.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=>QuestionEffect.isCollide); //특정 조건을 만족할때까지 대기
        QuestionEffect.isCollide = false;

        theDM.ShowDialogue(hitInfo.transform.GetComponent<InteractionEvent>().GetDialogues());
    }
}
