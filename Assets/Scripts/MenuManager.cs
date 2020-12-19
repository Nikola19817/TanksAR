using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public ToggleGroup playerOneToggleGroup;
    public ToggleGroup playerTwoToggleGroup;

    public Animator sceneTransitionController;
    public float transitionDelay = 1f;

    public Animator UiAnimationController;
    public void PlayScene()
    {
        StartCoroutine(SceneTransition("Scenes/Game"));
    }
    IEnumerator SceneTransition(string sceneName)
    {
        sceneTransitionController.SetTrigger("Start");

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public static void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SelectQualityDropdown()
    {
        int qualityIndex = QualitySettings.GetQualityLevel();
        transform.Find("OptionsMenu").Find("Graphics").GetChild(1).GetComponent<TMP_Dropdown>().value = qualityIndex;
    }
    public void SetPlayerColors()
    {
        IEnumerable<Toggle> playerOneColor = playerOneToggleGroup.ActiveToggles();
        GameController.playerOneColor = playerOneColor.First().GetComponent<Toggle>().colors.normalColor;

        IEnumerable<Toggle> playerTwoColor = playerTwoToggleGroup.ActiveToggles();
        GameController.playerTwoColor = playerTwoColor.First().GetComponent<Toggle>().colors.normalColor;
    }
    public void SelectPlayerColorOptions()
    {
        Color32 colorOne = GameController.playerOneColor;
        Color32 colorTwo = GameController.playerTwoColor;

        for(int i = 0; i < playerOneToggleGroup.transform.childCount; i++)
        {
            Toggle t1 = playerOneToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
            if (t1.colors.normalColor == colorOne)
                t1.isOn = true;

            Toggle t2 = playerTwoToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
            if (t2.colors.normalColor == colorTwo)
                t2.isOn = true;

        }
    }
    public void OpenCloseOptions()
    {
        if (UiAnimationController.GetBool("Options"))
            UiAnimationController.SetBool("Options", false);
        else
            UiAnimationController.SetBool("Options", true);
    }
}