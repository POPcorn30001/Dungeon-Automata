using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public GameManager.EntityClass entityClass = GameManager.EntityClass.Player;
    public bool Flip = false;
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
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

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
            rb.velocity = Vector3.zero;
            return;
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
        FlipEntity(targetDir.x, Flip);


        //atack target
        if(Time.time >= nextAttackTime) Attack(targetDistance, currentTarget.transform.position, GameManager.EntityClass.Enemy);

        
        
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

        //move
        if(targetDistance > range){

            rb.velocity = targetDir * speed;
        }
        else
        { rb.velocity = Vector3.zero; }

        //sort layer
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        FlipEntity(targetDir.x, Flip);

        //atack target
        if(Time.time >= nextAttackTime){
            Attack(targetDistance, currentTarget.transform.position, GameManager.EntityClass.Player);
            Debug.Log("turret shooting, target dir: "+targetDir);
        } 

        
    }
    private void Attack(float targetDistance, Vector3 targetPos, GameManager.EntityClass sender){

        if(targetDistance <= range){

            attackEnd = Time.time + stats.attackTime;
            attacking = true;
            nextAttackTime = Time.time + stats.attackSpeed;
            Vector3 spawnPos = gameObject.transform.position;
            Vector3 targetDir = (targetPos - new Vector3(gameObject.transform.position.x + attackOffsetPoint.x, gameObject.transform.position.y + attackOffsetPoint.y, 0)).normalized;


            spawnPos.x += targetDir.x * attackOffset+ attackOffsetPoint.x;
            spawnPos.y += targetDir.y * attackOffset + attackOffsetPoint.y;
            
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject attack = Instantiate(attackPf, spawnPos, rotation);
            attack.GetComponent<Attack>().Setup(targetDir, sender);
            
        }
    }

    private void FlipEntity(float dirx, bool mirrorFLip){
        //flip player
        if(dirx == 0) return;
        Vector3 scale = gameObject.transform.localScale;

        if(dirx > 0){ //move right
            
            if(mirrorFLip){
                scale.x = Mathf.Abs(scale.x);
                attackOffsetPoint.x = Mathf.Abs(attackOffsetPoint.x);
            }
            else{
                scale.x = Mathf.Abs(scale.x) * -1;
                attackOffsetPoint.x = Mathf.Abs(attackOffsetPoint.x) * -1;
            }
            
        }
        else if(dirx < 0){ //move left
            
            if(mirrorFLip){
                scale.x = Mathf.Abs(scale.x) * -1;
                attackOffsetPoint.x = Mathf.Abs(attackOffsetPoint.x) * -1;
            }
            else{
                scale.x = Mathf.Abs(scale.x);
                attackOffsetPoint.x = Mathf.Abs(attackOffsetPoint.x);
            }
        }
        gameObject.transform.localScale = scale;
    }

}

