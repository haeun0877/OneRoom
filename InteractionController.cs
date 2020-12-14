using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair; //crosshair의 부모객체
    [SerializeField] GameObject go_Cursor;

    bool isContact = false;
    public static bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;

    DialogueManager theDM;

    public void HideUI()
    {
        go_Crosshair.SetActive(false);
        go_Cursor.SetActive(false);
    }

    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>(); // 게임 오브젝트중 DialogueManger을 가지고 있는 오브젝트들을 찾는다.
    }

    // Update is called once per frame
    void Update()
    {
        CheckObject();
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
            if (!isContact)
            {
                isContact = true;
                go_InteractiveCrosshair.SetActive(true);
                go_NomalCrosshair.SetActive(false);
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
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NomalCrosshair.SetActive(true);
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

    void Interact()
    {
        isInteract = true;

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

        theDM.ShowDialogue();
    }
}
