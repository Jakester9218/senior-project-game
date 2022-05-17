using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public int startingHealth;
    public int health;
    public int assignedPlayer;
    public GameObject gameManager;
    public GameObject healthBarManager;
    private HealthBarManager hbm;
    private Rigidbody rb;

    private void Awake()
    {
        
        
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        health = startingHealth;
        gameManager = GameObject.Find("GameManager");
        healthBarManager = GameObject.Find("HealthBarManager");
        hbm = healthBarManager.GetComponent<HealthBarManager>();
        if (gameManager.GetComponent<GameManager>().player1)
        {
            assignedPlayer = 2;
        }
        else
        {
            assignedPlayer = 1;
        }
    }

    public void AddHealth(InputAction.CallbackContext context)
    {
        health += 1;
    }

    public void RemoveHealth(InputAction.CallbackContext context)
    {
        health -= 1;
    }

    private void Update()
    {
        if (assignedPlayer == 1)
        {
            hbm.player1Health = health;
        }
        else if (assignedPlayer == 2)
        {
            hbm.player2Health = health;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
