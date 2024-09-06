using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject villageCam;
    public GameObject fieldCam;
    
    public void ShowVillageView()
    {
        villageCam.SetActive(true);
        fieldCam.SetActive(false);
    }

    public void ShowFieldView()
    {
        fieldCam.SetActive(true);
        villageCam.SetActive(false);
    }
}
