using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] Image image;

    [SerializeField] Color colorwhite;
    [SerializeField] Color colorBlack;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed;

    public static bool isfinished = false;

    public IEnumerator FadeOut(bool _isWhite, bool _isSlow)
    {
        Color t_color = (_isWhite == true) ? colorwhite : colorBlack;
        t_color.a = 0;

        image.color = t_color;

        while (t_color.a < 1)
        {
            t_color.a += (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_color;
            yield return null;
        }
        isfinished = true;
    }

    public IEnumerator FadeIn(bool _isWhite, bool _isSlow)
    {
        Color t_color = (_isWhite == true) ? colorwhite : colorBlack;
        t_color.a = 1;

        image.color = t_color;

        while (t_color.a >0 )
        {
            t_color.a -= (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_color;
            yield return null;
        }
        isfinished = true;
    }

}
