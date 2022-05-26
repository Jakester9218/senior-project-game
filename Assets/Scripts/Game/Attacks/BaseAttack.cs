using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName ="Attack")]
public class BaseAttack : ScriptableObject
{
    public GameObject attackPrefab;
    private GameObject attackOwner;

    public int attackDuration;
    public bool CubeOrSphere;
    public int firstActiveFrame;
    public int lastActiveFrame;
    public Vector3 boxDimensions;
    public Vector3 relativeBoxPosition;

    private Bounds box;
    
    public int elapsedDuration;
    private LayerMask playerLayer = 7;
    public bool isMelee;
    private List<GameObject> hitObjects;
    public bool canHitMultipleTimes;
    public int hitCooldown;
    private int facingScale;

    public void StartAttack(GameObject player)
    {
        elapsedDuration = 0;
    }

    public void WhileActive(GameObject player)
    {
        elapsedDuration += 1;
        if (elapsedDuration == firstActiveFrame)
        {
            if (player.GetComponent<PlayerController>().facingRight)
            {
                facingScale = 1;
            }
            else
            {
                facingScale = -1;
            }
            box.center = new Vector3(player.transform.position.x + relativeBoxPosition.x*facingScale, player.transform.position.y + relativeBoxPosition.y, player.transform.position.z + relativeBoxPosition.z);
            box.size = boxDimensions;
            foreach (var hit in inHitbox(playerLayer, player))
            {
                OnHit(hit.gameObject);
            }
        }
        else if (elapsedDuration > firstActiveFrame && elapsedDuration < lastActiveFrame)
        {
            if (player.GetComponent<PlayerController>().facingRight)
            {
                facingScale = 1;
            }
            else
            {
                facingScale = -1;
            }
            box.center = new Vector3(player.transform.position.x + relativeBoxPosition.x*facingScale, player.transform.position.y + relativeBoxPosition.y, player.transform.position.z + relativeBoxPosition.z);
            foreach (var hit in inHitbox(playerLayer, player))
            {
                if (!hitObjects.Contains(hit.gameObject))
                {
                    OnHit(hit.gameObject);
                }
            }
        }
        else if (elapsedDuration > lastActiveFrame)
        {
            //Attack has ended
        }
       
    }

    public void OnHit(GameObject target)
    {
        hitObjects.Add(target);
        target.GetComponent<PlayerCombat>().RemoveHealth();
    }

    public List<GameObject> inHitbox(LayerMask layerMask, GameObject player)
    {
        List<GameObject> output = new List<GameObject>();

        var hits = Physics.OverlapBox(box.center, new Vector3(box.size.x / 2, box.size.y / 2, box.size.z / 2));
        Debug.DrawLine(new Vector3(box.center.x - box.size.x / 2, box.center.y - box.size.y / 2), new Vector3(box.center.x - box.size.x / 2, box.center.y + box.size.y / 2));
        Debug.DrawLine(new Vector3(box.center.x + box.size.x / 2, box.center.y - box.size.y / 2), new Vector3(box.center.x + box.size.x / 2, box.center.y + box.size.y / 2));
        Debug.DrawLine(new Vector3(box.center.x - box.size.x / 2, box.center.y + box.size.y / 2), new Vector3(box.center.x + box.size.x / 2, box.center.y + box.size.y / 2));
        Debug.DrawLine(new Vector3(box.center.x - box.size.x / 2, box.center.y - box.size.y / 2), new Vector3(box.center.x + box.size.x / 2, box.center.y - box.size.y / 2));

        foreach (var hit in hits)
        {
            if (hit.gameObject.layer == layerMask && hit.gameObject != player)
            {
                output.Add(hit.gameObject);
            }
        }

        return output;
    }
         
    

    public void EndAttack(GameObject player)
    {
        //Cleanup
    }



}
