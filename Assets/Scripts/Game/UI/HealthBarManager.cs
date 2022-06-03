using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{

    public GameObject player1HealthBar;
    public GameObject player2HealthBar;

    public GameObject healthBarItem;

    public int player1Health = 3;
    private int player1LastHealth;
    public int player2Health = 3;
    private int player2LastHealth;

    public List<GameObject> player1HealthItems;
    public List<GameObject> player2HealthItems;

    // Start is called before the first frame update
    void Start()
    {
        player1LastHealth = player1Health;
        player2LastHealth = player2Health;
        for (int i = 0; i < player1Health; i++)
        {
            player1HealthItems.Add(addHealthItem(player1HealthBar, Color.blue));
        }
        for (int i = 0; i < player2Health; i++)
        {
            player2HealthItems.Add(addHealthItem(player2HealthBar, Color.red));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        if (player1Health > player1LastHealth)
        {
            while (player1HealthItems.Count < player1Health)
            {
                player1HealthItems.Add(addHealthItem(player1HealthBar, Color.blue));
            }
        }
        else if (player1Health < player1LastHealth)
        {
            while (player1HealthItems.Count > player1Health)
            {
                var len = player1HealthItems.Count - 1;
                Destroy(player1HealthItems[len]);
                player1HealthItems.RemoveAt(len);
            }
        }

        if (player2Health > player2LastHealth)
        {
            while (player2HealthItems.Count < player2Health)
            {
                player2HealthItems.Add(addHealthItem(player2HealthBar, Color.red));
            }
        }
        else if (player2Health < player2LastHealth)
        {
            while (player2HealthItems.Count > player2Health)
            {
                var len = player2HealthItems.Count - 1;
                Destroy(player2HealthItems[len]);
                player2HealthItems.RemoveAt(len);
            }
        }
        player1LastHealth = player1Health;
        player2LastHealth = player2Health;
    }

    GameObject addHealthItem(GameObject parent, Color color)
    {
        GameObject GA = Instantiate(healthBarItem, parent.transform);
        GA.GetComponent<Image>().color = color;
        return GA;
    }


    
}
