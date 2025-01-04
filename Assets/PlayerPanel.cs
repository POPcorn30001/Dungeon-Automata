using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text parts;

    public void SetHealth(int value){
        health.text = ""+value;
    }

    public void SetParts(int value){
        parts.text = ""+value;
    }
}
