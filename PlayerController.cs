using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform tf_Crosshair;


    // Update is called once per frame
    void Update()
    {
        CrosshairMoving();
    }

    void CrosshairMoving()
    {
        //localPosition은 부모와의 상대적인 위치를 나타냄 , Position은 절대적인 좌표값
        //그냥 mousePosition을 했을 때는 마우스와 실제 Crosshair가 화면의 반만큼 차이가 나서 그 값을 빼줌
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x-(Screen.width/2), Input.mousePosition.y-(Screen.height/2));

        //Crosshair가 화면 밖으로 나가지않도록 하기위해
        float t_cursorPosX = tf_Crosshair.localPosition.x;
        float t_cursorPosY = tf_Crosshair.localPosition.y;

        t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2 +30), (Screen.width / 2 - 30));
        t_cursorPosY= Mathf.Clamp(t_cursorPosY, (-Screen.height / 2 + 30), (Screen.height / 2 - 30));

        tf_Crosshair.localPosition = new Vector2(t_cursorPosX, t_cursorPosY);
    }
}
