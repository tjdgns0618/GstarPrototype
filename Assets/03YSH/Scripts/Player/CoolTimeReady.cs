using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoolTimeReady : MonoBehaviour
{
    public DOTweenAnimation DOTween;
    public void OnComplete()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.0f);
        DOTween.DOPlayBackwards();
    }
}
