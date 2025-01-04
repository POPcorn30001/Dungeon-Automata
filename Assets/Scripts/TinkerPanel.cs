using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TinkerPanel : MonoBehaviour
{
    private static TinkerPanel sInstance;
    public static TinkerPanel Instance{ 
        get { return sInstance; } 
    }


    public GameObject lastBox;
    public int cost =0;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject buildButton;
    [SerializeField] private List<RobotBox.Components> parts = new List<RobotBox.Components>();
    [SerializeField] private GameObject listItemPF;
    [SerializeField] private GameObject list;
    
    

    void Awake(){
        if (sInstance != null && sInstance != this)
        { Destroy(gameObject); }
        else
        { sInstance = this; }
        gameObject.SetActive(false);
    }


    public void AddToList(int num){
        
        RobotBox.Components module = (RobotBox.Components)num;
        parts.Add(module);

        GameObject item = Instantiate(listItemPF, list.transform);
        item.GetComponent<ModuleItem>().SetComponentType(module);
        switch (module){

            case RobotBox.Components.Melee:
                cost += 1;
                break;
            case RobotBox.Components.Shoot:
                cost += 2;
                break;
            case RobotBox.Components.Move:
                cost += 1;
                break;
            case RobotBox.Components.Explode:
                cost += 2;
                break;
            default:
                break;
        }

        costText.text = "Cost:  "+cost;
    }

    public void RemoveFromList(RobotBox.Components module){
        if(parts.Contains(module)) parts.Remove(module);

        switch (module){

            case RobotBox.Components.Melee:
                cost -= 1;
                break;
            case RobotBox.Components.Shoot:
                cost -= 2;
                break;
            case RobotBox.Components.Move:
                cost -= 1;
                break;
            case RobotBox.Components.Explode:
                cost -= 2;
                break;
            default:
                break;
        }

        costText.text = "Cost:  "+cost;
    }

    public void CompleteBuild(){
        
        if(cost > GameManager.Instance.parts || parts.Count == 0) return;

        GameManager.Instance.parts -= cost;
        GameManager.Instance.playerPanel.SetParts(GameManager.Instance.parts);

        lastBox.GetComponent<RobotBox>().SetComponents(parts);
        Debug.Log("Giving box components: "+parts);
        Cancel();
    }

    public void Cancel(){

        cost = 0;
        costText.text = "Cost:  "+cost;
        parts.Clear();
        foreach (Transform child in list.transform)
        {
            Destroy(child.gameObject);
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
