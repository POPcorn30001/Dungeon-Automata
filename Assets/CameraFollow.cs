using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    public float zPos = -10;

    // Update is called once per frame
    void Update()
    {
       if(target) Follow();
    }

    void Follow(){
         Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y+1, zPos);
        gameObject.transform.position = pos;
    }
}
