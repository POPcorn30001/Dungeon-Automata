using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject creditPanel;
    [SerializeField] private Difficulty difficulty;
    private bool creditsActive;
    private bool startingGame = false;
    [SerializeField] private AudioSource audioSource;

    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update(){
        if(startingGame && !audioSource.isPlaying){
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void OnPlayClicked(){
        difficulty.extraEnemies = 0;
        difficulty.spawnerSpeedup = 1;
        startingGame = true;
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

    public void PlayButtonSound(){
        audioSource.Play();
    }

}
