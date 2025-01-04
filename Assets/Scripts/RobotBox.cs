using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotBox : MonoBehaviour
{
    public enum Components{
        Melee,
        Shoot,
        Move,
        Explode,
        Heal
    }

    private List<Components> parts = new List<Components>();
    [SerializeField] GameObject indicatorPanelPrefab;
    [SerializeField] GameObject tinkerPanelPrefab;
    private GameObject indicatorPanel;
    [SerializeField] private Vector2 indicatorPanelOffset;
    private Indicator indicator;
    private GameObject tinkerPanel;
    private Canvas worldUiCanvas;

    
    public bool hasComponents = false;
    private bool playerInRange = false;
    public float progress = 0;
    public float buildTime = 4;
    public float range = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddEntityToList(gameObject, GameManager.EntityClass.RobotBox);
        worldUiCanvas = GameManager.Instance.GetWorldCanvas();
        indicatorPanel = Instantiate(indicatorPanelPrefab, worldUiCanvas.transform);
        indicatorPanel.transform.position = new Vector3(gameObject.transform.position.x + indicatorPanelOffset.x, gameObject.transform.position.y + indicatorPanelOffset.y);
        indicator = indicatorPanel.GetComponent<Indicator>();
        if(!indicator) Debug.Log("Missing indicator");
        indicatorPanel.SetActive(false);

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        
        //spawn UI
    }


    
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(GameManager.Instance.player.transform.position, gameObject.transform.position) <= range ){
            indicatorPanel.SetActive(true);
            playerInRange = true;
        }
        else{
            indicatorPanel.SetActive(false);
            playerInRange = false;
        }
    }

    public void OnInteract(){
        if(!playerInRange) return;
        
        if(!hasComponents){ //tinker phase
            //TODO, skip





            hasComponents = true;
            indicator.ShowHold();

        }
        else{ //build phase
            progress += Time.deltaTime / buildTime;
            indicator.ChangeFill(progress);
            if(progress >= 1){
                FinisBuild();
            }
        }
    }

    private void FinisBuild(){
        //spawn robot
        Debug.Log("building done");
        Destroy(indicatorPanel);
        Destroy(tinkerPanel);

        GameManager.Instance.RemoveEntityFromList(gameObject, GameManager.EntityClass.RobotBox);
        Instantiate(Resources.Load<GameObject>("Prefabs/Turret"), gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
