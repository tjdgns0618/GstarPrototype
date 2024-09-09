using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public GameObject[] stages;
    public TMP_Text UIstage;

    private int currentstageIndex = 0;

    void Start()
    {
        updateStage();
    }

    void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 마을에서 포탈을 탔을 때
        if (collision.gameObject.tag == "portal")
        {
            // StageCount+1
            NextStage();
        }
    }
    // Update is called once per frame
    public void NextStage()
    {
        if(currentstageIndex < stages.Length -1)
        {
            stages[currentstageIndex].SetActive(false);
            currentstageIndex++;
            stages[currentstageIndex].SetActive(false);
            updateStage();
        }
        
    }
    void updateStage()
    {
        UIstage.text = "Stage " + stages[currentstageIndex].name;
    }
}
