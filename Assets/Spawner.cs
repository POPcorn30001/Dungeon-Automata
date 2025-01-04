 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public int countLeft =0;
    public float cooldown;
    private float nextSpawn;
    public Vector2 spawnerRadius = Vector2.zero;

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
        pos.x += Random.Range(-spawnerRadius.x, spawnerRadius.x);
        pos.y += Random.Range(-spawnerRadius.y, spawnerRadius.y);

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
