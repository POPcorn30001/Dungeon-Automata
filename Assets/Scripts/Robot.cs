using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public GameManager.EntityClass entityClass = GameManager.EntityClass.Player;
    public bool Flip = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private GameObject currentTarget;
    [SerializeField] private StatsObject stats;
    

    private bool setUpDone = false;


    //move
    public float speed= 1;
    [SerializeField] private bool moveEnabled = false;

    
    //explode
    [SerializeField] private GameObject explodeAttackPf;
    private bool explodeEnabled = false;
    private int explodeAmount = 1;

    //Attacking
    [SerializeField] private GameObject meleeAttackPf;
    [SerializeField] private GameObject rangedAttackPf;
    [SerializeField]private float rangeMelee = 1;
    [SerializeField]private float rangeRanged = 7;
    [SerializeField]private float meleeAttackSpeed = 2;
    [SerializeField]private float rangedAttackSpeed = 3;
    [SerializeField]private bool meleeEnabled =false;
    [SerializeField]private bool rangedEnabled = false;
    [SerializeField]private float nextAttackMelee =0;
    [SerializeField]private float nextAttackRanged =0;
    

    [SerializeField] private float meleeOffset =1;
    [SerializeField] private Vector2 attackOffsetPoint;
    [SerializeField] float targetDistance =0;
    [SerializeField] Vector3 targetDir = Vector3.zero;
    
    
    


    //Robot stuff
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        

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
        if(explodeEnabled) Explode();
        FlipEntity(targetDir.x, Flip);
    }   

    public void RobotSetUp(List<RobotBox.Components> partsList){
        
        //set base stats
        rb = gameObject.GetComponent<Rigidbody2D>();




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
                case RobotBox.Components.Explode:
                    if(!explodeEnabled){
                        explodeEnabled = true;
                    }
                    else{
                        explodeAmount += 1;
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
        Attack(rangedAttackPf, false);
    }

    private void Melee(){
        if(nextAttackMelee > Time.time || targetDistance > rangeMelee) return;
        nextAttackMelee = Time.time + meleeAttackSpeed;
        Debug.Log("Robot melle atack");
        Attack(meleeAttackPf, true);
    }
    private void Move(){
        

        //move
        if( (meleeEnabled && targetDistance > rangeMelee) || (rangedEnabled && targetDistance > rangeRanged)){ //not in range

            rb.velocity = targetDir * speed;
        }
        else if(!meleeEnabled && rangedEnabled && targetDistance < rangeRanged-1){ //flee when only ranged
            rb.velocity = (-targetDir) * speed;
        }
        else if(!meleeEnabled && !rangedEnabled && targetDistance > 0.5){ //only move
            rb.velocity = targetDir * speed;
        }
        else
        { rb.velocity = Vector3.zero; }
    }

    private void Explode(){

        if(targetDistance < 1){
            for(int i =0; i < explodeAmount; i++){
                Attack(explodeAttackPf, false);
            }
            gameObject.GetComponent<Health>().TakeDamage(100);
        }
    }
    private void Attack(GameObject attackPf, bool melee){
        if(!attackPf) return;

        Vector3 spawnPos = gameObject.transform.position;

        if(!melee){
            spawnPos.x += attackOffsetPoint.x;
            spawnPos.y += attackOffsetPoint.y;
        }
        else{
            spawnPos.x += targetDir.x * meleeOffset;
            spawnPos.y += targetDir.y * meleeOffset;
        }
        
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject attack = Instantiate(attackPf, spawnPos, rotation);
        attack.GetComponent<Attack>().Setup(targetDir, GameManager.EntityClass.Player);
        
        
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
