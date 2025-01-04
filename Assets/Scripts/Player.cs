using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    private PlayerInput playerInput;    
    private Rigidbody2D rb;
    

  
    
    [SerializeField] private StatsObject stats;
    public float speed = 1;
    public float range = 1;

    private float nextTurret =0;
    public float turretCooldown;
    private bool interactHeld = false;

   
    [SerializeField] private GameObject attackPf;
    [SerializeField] private Vector2 attackOffsetPoint;
    private bool attacking = false;
    private float attackEnd =0;
    private float nextAttackTime;

    private void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInput = gameObject.GetComponent<PlayerInput>();

        speed = stats.speed;
        range = stats.range;
    }
    void Update()
    {
        if(!attacking){
            MovePlayer(moveInput);
        }
        else{
            rb.velocity = Vector2.zero;
        }
        if(Time.time > attackEnd) attacking = false;

        if(Input.GetKeyDown(KeyCode.T)) PlaceTurret();

        if(interactHeld){
            GameObject closestBox = GameManager.Instance.NearestEntity(GameManager.EntityClass.RobotBox, gameObject.transform.position);
            if(!closestBox) return;
        
            closestBox.GetComponent<RobotBox>().OnInteract();
        }
    }


    public void OnMove(InputAction.CallbackContext ctx){
        moveInput = ctx.ReadValue<Vector2>();
        //Debug.Log(moveInput);
    }

    public void OnInteract(InputAction.CallbackContext ctx){
        interactHeld = ctx.ReadValueAsButton();
    }

    private void MovePlayer(Vector2 dir){
        Vector3 move = new Vector3(dir.x, dir.y, 0);
        rb.velocity = move * speed;
        
        //sort layer
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        //animate
        if(move != Vector3.zero){
            animator.SetBool("Moving", true);
        }
        else{
            animator.SetBool("Moving", false);
        }

        //flip player
        if(rb.velocity.x == 0) return;
        Vector3 scale = gameObject.transform.localScale;

        if(rb.velocity.x > 0){ //move right
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else if(rb.velocity.x < 0){
            scale.x = Mathf.Abs(scale.x);
        }
        gameObject.transform.localScale = scale;


    }

    private void Attack(){
        if(attacking || nextAttackTime > Time.time) return;


        attacking = true;
        attackEnd = Time.time + stats.attackTime;
        nextAttackTime = Time.time + stats.attackSpeed;
        Vector3 spawnPos = gameObject.transform.position;

        float flip;
        if(gameObject.transform.localScale.x >= 0){
            spawnPos.x += attackOffsetPoint.x;
            spawnPos.y += attackOffsetPoint.y;
            flip = 1;
        }
        else{
            spawnPos.x -= attackOffsetPoint.x;
            spawnPos.y += attackOffsetPoint.y;
            flip = -1;
        }
        
        GameObject attack = Instantiate(attackPf, spawnPos, Quaternion.identity);
        attack.GetComponent<Attack>().Setup(Vector2.zero, GameManager.EntityClass.Player);
        Vector3 scale = attack.transform.localScale;
        scale.x *= flip;
        attack.transform.localScale = scale;
    }
    
    private void PlaceTurret(){
        if(nextTurret > Time.time) return;

        
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        if(GameManager.Instance.SpawnRobot(pos)){ //placement succes
            nextTurret = Time.time + turretCooldown;
        } 
        
    }
    
}
