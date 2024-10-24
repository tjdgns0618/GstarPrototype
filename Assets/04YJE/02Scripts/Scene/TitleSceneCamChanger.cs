using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneCamChanger : MonoBehaviour
{
    public GameObject[] cams;
    public CinemachineBrain mainCam;

    private GameObject currentCam;

    public void SwitchToCamera(GameObject cam)
    {
        for(int i = 0; i < cams.Length; i++)
        {
            if (cams[i] == cam)
            {
                cam.SetActive(true);
                currentCam = cam;
            }
            else
                cams[i].SetActive(false);
        }
    }

    public void TransitionToCamera(Animator charAnim)
    {
        StartCoroutine(TransitionToIsPicked(charAnim));
    }

    IEnumerator TransitionToIsPicked(Animator charAnim)
    {
        if(charAnim != null) charAnim.ResetTrigger("PickOther");

        while (mainCam.IsBlending)
        {
            yield return null; 
        }

        if(charAnim != null) charAnim.SetTrigger("IsPicked");

    }

    public void TransitionToIdle(Animator charAnim)
    {
        if (charAnim != null)
        {
            charAnim.ResetTrigger("IsPicked");
            charAnim.SetTrigger("PickOther");
        }
    }

    public ICinemachineCamera GetCamera()
    {
        return currentCam.GetComponent<ICinemachineCamera>();
    }
}
