using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 20f;
    public float maxDistance = 5f;
    private Vector3 initialPosition;
    private GameObject enemy;
    private bool returning = false;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        enemy = GameObject.FindGameObjectWithTag("enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if(!returning)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            if(Vector3.Distance(initialPosition, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            transform.position += directionToEnemy * speed * Time.deltaTime;

            if(Vector3.Distance(enemy.transform.position, transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
