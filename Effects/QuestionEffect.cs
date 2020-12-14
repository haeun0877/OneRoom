using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Vector3 targetPos = new Vector3();
    [SerializeField] ParticleSystem ps_Effect;

    public static bool isCollide = false; //다른 스크립트에서 언제든지 사용할 수 있도록

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPos != Vector3.zero) //목표물의 위치값을 알아냈을 경우
        {
            if ((transform.position - targetPos).sqrMagnitude >= 0.1f) // sqrManitude : 두 거리간의 거리차의 제곱값
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed); // Lerp : 목표물과의 거리차를 n분의 1씩 좁혀나가는 방식
            }
            else
            {
                ps_Effect.gameObject.SetActive(true);
                ps_Effect.transform.position = transform.position;
                ps_Effect.Play();
                isCollide = true;
                targetPos = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }
}
