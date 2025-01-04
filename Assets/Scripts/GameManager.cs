using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool GameRunning = true;
    public bool playerAlive = true;
    private float nextHeal = 0;
    public float passiveRegenerationSpeed = 2;
    public GameObject player;
    public int parts = 10;

    private bool menuActive = false;
    private bool settingsActive = false;
    
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject overPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private PlayerPanel playerPanel;
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


        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        overPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) OnClickMenu();

        if(playerAlive && Time.time >= nextHeal){
            nextHeal = Time.time + passiveRegenerationSpeed;
            player.GetComponent<Health>().Heal(1);
        }
    }

    private void PrepareGame(){

    }
    public void EndGame(){
        playerAlive = false;
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

        //if enemy chance to spawn parts
        if(targetType == EntityClass.Enemy){
            if(Random.Range(0,3) == 1){
                parts ++;
                playerPanel.SetParts(parts);
            }
        }
    }

    public bool SpawnTurret(Vector3 pos){
        if(menuActive || !playerAlive) return false;

        if(parts <= 0){ //not enough parts

            return false;
        }

        parts--;
        playerPanel.SetParts(parts);
        
        Instantiate(Resources.Load<GameObject>("Prefabs/Turret"), pos, Quaternion.identity);
        return true;
    }

    public void OnClickMenu(){
        if(!playerAlive) return;
        if(menuActive){ //close menu
            menuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            
            menuActive = false;
            settingsActive = false;

            Time.timeScale = 1f;
            
        }
        else{ //open menu
            menuPanel.SetActive(true);
            
            menuActive = true;

            Time.timeScale = 0f;
        }
    }
    public void OnCLickSettings(){
        if(!playerAlive) return;
        if(settingsActive){
            settingsPanel.SetActive(false);
            settingsActive = false;
        }
        else{
            settingsPanel.SetActive(true);
            settingsActive = true;
        }
    }
    public void OnCLickStartingScreen(){
        SceneManager.LoadScene(0);
    }
}
