using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{

    public GameObject player1 = null;
    public GameObject player2 = null;
    private bool player1Joined = false;

    public List<GameObject> levels;
    public List<GameObject> unusedLevels;
    public List<GameObject> activeLevels;
    public GameObject activeLevel;
    public GameObject startLevel;
    public int levelCount;
    public int startLevelIndex;
    public Camera camera;
    public GameObject leftSpawn;
    public GameObject rightSpawn;
    public int leftBoundary;
    public int rightBoundary;

    // Start is called before the first frame update
    void Start()
    {
        ConstructLevel();
    }

    public void ConstructLevel()
    {
        unusedLevels = new List<GameObject>();
        activeLevels = new List<GameObject>();
        foreach (var item in levels)
        {
            unusedLevels.Add(item);
        }
        for (int i = 0; i < 2*levelCount+1; i++)
        {
            var lev = unusedLevels[Random.Range(0, unusedLevels.Count - 1)];
            unusedLevels.Remove(lev);
            activeLevels.Add(lev);
        }

        startLevelIndex = Mathf.RoundToInt(activeLevels.Count / 2)-1;
        startLevel = levels[startLevelIndex];
        activeLevel = startLevel;
        leftBoundary = -10;
        rightBoundary = 10;

        foreach (var item in activeLevels)
        {
            Instantiate(item, new Vector3(20 * (activeLevels.IndexOf(item) - startLevelIndex), 0, 0), new Quaternion(0, 0, 0, 0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player1Joined)
        {
            if (player1.transform.position.x > rightBoundary)
            {
                ScreenTransition(player1);
            }
            else if (player2.transform.position.x < leftBoundary)
            {
                ScreenTransition(player2);
            }
        }
    }


    public void ScreenTransition(GameObject activePlayer)
    {
        if (activePlayer == player1)
        {
            leftBoundary += 20;
            rightBoundary += 20;
            camera.transform.position = new Vector3(camera.transform.position.x + 20, camera.transform.position.y, camera.transform.position.z);
            activeLevel = activeLevels[activeLevels.IndexOf(activeLevel)+1];
            player1.GetComponent<PlayerController>().Respawn();
            player2.GetComponent<PlayerController>().Respawn();
        }
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!player1Joined)
        {
            player1 = playerInput.gameObject;
            player1Joined = true;
            Debug.Log("Player 1 Joined");
        }
        else
        {
            player2 = playerInput.gameObject;
            Debug.Log("Player 2 Joined");
        }

    }
}
