using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    [SerializeField] private int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}