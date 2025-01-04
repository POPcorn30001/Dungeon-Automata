using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty",menuName ="ScriptableObjects/Difficulty")]
public class Difficulty : ScriptableObject
{
   public int extraEnemies = 0;
   public float spawnerSpeedup = 1;
}
