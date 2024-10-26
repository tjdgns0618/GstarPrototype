using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damage_Text;
    [SerializeField] private GameObject cam;

    private void OnEnable()
    {
        Destroy(gameObject, 2f);
    }

    public void Init(float damage, Vector3 pos)
    {
        damage_Text.text = ((int)damage).ToString();
        transform.position = pos;
    }

    private void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}
