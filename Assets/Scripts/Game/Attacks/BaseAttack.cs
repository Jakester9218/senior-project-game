using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName ="Attack")]
public class BaseAttack : ScriptableObject
{
    public GameObject attackPrefab;
    public GameObject attackOrigin;

    public float attackScale;
    public int attackDuration;
    private int elapsedDuration;
    private bool isMelee;

}
