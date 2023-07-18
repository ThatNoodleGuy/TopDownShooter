using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNecromencer : EnemyController
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    [SerializeField] private float timeBetweenSpawns;
    private float nextSpawnTime;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform spawnPoint;

    private Vector3 targetPosition;

    public override void Start()
    {
        base.Start();
        targetPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), transform.position.y, UnityEngine.Random.Range(minZ, maxZ));
    }

    public override void Update()
    {
        SetNavMeshAgentTarget(targetPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            if (Time.time > nextSpawnTime)
            {
                GameObject randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
                Instantiate(randomEnemy, spawnPoint.position, spawnPoint.rotation);
                nextSpawnTime = Time.time + timeBetweenSpawns;
            }
        }
    }
}