using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public Animator sceneTransitionController;
    public Animator UiAnimationController;
    public float transitionDelay = 1f;

    // Pauses the game
    public void PauseUnpauseGame()
    {
        if (GameController.gameIsPaused)
        {
            GameController.gameIsPaused = false;
            Time.timeScale = 1;
            UiAnimationController.SetBool("Pause", false);
        }
        else
        {
            GameController.gameIsPaused = true;
            Time.timeScale = 0;
            UiAnimationController.SetBool("Pause", true);
        }
    }
    public void OpenCloseSwitchMenu()
    {
        if (UiAnimationController.GetBool("Switch"))
            UiAnimationController.SetBool("Switch", false);
        else
            UiAnimationController.SetBool("Switch", true);
    }
    public void OpenCloseSettings()
    {
        if(UiAnimationController.GetBool("Settings"))
            UiAnimationController.SetBool("Settings", false);
        else
            UiAnimationController.SetBool("Settings", true);
    }
    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(SceneTransition("Menu"));
    }
    IEnumerator SceneTransition(string sceneName)
    {
        sceneTransitionController.SetTrigger("Start");

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(sceneName);
    }
}
