using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Difficulty difficulty;
    bool gameRunning = true;
    public bool playerAlive = true;
    private float nextHeal = 0;
    public float passiveRegenerationSpeed = 2;
    public GameObject player;
    public int parts = 10;
    public int enemiesLeft =26;

   
    //UI
    [SerializeField] private TMP_Text enemiesLeftText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject overPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject settingsPanel;
    public PlayerPanel playerPanel;
     [SerializeField] private Canvas worldCanvas;
    private bool menuActive = false;
    private bool settingsActive = false;


    //Entity list
    public enum EntityClass
    {
        Player,
        Enemy,
        RobotBox
    }
    /// <summary>
    /// List of friendly entities in the game, including the player
    /// </summary>
    private List<GameObject> playerEntities = new List<GameObject>();

    /// <summary>
    /// List of hostile entities in the game
    /// </summary>
    private List<GameObject> enemyEntities = new List<GameObject>();
     /// <summary>
    /// List of robot boxes in the game
    /// </summary>
    private List<GameObject> robotBoxEntities = new List<GameObject>();


    //sInstance vypujceno z 1zherv projektu
    private static GameManager sInstance;
    public static GameManager Instance{ 
        get { return sInstance; } 
    }
    

    //Audio
    public AudioSource audioSource;

    void Awake()
    {
        if (sInstance != null && sInstance != this)
        { Destroy(gameObject); }
        else
        { sInstance = this; }

        audioSource = gameObject.GetComponent<AudioSource>();
        PrepareGame();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) OnClickMenu();

        if(playerAlive && Time.time >= nextHeal){
            nextHeal = Time.time + passiveRegenerationSpeed;
            player.GetComponent<Health>().Heal(1);
        }

        if(playerAlive && enemiesLeft <= 0){
            WinGame();
            
        }

        FollowPlayer();
    }

    private void PrepareGame(){
        //UI
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        overPanel.SetActive(false);
        winPanel.SetActive(false);



        enemiesLeft = 26 + 5*difficulty.extraEnemies;
        UpdateEnemies(enemiesLeft);
        gameRunning = true;
    }
    public void EndGame(){
        
        playerAlive = false;
        gameRunning = false;
        overPanel.SetActive(true);

        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    public void WinGame(){
        winPanel.SetActive(true);

        overPanel.SetActive(true);
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gameRunning = false;
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
            case EntityClass.RobotBox:
                targetList = robotBoxEntities;
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
            enemiesLeft--;
            UpdateEnemies(enemiesLeft);
            if(Random.Range(0,5) == 1){
                parts ++;
                playerPanel.SetParts(parts);
            }
        }
    }

    public bool SpawnRobot(Vector3 pos){
        if(menuActive || !gameRunning) return false;

        if(parts <= 0){ //not enough parts

            return false;
        }

        parts--;
        playerPanel.SetParts(parts);
        
        Instantiate(Resources.Load<GameObject>("Prefabs/RobotBox"), pos, Quaternion.identity);
        return true;
    }

    public void OnClickMenu(){
        if(!gameRunning) return;
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
        if(!gameRunning) return;
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
        Time.timeScale = 1f;
        RestartDifficulty();
        SceneManager.LoadScene(0);
    }
    public void OnCLickRestart(){
        Time.timeScale = 1f;
        RestartDifficulty();
        SceneManager.LoadScene(1);
    }
    public void OnCLickNextLevel(){
        Time.timeScale = 1f;
        IncreaseDifficutly();
        SceneManager.LoadScene(1);
    }
    public Canvas GetWorldCanvas(){
        return worldCanvas;
    }

    public void PlayButtonSound(){
        audioSource.Play();
    }

    private void RestartDifficulty(){
        difficulty.extraEnemies = 0;
        difficulty.spawnerSpeedup = 1;
    }

    private void IncreaseDifficutly(){
        difficulty.extraEnemies += 2;
        difficulty.spawnerSpeedup *=2;
    }

    private void UpdateEnemies(int count){
        enemiesLeftText.text = "Enemies left: "+count;
    }

    private void FollowPlayer(){ //becouse of button play sound
        gameObject.transform.position = player.transform.position;
    }
}
