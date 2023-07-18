using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyController>())
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}