using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject creditPanel;
    private bool creditsActive;

    public void OnPlayClicked(){
        SceneManager.LoadScene(1);
    }

    public void OnCreditsClicked(){
        if(!creditPanel) return;

        if(creditsActive){
            creditPanel.SetActive(false);
            creditsActive = false;
        }
        else{
            creditPanel.SetActive(true);
            creditsActive = true;
        }
    }
    public void OnExitClicked(){
        Application.Quit();
    }

    private void PlayButtonSound(){
       
    }

}
