using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Variables")]
    [SerializeField] private float moveSpeed;

    [Header("Shooting Variables")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float timeBetweenShots;
    private float nexShotTime;
    private float defaultTimeBetweenShots;

    [Header("Stats")]
    [SerializeField] private int playerHealth;
    private int maxHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthBar;
    private bool playerDead;

    [HideInInspector][SerializeField] Vector2 lastMovedVector;

    private Rigidbody bodyRB;
    private Vector2 moveInput;
    private Camera mainCamera;
    private Animator animator;

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
        bodyRB = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();

        lastMovedVector = new Vector2(-1f, 0f);

        defaultTimeBetweenShots = timeBetweenShots;

        maxHealth = playerHealth;
        healthText.text = (playerHealth + "/" + maxHealth).ToString();
        healthBar.maxValue = maxHealth;
    }

    private void Update()
    {
        if (!playerDead)
        {
            PlayerInput();
            Shoot();
            UpdateHealthUI();
        }
    }

    private void PlayerInput()
    {
        moveInput = GameManager.playerInput.Player.Move.ReadValue<Vector2>();
        animator.SetBool("isRunning", false);

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            lastMovedVector = moveInput.normalized;
            animator.SetBool("isRunning", true);
        }

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        movement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * movement;
        bodyRB.MovePosition(bodyRB.position + movement * moveSpeed * Time.deltaTime);

        Vector2 mousePosition = GameManager.playerInput.Player.MousePosition.ReadValue<Vector2>();
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void Shoot()
    {
        if (Time.time > nexShotTime)
        {
            if (GameManager.playerInput.Player.Shoot.triggered)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
                nexShotTime = Time.time + timeBetweenShots;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;

            playerDead = true;
            
            healthBar.gameObject.SetActive(false);
            foreach (var item in gameObject.GetComponentsInChildren<Renderer>())
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyController>())
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            TakeDamage(enemy.GetDamageAmount());

            Destroy(enemy.gameObject);
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = (playerHealth + "/" + maxHealth).ToString();
        healthBar.value = playerHealth;
    }

    public void HealPlayer(int amount)
    {
        if (playerHealth < maxHealth)
        {
            playerHealth += amount;
        }
        UpdateHealthUI();
    }

    public void IncreaseFireRate(float newRateOfFire, float powerUpTime)
    {
        StartCoroutine(StartIncreaseFireRate(newRateOfFire, powerUpTime));
    }

    private IEnumerator StartIncreaseFireRate(float newRateOfFire, float powerUpTime)
    {
        timeBetweenShots = newRateOfFire;
        yield return new WaitForSeconds(powerUpTime);
        timeBetweenShots = defaultTimeBetweenShots;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public bool GetPlayerDead()
    {
        return playerDead;
    }
}