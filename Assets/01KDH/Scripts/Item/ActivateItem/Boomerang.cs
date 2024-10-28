using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boomerang : MonoBehaviour
{
    Vector3 _startPos;
    Vector3 _moveMent;

    private void OnEnable()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward + _moveMent*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision coll)
    {
        Vector3 _hitPos = coll.contacts[0].point;

        // 벽에 충돌시 튕기는용 입 사각, 반사각
        Vector3 incomingVec = _hitPos - _startPos;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, coll.contacts[0].normal);

        _moveMent = reflectVec.normalized;
        _startPos = transform.position;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyAI eAI = col.GetComponent<EnemyAI>();
            if (eAI != null)
            {
                eAI.Damage(10f);
            }
        }
        if(col.gameObject.CompareTag("Wall"))
        {
            Vector3 _hitPos = col.transform.position;

            // 벽에 충돌시 튕기는용 입 사각, 반사각
            Vector3 incomingVec = _hitPos - _startPos;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, _hitPos);

            _moveMent = reflectVec.normalized;
            _startPos = transform.position;
            Debug.Log("asdfasdfasdfasdfasdfasdfasdf");
        }
    }
}
