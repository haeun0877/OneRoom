using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void CameraTargetting(Transform p_Target, float p_CamSpeed = 0.05f)
    {
        if (p_Target != null)
        {
            StopAllCoroutines();
            StartCoroutine(CameraTargettingCoroutine(p_Target, p_CamSpeed));
        }
    }

    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_CamsSpeed = 0.05f)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_TargetFrontPos = t_TargetPos + p_Target.forward;
        Vector3 t_Direction = (t_TargetPos - t_TargetFrontPos).normalized; // 항상값을 방향만 표시할 수 있도록 최소화해줌
    
        //거리값이 가까워지거나 각도차가 거의 없어질때까지 반복
        while(transform.position != t_TargetPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction))>=0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, t_TargetFrontPos, p_CamsSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_CamsSpeed);
            yield return null;
        }
    }
}
