using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack",menuName ="ScriptableObjects/Attack")]
public class AttackObject : ScriptableObject
{
    public int damage;
    public float lifespan, speed;
    private bool AOE;
}