using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{

    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text holdText;


    public void ChangeFill(float val){
         if(!fillImage) return;

        fillImage.fillAmount = val;
    }

    public void ShowHold(){
       if(!holdText) return;
       holdText.gameObject.SetActive(true);
        
    }

    public void HideHold(){
        if(!holdText) return;
        holdText.gameObject.SetActive(false);
    }


}
