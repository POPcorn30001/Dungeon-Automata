using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private StatsObject stats;
    [SerializeField] private PlayerPanel playerPanel; 
    [SerializeField] private GameObject target; //what enemies are agroing
    public GameManager.EntityClass entityClass;
    public int health;
    private int maxHealth;

    public bool isPlayer = false;
    [SerializeField] private AudioSource audioSource;
    
    
    void Awake()
    {
        if(stats){
            health = stats.health;
            entityClass = stats.entityClass;
        } 
        else health = 20;

        maxHealth = health;

        GameManager.Instance.AddEntityToList(target, entityClass);
    }

    // Update is called once per frame
   
    void Update(){
        if(Input.GetKeyDown(KeyCode.H)) TakeDamage(5);
    }

    public void TakeDamage(int val){
        if(isPlayer && audioSource) audioSource.Play();
        health -= val;
        if(health <= 0){
            health = 0;
            KillEntity();
        } 
        if(isPlayer && playerPanel) playerPanel.SetHealth(health);
    }

    void KillEntity(){

        GameManager.Instance.RemoveEntityFromList(target, entityClass);
        
        if(isPlayer){
            if(audioSource) audioSource.Play();
            GameManager.Instance.EndGame();
            Destroy(gameObject, audioSource.clip.length);
        }
        else{
            Destroy(gameObject);
        }
        
    }

    public void Heal(int val){
        health += val;
        if(health > maxHealth) health = maxHealth;

        if(isPlayer) playerPanel.SetHealth(health);
    }


}
