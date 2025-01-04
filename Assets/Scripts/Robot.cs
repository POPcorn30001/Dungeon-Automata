using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public GameManager.EntityClass entityClass = GameManager.EntityClass.Player;
    public bool Flip = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private GameObject currentTarget;
    [SerializeField] private StatsObject stats;
    

    private bool setUpDone = false;


    //move
    public float speed= 1;
    private bool moveEnabled = false;

    
    

    //Attacking
    [SerializeField] private GameObject meleeAttackPf;
    [SerializeField] private GameObject rangedAttackPf;
    private float rangeMelee = 1;
    private float rangeRanged = 7;
    private float meleeAttackSpeed = 2;
    private float rangedAttackSpeed = 3;
    private bool meleeEnabled =false;
    private bool rangedEnabled = false;
    private float nextAttackMelee =0;
    private float nextAttackRanged =0;

    [SerializeField] private float attackOffset;
    [SerializeField] private Vector2 attackOffsetPoint;
    float targetDistance =0;
    Vector3 targetDir = Vector3.zero;
    
    
    


    //Robot stuff
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    void Update(){
        
        if(!setUpDone) return;

        currentTarget = GameManager.Instance.NearestEntity(GameManager.EntityClass.Enemy,gameObject.transform.position);
        if(!currentTarget) return;

        targetDistance = Vector2.Distance(gameObject.transform.position, currentTarget.transform.position);
        targetDir = (currentTarget.transform.position - gameObject.transform.position).normalized;

        if(rangedEnabled) Ranged();
        if(meleeEnabled) Melee();
        if(moveEnabled) Move();
    }   

    public void RobotSetUp(List<RobotBox.Components> partsList){
        
        //set base stats





        //go through list
        foreach (var part in partsList){

            switch (part){

                case RobotBox.Components.Melee:
                    if(!meleeEnabled){
                        meleeEnabled = true;
                    }
                    else{
                        meleeAttackSpeed /= 2f;
                    }
                    break;
                case RobotBox.Components.Shoot:
                    if(!rangedEnabled){
                        rangedEnabled = true;
                    }
                    else{
                        rangedAttackSpeed /= 2f;
                    }
                    break;
                case RobotBox.Components.Move:
                    if(!moveEnabled){
                        moveEnabled = true;
                        rb.bodyType = RigidbodyType2D.Dynamic;
                    }
                    else{
                        speed += 2;
                    }
                    break;
                default:
                    break;
            }
        }


        setUpDone = true;
    }


    private void Ranged(){

        if(nextAttackRanged > Time.time || targetDistance > rangeRanged) return;
        nextAttackRanged = Time.time + rangedAttackSpeed;


    }

    private void Melee(){

    }
    private void Move(){
        

        //move
        if(targetDistance > range){

            rb.velocity = targetDir * speed;
        }
        else
        { rb.velocity = Vector3.zero; }
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
