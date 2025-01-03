using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    bool GameRunning = true;
    public GameObject player;
    public enum EntityClass
    {
        Player,
        Enemy
    }
    //sInstance vypujceno z 1zherv projektu
    private static GameManager sInstance;
    public static GameManager Instance{ 
        get { return sInstance; } 
    }
    /// <summary>
    /// List of friendly entities in the game, including the player
    /// </summary>
    private List<GameObject> playerEntities = new List<GameObject>();

    /// <summary>
    /// List of hostile entities in the game
    /// </summary>
    private List<GameObject> enemyEntities = new List<GameObject>();

    void Awake()
    {
        if (sInstance != null && sInstance != this)
        { Destroy(gameObject); }
        else
        { sInstance = this; }

        playerEntities.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EndGame(){
        //TODO
    }

    /// <summary>
    /// Finds the nearest target from a desired group
    /// </summary>
    /// <param name="target"> Enum of the target group.</param>
    ///  /// <param name="target"> Position of the caller object.</param>
 
    public GameObject NearestEntity(EntityClass targetType, Vector3 position){

        float targetDistance = float.MaxValue;
        GameObject closestTarget = null;

        //find correct list
        List<GameObject> targetList = GetEntityList(targetType);

        //find closest entity
        foreach (var entity in targetList){

            float entityDistance = Vector2.Distance(position, entity.transform.position);
            if (entityDistance < targetDistance){
                closestTarget = entity;
                targetDistance = entityDistance;
            }
        }

        return closestTarget;
    }

    private List<GameObject> GetEntityList(EntityClass targetType){
        
        List<GameObject> targetList;
        switch(targetType){
            case EntityClass.Player:
                targetList = playerEntities;
                break;
            case EntityClass.Enemy:
                targetList = enemyEntities;
                break;
            default: 
                return null;
        }

        return targetList;
    }
    public void AddEntityToList(GameObject target, EntityClass targetType){
        
        List<GameObject> targetList = GetEntityList(targetType);
        if(!targetList.Contains(target)){
            targetList.Add(target);
        } 
    }

    public void RemoveEntityFromList(GameObject target, EntityClass targetType){

        List<GameObject> targetList = GetEntityList(targetType);
        if(targetList.Contains(target)){
            targetList.Remove(target);
        } 
    }
}
