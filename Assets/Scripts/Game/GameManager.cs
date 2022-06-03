using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public bool canScreenTransition;
    public int ScreenTransitionDelay;

    public GameObject player1JoinText;
    public GameObject player2JoinText;

    // Start is called before the first frame update
    void Start()
    {
        ConstructLevel();
        player1JoinText.SetActive(true);
        player2JoinText.SetActive(true);
        canScreenTransition = true;
        ScreenTransitionDelay = 60;
    }

    public void ConstructLevel()
    {
        Debug.Log("Constructing Level");
        Random.InitState(System.DateTime.Now.Millisecond);
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

        startLevelIndex = Mathf.RoundToInt(activeLevels.Count / 2);
        startLevel = activeLevels[startLevelIndex];
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
        Debug.Log("Screen Transitioned");
            if (activePlayer == player1)
            {
                Debug.Log("ST1");
                leftBoundary += 20;
                rightBoundary += 20;
                camera.transform.position = new Vector3(camera.transform.position.x + 20, camera.transform.position.y, camera.transform.position.z);
                if (activeLevel == activeLevels[activeLevels.Count - 1])
                {
                    Debug.Log("GameEnded");
                    EndGame(player1);
                }
                activeLevel = activeLevels[activeLevels.IndexOf(activeLevel) + 1];
                player1.GetComponent<PlayerController>().Respawn();
                player2.GetComponent<PlayerController>().Respawn();
            }
            if (activePlayer == player2)
            {
                Debug.Log("ST2");
                leftBoundary -= 20;
                rightBoundary -= 20;
                camera.transform.position = new Vector3(camera.transform.position.x - 20, camera.transform.position.y, camera.transform.position.z);
                if (activeLevel == activeLevels[0])
                {
                    Debug.Log("GameEnded");
                    EndGame(player2);
                }
                activeLevel = activeLevels[activeLevels.IndexOf(activeLevel) - 1];
                player1.GetComponent<PlayerController>().Respawn();
                player2.GetComponent<PlayerController>().Respawn();
            }
        
    }

    public void EndGame(GameObject winner)
    {
        SceneManager.LoadScene("MainMenu");
        return;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!player1Joined)
        {
            player1 = playerInput.gameObject;
            player1Joined = true;
            Debug.Log("Player 1 Joined");
            player1JoinText.SetActive(false);

        }
        else
        {
            player2 = playerInput.gameObject;
            Debug.Log("Player 2 Joined");
            player2JoinText.SetActive(false);
        }

    }
}
