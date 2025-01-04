 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] Difficulty difficulty;

    public int countLeft =0;
    public float cooldown;
    private float nextSpawn;
    public bool minimumRange =false;
    public Vector2 spawnerRadius = Vector2.zero;
    public Vector2 minimumRadius = Vector2.zero;

    void Start(){
        if(difficulty){
            cooldown /=difficulty.spawnerSpeedup;
            countLeft += difficulty.extraEnemies;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(countLeft >0) SpawnEntity();
    }


    private void SpawnEntity(){
        if(Time.time < nextSpawn) return;

        nextSpawn = Time.time + cooldown;
        countLeft--;
        
        Vector3 pos = gameObject.transform.position;
        var ranPosX = Random.Range(-spawnerRadius.x, spawnerRadius.x);
        var ranPosY = Random.Range(-spawnerRadius.y, spawnerRadius.y);
        if(minimumRange){
            if(ranPosX < 0 && ranPosX > -minimumRadius.x) ranPosX = -minimumRadius.x;
            if(ranPosX > 0 && ranPosX <  minimumRadius.x) ranPosX = minimumRadius.x;
            if(ranPosY < 0 && ranPosY > -minimumRadius.y) ranPosY = -minimumRadius.y;
            if(ranPosY < 0 && ranPosY > -minimumRadius.y) ranPosY = -minimumRadius.y;
        }

        pos.x += ranPosX;
        pos.y += ranPosY;
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
