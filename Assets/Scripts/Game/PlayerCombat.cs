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
    public List<BaseAttack> availableAttacks;
    //0: Main Weapon
    //1: Special


    public bool ableToAttack;

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
        ableToAttack = true;
        if (gameObject == gameManager.GetComponent<GameManager>().player1)
        {
            assignedPlayer = 1;
        }
        else
        {
            assignedPlayer = 2;
        }


    }

    public void OnWeaponAttackInput(InputAction.CallbackContext context)
    {

        if (ableToAttack && context.action.triggered)
        {
            ableToAttack = false;
            StartCoroutine(PerformAttack(availableAttacks[0]));
        }
    }

    IEnumerator PerformAttack(BaseAttack attack)
    {
        attack.StartAttack(gameObject);
        while (attack.elapsedDuration < attack.lastActiveFrame)
        {
            attack.WhileActive(gameObject);
            yield return null;
        }
        ableToAttack = true;
    }

    public void AddHealth()
    {
        health += 1;
    }

    public void RemoveHealth()
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
