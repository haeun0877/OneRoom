using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform tf_Crosshair;

    [SerializeField] Transform tf_Cam;
    [SerializeField] Vector2 camBoundary; // 캠의 영역

    [SerializeField] float sightMoveSpeed; // 좌우 움직임 스피드
    [SerializeField] float sightSensivitity; // 고개의 움직임 속도
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    [SerializeField] GameObject go_NotCamDown;
    [SerializeField] GameObject go_NotCamUp;
    [SerializeField] GameObject go_NotCamLeft;
    [SerializeField] GameObject go_NotCamRight;

    float originPosY;

    public void Reset()
    {
        currentAngleX = 0;
        currentAngleY = 0;
    }

    void Start()
    {
        originPosY = tf_Cam.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InteractionController.isInteract)
        {
            CrosshairMoving();
            ViewMoving();
            KeyViewMoving();
            CameraLimit();
            NotCamUI();
        }
    }

    void NotCamUI()
    {
        go_NotCamDown.SetActive(false);
        go_NotCamUp.SetActive(false);
        go_NotCamLeft.SetActive(false);
        go_NotCamRight.SetActive(false);

        if (currentAngleY >= lookLimitX)
            go_NotCamRight.SetActive(true);
        else if (currentAngleY <= -lookLimitX)
            go_NotCamLeft.SetActive(true);

        if (currentAngleX <= -lookLimitY)
            go_NotCamUp.SetActive(true);
        if (currentAngleX >= lookLimitY)
            go_NotCamDown.SetActive(true);

    }

    void CameraLimit()
    {
        if(tf_Cam.localPosition.x >= camBoundary.x)
        {
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        else if(tf_Cam.localPosition.x <= -camBoundary.x)
        {
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Cam.localPosition.y >= camBoundary.y + originPosY)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, camBoundary.y +originPosY, tf_Cam.localPosition.z);
        }
        else if (tf_Cam.localPosition.y <= -camBoundary.y + originPosY)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, -camBoundary.y + originPosY, tf_Cam.localPosition.z);
        }
    }

    void KeyViewMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0) // 방향키 오른쪽은 1, 왼쪽은 -1, 0일때는 아무것도 눌리지 않았을 때
        {
            currentAngleY += sightSensivitity * Input.GetAxis("Horizontal");
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxis("Horizontal"), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (Input.GetAxisRaw("Vertical") != 0) // 방향키 위쪽은 1, 아래쪽은 -1, 0일때는 아무것도 눌리지 않았을 때
        {
            currentAngleX += sightSensivitity * -Input.GetAxis("Vertical");
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x , tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxis("Vertical"), tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    void ViewMoving()
    {
        if (tf_Crosshair.localPosition.x > (Screen.width / 2 - 50) || tf_Crosshair.localPosition.x < (-Screen.width / 2 + 50))
        {
            currentAngleY += (tf_Crosshair.localPosition.x > 0) ? sightSensivitity : -sightSensivitity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 50) || tf_Crosshair.localPosition.y < (-Screen.height / 2 + 50))
        {
            currentAngleX += (tf_Crosshair.localPosition.y > 0) ? -sightSensivitity : sightSensivitity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
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
