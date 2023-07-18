using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            EnemyController[] enemiesInScene = FindObjectsOfType<EnemyController>();

            for (int i = 0; i < enemiesInScene.Length; i++)
            {
                Destroy(enemiesInScene[i].gameObject);
            }
            Destroy(gameObject);
        }
    }
}