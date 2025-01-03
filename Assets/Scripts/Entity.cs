using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public GameManager.EntityClass entityClass = GameManager.EntityClass.Player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private GameObject currentTarget;
    [SerializeField] private StatsObject stats;
    public float range = 1;
    public float speed= 1;


    [SerializeField] private GameObject attackPf;
    [SerializeField] private float attackOffset;
    [SerializeField] private Vector2 attackOffsetPoint;
    private bool attacking = false;
    private float attackEnd =0;
    private float nextAttackTime;
    // Start is called before the first frame update
    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        speed = stats.speed;
        range = stats.range;
        nextAttackTime = Time.time;

        GameManager.Instance.AddEntityToList(gameObject, entityClass);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > attackEnd) attacking = false;
        if(attacking) return;
        switch (entityClass){
            case GameManager.EntityClass.Player:
                sAlly();
                break;
            case GameManager.EntityClass.Enemy:
                sEnemy();
                break;
            default:
                return;
        } 
        
    }
    /// <summary>
    /// Logic statemachine for enemies entities
    /// </summary>
    private void sEnemy(){

        //find target
        currentTarget = GameManager.Instance.NearestEntity(GameManager.EntityClass.Player, gameObject.transform.position);
        if(currentTarget == null){ //no player entities left, stop moving
            this.enabled = false;
        }
        

        //approach target to range distance
        Vector3 targetDir = (currentTarget.transform.position - gameObject.transform.position).normalized;
        float targetDistance = Vector2.Distance(gameObject.transform.position, currentTarget.transform.position);

        if(targetDistance > range){

            rb.velocity = targetDir * speed;
        }
        else
        { rb.velocity = Vector3.zero; }

        //sort layer
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);


        //atack target
        if(Time.time >= nextAttackTime) Attack(targetDistance, targetDir, GameManager.EntityClass.Enemy);

        
        
    }

    /// <summary>
    /// Logic statemachine for player entities
    /// </summary>
    private void sAlly(){

        //find target
        currentTarget = GameManager.Instance.NearestEntity(GameManager.EntityClass.Enemy, gameObject.transform.position);
        if(!currentTarget){
            return;
        }

        Vector3 targetDir = (currentTarget.transform.position - gameObject.transform.position).normalized;
        float targetDistance = Vector2.Distance(gameObject.transform.position, currentTarget.transform.position);

        if(targetDistance > range){

            rb.velocity = targetDir * speed;
        }
        else
        { rb.velocity = Vector3.zero; }

        //sort layer
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        //atack target
        if(Time.time >= nextAttackTime){
            Attack(targetDistance, targetDir, GameManager.EntityClass.Player);
            Debug.Log("turret shooting, target dir: "+targetDir);
        } 

        
    }
    private void Attack(float targetDistance, Vector3 targetDir, GameManager.EntityClass sender){

        if(targetDistance <= range){

            attacking = true;
            attackEnd = Time.time + stats.attackTime;
            nextAttackTime = Time.time + stats.attackSpeed;
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.x += targetDir.x * attackOffset+ attackOffsetPoint.x;
            spawnPos.y += targetDir.y * attackOffset + attackOffsetPoint.y;
            
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject attack = Instantiate(attackPf, spawnPos, rotation);
            attack.GetComponent<Attack>().Setup(targetDir, sender);
            
        }
    }

}

