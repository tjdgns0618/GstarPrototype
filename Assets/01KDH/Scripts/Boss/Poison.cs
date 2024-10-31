using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private Vector3 _endpos;
    private Vector3 _setpos;

    private void OnEnable()
    {
        _endpos = PlayerCharacter.Instance.transform.position;
        _setpos = new Vector3(_endpos.x, 0.5f, _endpos.z);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _setpos, Time.deltaTime * 12f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            GameObject particle = GameManager.instance.particlePoolManager.GetParticle("Fog");
            if (particle != null)
            {
                particle.transform.position = transform.position;
            }
            gameObject.SetActive(false);
        }
        if(col.gameObject.CompareTag("Player"))
        {
            IDamageAble<float> damageAble = col.GetComponent<IDamageAble<float>>();
            damageAble.Damage(GameManager.instance._maxhp * 0.02f);
            gameObject.SetActive(false);
        }
    }
}
