using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : EnemyController
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float timeBetweenShots;
    private float nextShotTime;

    public override void Update()
    {
        base.Update();

        if (Time.time > nextShotTime)
        {
            Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
            nextShotTime = Time.time + timeBetweenShots;
        }
    }
}