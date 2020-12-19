using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState { Player1, Player2, EndGame }
    public GameState gameState;
    public static bool gameIsPaused=false;

    // STATIC SETTINGS
    public static Color32 playerOneColor = new Color32(0,173,34,255);
    public static Color32 playerTwoColor = new Color32(177, 0,0,255);

    // PLAYER REFERENCES
    public GameObject playerOne;
    public GameObject playerTwo;

    // UI ELEMENTS
    public GameObject moveJoystick;
    public GameObject aimJoystick;
    public GameObject shoot;

    public void Start()
    {
        gameState = GameState.Player1;
        SetPlayerColors();
    }
    // Sets player colors
    public void SetPlayerColors()
    {
        playerOne.GetComponent<MeshRenderer>().materials[2].color = playerOneColor;
        playerOne.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = playerOneColor;
        playerOne.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = playerOneColor;

        GameObject p1Health = playerOne.GetComponent<PlayerStats>().healthbar.gameObject;
        p1Health.transform.Find("Fill").GetComponent<Image>().color = playerOneColor;

        playerTwo.GetComponent<MeshRenderer>().materials[2].color = playerTwoColor;
        playerTwo.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = playerTwoColor;
        playerTwo.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = playerTwoColor;

        GameObject p2Health = playerTwo.GetComponent<PlayerStats>().healthbar.gameObject;
        p2Health.transform.Find("Fill").GetComponent<Image>().color = playerTwoColor;

        UiColorChange(playerOneColor, true);
    }
    // Ends the current players turn
    public void EndTurn()
    { 
        if(gameState == GameState.EndGame)
        {
            return;
        }
        else if (gameState == GameState.Player1)
        {
            gameState = GameState.Player2;
            UiColorChange(playerTwoColor, false);

            shoot.GetComponent<Button>().onClick.RemoveAllListeners();
            shoot.GetComponent<Button>().onClick.AddListener(() => playerTwo.GetComponent<PlayerShoot>().Shoot());

        }
        else if (gameState == GameState.Player2)
        {
            gameState = GameState.Player1;
            UiColorChange(playerOneColor, true);

            shoot.GetComponent<Button>().onClick.RemoveAllListeners();
            shoot.GetComponent<Button>().onClick.AddListener( () => playerOne.GetComponent<PlayerShoot>().Shoot() );
        }
        GameObject.Find("Timer").GetComponent<TurnTimer>().DeactivateTimer();
        this.GetComponent<UIController>().OpenCloseSwitchMenu();
        
    }
    // Ends the game and displays result
    public void EndGame()
    {
        if (gameState == GameState.EndGame)
            return;
        float playerOneHp = playerOne.GetComponent<PlayerStats>().GetCurrentHealth();
        float playerTwoHp = playerTwo.GetComponent<PlayerStats>().GetCurrentHealth();

        GameObject GameUI = this.gameObject.transform.Find("GameUI").gameObject;
        if (GameUI.activeInHierarchy)
            GameUI.SetActive(false);
        else return;

        Color draw = new Color(255, 255, 255, 255);
        if (playerOneHp == playerTwoHp)
        {
            EndGameSceneSetup("DRAW!", "Both players HP reached 0!", draw);
        }
        else
        {
            if (playerOneHp > playerTwoHp)
                EndGameSceneSetup("WINNER!", "Player one wins!", playerOneColor);
            else
                EndGameSceneSetup("WINNER!", "Player two wins!", playerTwoColor);
        }

        Time.timeScale = 0;
        gameState = GameState.EndGame;
    }
    // Changes UI color to c and switches on/off the scripts for player control
    void UiColorChange(Color c, bool b)
    {
        moveJoystick.GetComponent<Image>().color = c;
        moveJoystick.transform.Find("Handle").GetComponent<Image>().color = c;

        aimJoystick.GetComponent<Image>().color = c;
        aimJoystick.transform.Find("Handle").GetComponent<Image>().color = c;

        shoot.GetComponent<Image>().color = c;

        playerOne.GetComponent<PlayerMovement>().enabled = b;
        playerOne.GetComponent<PlayerAim>().enabled = b;
        playerOne.GetComponent<PlayerShoot>().enabled = b;

        playerTwo.GetComponent<PlayerMovement>().enabled = !b;
        playerTwo.GetComponent<PlayerAim>().enabled = !b;
        playerTwo.GetComponent<PlayerShoot>().enabled = !b;
    }
    // Sets up the UI for the end game screen
    void EndGameSceneSetup(string result, string smallText, Color c)
    {
        GameObject winnerUI = GameObject.Find("UI").transform.Find("WinnerUI").gameObject;
        winnerUI.SetActive(true);

        winnerUI.transform.Find("Winner").gameObject.GetComponent<TextMeshProUGUI>().text = result;
        winnerUI.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = smallText;


        winnerUI.transform.Find("Winner").gameObject.GetComponent<TextMeshProUGUI>().color = c;
        winnerUI.transform.Find("Replay").gameObject.GetComponent<Image>().color = c;
        winnerUI.transform.Find("MainMenu").gameObject.GetComponent<Image>().color = c;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SelectQualityDropdown()
    {
        int qualityIndex = QualitySettings.GetQualityLevel();
        transform.Find("SettingsUI").Find("Graphics").GetChild(1).GetComponent<TMP_Dropdown>().value = qualityIndex;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    // Add event to the shoot button based on the turn player
    public void AddEventToPlayer()
    {
        if(this.gameState== GameState.Player1)
        {
            GameObject shoot = GameObject.Find("ShootBtn").gameObject;
            shoot.GetComponent<Button>().onClick.RemoveAllListeners();
            shoot.GetComponent<Button>().onClick.AddListener(() => playerOne.GetComponent<PlayerShoot>().Shoot());
        }
        else if( this.gameState == GameState.Player2)
        {
            GameObject shoot = GameObject.Find("ShootBtn").gameObject;
            shoot.GetComponent<Button>().onClick.RemoveAllListeners();
            shoot.GetComponent<Button>().onClick.AddListener(() => playerTwo.GetComponent<PlayerShoot>().Shoot());
        }
    }
}