using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private GameManager.EntityClass entityClass = GameManager.EntityClass.Player;
    private Rigidbody2D rb;
    
    [SerializeField] private AttackObject stats;
    public float speed = 1;
    private float lifetimeEnd = 1;
    public int damage = 0;
    private bool enemyHit = false;
    private bool AOE = false;
    public Vector2 dir = Vector2.zero;


    void Awake(){
        if(stats){
            speed = stats.speed;
            damage = stats.damage;
            lifetimeEnd = Time.time + stats.lifespan;
        }
        else{
            lifetimeEnd = Time.time;
            Debug.Log("Missing attack stats");
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.SetActive(false); 
       
    }

    /// <summary>
    /// Makes the attack into a projectile
    /// </summary>
    public void Setup(Vector2 dir, GameManager.EntityClass eClass){
        this.dir = dir;
        entityClass = eClass;
        gameObject.SetActive(true); 
    }


    void Update(){
        if(Time.time > lifetimeEnd) Destroy(gameObject);
        rb.velocity = dir * speed;
    }
   
    private void OnTriggerEnter2D(Collider2D collider){
        

        //ignore other projectiles
        Attack attack = collider.GetComponent<Attack>();
        if(attack) return;

        //hit entity
        Health health = collider.GetComponent<Health>();
        if(health){
            
            if(health.entityClass == this.entityClass){ //pass through friendly objects
                return;
            }

            if(!enemyHit) health.TakeDamage(damage);
            enemyHit = true;
        }

        //hit obstacle

        if(!AOE) Destroy(gameObject);
    }
}
