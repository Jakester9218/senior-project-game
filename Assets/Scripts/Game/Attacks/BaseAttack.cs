using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName ="Attack")]
public class BaseAttack : ScriptableObject
{
    public GameObject attackPrefab;
    private GameObject activePrefab;
    private GameObject attackOwner;

    public Vector3 attackOffset;
    public Vector3 attackRotation;
    public Vector3 attackScale;

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
    private int previousFacingScale;

    public void StartAttack(GameObject player)
    {
        elapsedDuration = 0;
        activePrefab = Instantiate(attackPrefab, player.transform);
        activePrefab.transform.localPosition = attackOffset;
        activePrefab.transform.localRotation = Quaternion.Euler(attackRotation);
        activePrefab.transform.localScale = attackScale;
        if (player.GetComponent<PlayerController>().facingRight)
        {
            facingScale = 1;
        }
        else
        {
            facingScale = -1;
        }
        activePrefab.transform.localPosition = new Vector3(activePrefab.transform.localPosition.x * facingScale, activePrefab.transform.localPosition.y, activePrefab.transform.localPosition.z);
        activePrefab.transform.localRotation = Quaternion.Euler(new Vector3(activePrefab.transform.localRotation.eulerAngles.x, activePrefab.transform.localRotation.eulerAngles.y*facingScale, activePrefab.transform.localRotation.eulerAngles.z));
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
        else if (elapsedDuration == lastActiveFrame+1)
        {
            //Attack has ended
            Destroy(activePrefab);
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
