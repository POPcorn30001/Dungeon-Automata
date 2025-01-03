using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats",menuName ="ScriptableObjects/Stats")]
public class StatsObject : ScriptableObject
{
    public int health, dmg;
    public float speed, range, attackSpeed, attackTime;
    public bool ranged;
    public GameManager.EntityClass entityClass;
}
