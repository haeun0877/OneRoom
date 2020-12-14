using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    [SerializeField] float disappearTime;

    private void OnEnable() //Disappear가 붙어있는 객체가 활성화 돼있을 때 마다 실행되는 것
    {
        StartCoroutine(DisapperCoroutine());
    }

    IEnumerator DisapperCoroutine()
    {
        yield return new WaitForSeconds(disappearTime);

        gameObject.SetActive(false);
    }
}
