using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModuleItem : MonoBehaviour
{
    [SerializeField] private TMP_Text textTMP;
    private RobotBox.Components type;

    private void EditText(string text){
        textTMP.text = ""+text;
    }

    public void SetComponentType(RobotBox.Components module){
        type = module;

        switch(module){
            case RobotBox.Components.Melee:
                EditText("Melee Module");
                break;
            case RobotBox.Components.Shoot:
                EditText("Ranged Module");
                break;
            case RobotBox.Components.Move:
                EditText("Move Module");
                break;
            case RobotBox.Components.Explode:
                EditText("Explosion load");
                break;
            default:
                EditText("Missing Variant");
                break;
        }
    }

    public void RemoveSelfFromList(){
        TinkerPanel.Instance.RemoveFromList(type);
        GameManager.Instance.audioSource.Play();
        Destroy(gameObject);
    }
}
