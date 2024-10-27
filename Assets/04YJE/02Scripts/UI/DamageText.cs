using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageText : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    private TMP_Text damage_Text;
    private Transform _transform;
    private Animator animator;

    private void Start()
    {
        _transform = transform;
        animator = GetComponent<Animator>();
        damage_Text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
    }

    public void Init(float damage, Vector3 pos, bool isCritical)
    {
        damage_Text.text = ((int)damage).ToString();
        _transform.position = pos;
        animator.SetBool("IsCritical", isCritical);
    }

    private void Update()
    {
        _transform.LookAt(cam.transform.position);
    }
}
