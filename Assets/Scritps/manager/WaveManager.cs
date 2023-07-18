using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private TextMeshProUGUI waveIndexText;

    private int currentWaveIndex = 0;
    private Wave currentWave;
    private bool gameHasEnded;
    private bool playerDead;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        waveIndexText.gameObject.SetActive(false);
        StartCoroutine(SpawnWavesCoroutine());
    }

    public IEnumerator SpawnWavesCoroutine()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waveIndexText.gameObject.SetActive(true);
            waveIndexText.text = "Wave " + (i + 1).ToString();

            Invoke("DeactivateWaveText", 1.2f);

            currentWave = waves[i];

            if (currentWave == waves[i])
            {
                currentWaveIndex = i;
            }

            for (int j = 0; j < currentWave.numberOfEnemies; j++)
            {
                if (playerDead)
                {
                    yield break;
                }

                GameObject randomEnemy = currentWave.enemies[UnityEngine.Random.Range(0, currentWave.enemies.Length)];
                Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

                GameObject enemySpawn = Instantiate(randomEnemy, randomSpawnPoint.position, randomSpawnPoint.rotation);

                yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        gameHasEnded = true;
    }

    private void DeactivateWaveText()
    {
        waveIndexText.gameObject.SetActive(false);
    }

    public int GetWaveIndex()
    {
        return currentWaveIndex;
    }

    public bool GetGameHasEnded()
    {
        return gameHasEnded;
    }

    public void SetPlayerDead(bool value)
    {
        playerDead = value;
    }
}

[System.Serializable]
public class Wave
{
    public int numberOfEnemies;
    public GameObject[] enemies;
    public float timeBetweenSpawns;
}