using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCoverOnOff : MonoBehaviour
{
    [SerializeField] Text continueLevelText;
    private GameObject player;
    private GameObject userInterface;
    private GameObject continueButton;
    private GameObject gameOverObject;
    private GameObject cover;
    private GameObject buttons;
    private GameObject difficultButtons;
    public bool fromPause = false;
    public bool gameOver = false;

    void Start()
    {
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
        userInterface = GameObject.Find(Constants.UI_GAMEOBJECT);
        continueButton = GameObject.Find(Constants.CONT_BUTTON_GAMEOBJECT);
        gameOverObject = GameObject.Find(Constants.GAMEOVER_GAMEOBJECT);
        cover = GameObject.Find(Constants.COVER_GAMEOBJECT);
        buttons = GameObject.Find(Constants.BUTTONS_GAMEOBJECT);
        difficultButtons = GameObject.Find(Constants.DIFFICULTBUTTONS_GAMEOBJECT);
        player.GetComponent<Player>().enabled = false;
        userInterface.SetActive(false);
        difficultButtons.SetActive(false);
    }
    private void OnEnable()
    {
        if (difficultButtons) difficultButtons.SetActive(false);
        buttons.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewGame();
        }

        gameOverObject.SetActive(gameOver);
        //cover.SetActive(!gameOver);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
        if (PlayerPrefs.HasKey(Constants.SAVED_TOP_LEVEL) && PlayerPrefs.GetInt(Constants.SAVED_TOP_LEVEL) != 0 && !gameOver)
        {
            continueButton.SetActive(true);
            continueLevelText.text = "Level " + PlayerPrefs.GetInt(Constants.SAVED_LEVEL);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void easy()
    {
        player.GetComponent<LevelManager>().level = 0;
        player.GetComponent<LevelManager>().topLevel = 0;
        player.GetComponent<LevelManager>().hard = 0;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(Constants.SAVED_HARD, 0);
        
        RunGame();
        gameOver = false;
    }
    public void hard()
    {
        player.GetComponent<LevelManager>().level = 0;
        player.GetComponent<LevelManager>().topLevel = 0;
        player.GetComponent<LevelManager>().hard = 1;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(Constants.SAVED_HARD, 1);
        RunGame();
        gameOver = false;
    }

    public void NewGame()
    {
        buttons.SetActive(false);
        difficultButtons.SetActive(true);
    }
    public void Continue()
    {
        if (PlayerPrefs.HasKey(Constants.SAVED_LEVEL))
        {
            player.GetComponent<LevelManager>().level = PlayerPrefs.GetInt(Constants.SAVED_LEVEL);
            player.GetComponent<LevelManager>().topLevel = PlayerPrefs.GetInt(Constants.SAVED_TOP_LEVEL);
            player.GetComponent<LevelManager>().hard = PlayerPrefs.GetInt(Constants.SAVED_HARD);
        }
        else
        {
            player.GetComponent<LevelManager>().level = 0;
            player.GetComponent<LevelManager>().topLevel = 0;
        }
        RunGame();
    }
    private void RunGame()
    {
        player.GetComponent<Player>().enabled = true;

        if (player.GetComponent<LevelManager>().level == 0)
        {
            player.GetComponent<Transform>().position = Vector3.zero;
            player.GetComponent<LevelManager>().ResetCurrentLevel();
        }
        else
        {
            if (!fromPause)
            {
                player.GetComponent<LevelManager>().ResetCurrentLevel();
                player.GetComponent<LevelManager>().go = true;
            }
            fromPause = false;
        }

        userInterface.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        PlayerPrefs.SetInt(Constants.SAVED_LEVEL, player.GetComponent<LevelManager>().level);
        PlayerPrefs.SetInt(Constants.SAVED_TOP_LEVEL, player.GetComponent<LevelManager>().topLevel);
        Application.Quit();
    }
}
