using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
       if (player1)
        {
            player2 = playerInput.gameObject;
        }
       else
        {
            player1 = playerInput.gameObject;
        }

    }
}
