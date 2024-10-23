using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneCamChanger : MonoBehaviour
{
    public GameObject[] cams;
    public Animator[] animator;

    public void SwitchToCamera(GameObject cam)
    {
        for(int i = 0; i < cams.Length; i++)
        {
            if (cams[i] == cam)
                cam.SetActive(true);
            else
                cams[i].SetActive(false);
        }
    }

    public void SwitchToAction(GameObject character)
    {
        for(int i = 0; i < animator.Length; i++)
        {
            if (character == animator[i].gameObject)
                character.GetComponent<Animator>().SetTrigger("AttackAction");
        }
    }
}
