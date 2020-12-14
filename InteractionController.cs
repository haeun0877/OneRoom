using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NomalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;

    bool isContact = false;
    bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (isContact)
            {
                Interact();
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
    }
}
