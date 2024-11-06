using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVscroll : MonoBehaviour
{
    // Scroll main texture based on time
    public int materialId = 0;
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;
    Renderer rend;
    GameManager gm;

    void Start()
    {
        rend = GetComponent<Renderer>();
        gm = GameManager.instance;
    }

    void Update()
    {
        //GetComponent<LineRenderer>().materials[0].
        

        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        rend.materials[materialId].SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));

        //rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }

    private void OnParticleCollision(GameObject other)
    {
        //EnemyAI eAI = other.GetComponent<EnemyAI>();
        //Debug.Log(other.name);
        //if (eAI != null)
        //{
        //    eAI.Damage(gm._damage * ItemDataBase.instance.Variable2(34));
        //}

        if (other.gameObject.CompareTag("Enemy"))
        {
            IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
            if (damageAble != null)
            {
                damageAble.Damage(gm._damage * (ItemDataBase.instance.Variable(34) + ItemDataBase.instance.Variable2(34) * (gm.FindItemCount(34) - 1)));
            }
        }
    }

}
