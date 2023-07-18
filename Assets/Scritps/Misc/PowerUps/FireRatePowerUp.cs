using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePowerUp : MonoBehaviour
{
    [SerializeField] private float newTimeBetweenShots;
    [SerializeField] float powerUpTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController.instance.IncreaseFireRate(newTimeBetweenShots, powerUpTime);
            Destroy(gameObject);
        }
    }
}