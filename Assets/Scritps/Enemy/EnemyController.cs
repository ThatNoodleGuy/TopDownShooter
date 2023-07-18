using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkingRange = 10f;
    [SerializeField] private bool shouldWalkAround = false;
    [SerializeField] private bool shouldTargetPlayer = false;

    [Header("Health Variables")]
    [SerializeField] private int health;

    [Header("Attack Variables")]
    [SerializeField] private int damageAmount;

    [Header("Drops")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float powerUpDropChance;
    private Vector3 powerUpDropOffset = new Vector3(0, 1f, 0);

    private NavMeshAgent agent;
    private GameObject target;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerController.instance.gameObject;

        agent.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
    }

    public virtual void Update()
    {
        if ((PlayerController.instance.GetPlayerHealth() > 0) && shouldTargetPlayer)
        {
            agent.destination = target.transform.position;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (shouldWalkAround)
                {
                    SetRandomDestination();
                }
            }
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;

            float random = UnityEngine.Random.Range(0, 100f);
            if (random < powerUpDropChance)
            {
                GameObject randomPowerUp = powerUps[UnityEngine.Random.Range(0, powerUps.Length)];
                Instantiate(randomPowerUp, transform.position + powerUpDropOffset, transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    public virtual void SetRandomDestination()
    {
        // Generate a random point within the walking range
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * walkingRange;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomPoint, out hit, walkingRange, NavMesh.AllAreas);

        // Set the destination for the agent
        agent.SetDestination(hit.position);
    }

    public virtual int GetHealth()
    {
        return health;
    }

    public virtual int GetDamageAmount()
    {
        return damageAmount;
    }

    public virtual void SetTargetToNull()
    {
        target = null;
    }

    public virtual void SetNavMeshAgentTarget(Vector3 newTarget)
    {
        agent.destination = newTarget;
    }
}