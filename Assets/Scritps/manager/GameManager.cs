using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject loosePanel = null;
    [SerializeField] private GameObject winPanel = null;
    [SerializeField] private GameObject playerHealthPanel = null;
    [SerializeField] private TextMeshProUGUI waveSurvivedText = null;

    public static PlayerInput playerInput;

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

        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            PlayerController.instance.enabled = false;
        }
    }

    private void Update()
    {
        if (PlayerController.instance.GetPlayerDead())
        {
            waveSurvivedText.text = ("You survived " + WaveManager.instance.GetWaveIndex() + " waves").ToString();

            StopCoroutine(WaveManager.instance.SpawnWavesCoroutine());

            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemies.Length; i++)
            {
                WaveManager.instance.SetPlayerDead(true);
                Destroy(enemies[i].gameObject);
            }
            loosePanel.SetActive(true);
            winPanel.SetActive(false);
        }

        if (WaveManager.instance.GetGameHasEnded() && WaveManager.instance.enabled)
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            if (enemies.Length <= 0)
            {
                playerHealthPanel.SetActive(false);
                loosePanel.SetActive(false);
                winPanel.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        Input.backButtonLeavesApp = true;
    }
}