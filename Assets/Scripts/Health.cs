using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private StatsObject stats;
    public GameManager.EntityClass entityClass;
    public int health;

    private bool isPlayer = false;
    
    
    void Awake()
    {
        if(stats){
            health = stats.health;
            entityClass = stats.entityClass;
        } 
        else health = 20;
    }

    // Update is called once per frame
   
    void Update(){
        if(Input.GetKeyDown(KeyCode.H)) TakeDamage(5);
    }

    public void TakeDamage(int val){

        health -= val;
        if(health <= 0) KillEntity();
    }

    void KillEntity(){


        if(isPlayer){
            GameManager.Instance.EndGame();
        }

        GameManager.Instance.RemoveEntityFromList(gameObject, entityClass);
        Destroy(gameObject);
    }

    public void Heal(int val){
        health += val;
    }


}
