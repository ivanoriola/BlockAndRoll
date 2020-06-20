using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScripts : MonoBehaviour
{
    [HideInInspector] public bool paused = false;
    [HideInInspector] public bool audioOn;
    [HideInInspector] public bool musicOn;
    [HideInInspector] public bool audioPanelOn = false;
    [HideInInspector] public bool levelsPanelOn = false;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;

    private GameObject pausePanel;
    private GameObject pauseButton;
    private GameObject pauseBIGButton;
    private GameObject audioButton;
    private GameObject musicButton;
    private GameObject fxButton;
    private GameObject player;
    private GameObject soundManager;
    private GameObject musicManager;
    private GameObject levelsPanel;
    private GameObject introCover;
    private GameObject audioPanel;

    private void Awake()
    {
        pausePanel = GameObject.Find(Constants.PAUSEPANEL_GAMEOBJECT);
        pauseButton = GameObject.Find(Constants.PAUSEBUTTON_GAMEOBJECT);
        pauseBIGButton = GameObject.Find(Constants.PAUSEBIGBUTTON_GAMEOBJECT);
        audioButton = GameObject.Find(Constants.AUDIOBUTTON_GAMEOBJECT);
        musicButton = GameObject.Find(Constants.MUSICBUTTON_GAMEOBJECT);
        fxButton = GameObject.Find(Constants.FXBUTTON_GAMEOBJECT);
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT);
        musicManager = GameObject.Find(Constants.MUSICMANAGER_GAMEOBJECT);
        levelsPanel = GameObject.Find(Constants.LEVELSPANEL_GAMEOBJECT);
        introCover = GameObject.Find(Constants.INTRO_COVER_GAMEOBJECT);
        audioPanel = GameObject.Find(Constants.AUDIOPANEL_GAMEOBJECT);

        levelsPanel.SetActive(false);
        pausePanel.SetActive(false);
        audioPanel.SetActive(false);
        pauseBIGButton.GetComponent<Button>().enabled = false;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }
    public void Pause()
    {
        if (!paused)
        {
            pauseButton.GetComponent<Image>().sprite = playSprite;
            pauseBIGButton.GetComponent<Button>().enabled = true;
            player.GetComponent<Player>().enabled = false;
            levelsPanel.SetActive(false);
            audioPanel.SetActive(false);
            pausePanel.SetActive(true);
            paused = true;
        }
        else
        {
            pauseButton.GetComponent<Image>().sprite = pauseSprite;
            pauseBIGButton.GetComponent<Button>().enabled = false;
            player.GetComponent<Player>().enabled = true;
            levelsPanel.SetActive(false);
            audioPanel.SetActive(false);
            pausePanel.SetActive(false);
            paused = false;
        }
    }
    public void AudioOnOff()
    {
        if (!audioOn)
        {
            fxButton.GetComponent<Image>().color = new Color (1,1,1,1);
            if (soundManager) { soundManager.SetActive(true); }
            audioOn = true;
        }
        else
        {
            fxButton.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
            if (soundManager) { soundManager.SetActive(false); }
            audioOn = false;
        }
    }
    public void MusicOnOff()
    {
        if (!musicOn)
        {
            musicButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            if (musicManager) { musicManager.SetActive(true); }
            musicOn = true;
        }
        else
        {
            musicButton.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
            if (musicManager) { musicManager.SetActive(false); }
            musicOn = false;
        }
    }
    public void AudioPanelOnOff()
    {
        levelsPanel.SetActive(false);
        levelsPanelOn = false;
        if (!audioPanelOn)
        {
            audioPanel.SetActive(true);
            audioPanelOn = true;
        }
        else
        {
            audioPanel.SetActive(false);
            audioPanelOn = false;
        }
    }

    public void LevelsPanelOnOff()
    {
        audioPanel.SetActive(false);
        audioPanelOn = false;
        if (!levelsPanelOn)
        {
            levelsPanel.SetActive(true);
            levelsPanelOn = true;
        }
        else
        {
            levelsPanel.SetActive(false);
            levelsPanelOn = false;
        }
    }
    public void LevelButton(int newLevel)
    {
        player.GetComponent<LevelManager>().level = newLevel;
        player.GetComponent<LevelManager>().ResetCurrentLevel();
        LevelsPanelOnOff();
        Pause();
    }
    public void ExitGame()
    {
        PlayerPrefs.SetInt(Constants.SAVED_LEVEL, player.GetComponent<LevelManager>().level);
        PlayerPrefs.SetInt(Constants.SAVED_TOP_LEVEL, player.GetComponent<LevelManager>().topLevel);
        Pause();
        player.GetComponent<Player>().enabled = false;
        introCover.SetActive(true);
        gameObject.SetActive(false);
        introCover.GetComponent<IntroCoverOnOff>().fromPause = true;
        introCover.GetComponent<IntroCoverOnOff>().gameOver = false;
    }
    public void GameOver()
    {
        PlayerPrefs.SetInt(Constants.SAVED_LEVEL, player.GetComponent<LevelManager>().level);
        PlayerPrefs.SetInt(Constants.SAVED_TOP_LEVEL, player.GetComponent<LevelManager>().topLevel);
        Pause();
        player.GetComponent<Player>().enabled = false;
        introCover.SetActive(true);
        gameObject.SetActive(false);
        introCover.GetComponent<IntroCoverOnOff>().fromPause = true;
        introCover.GetComponent<IntroCoverOnOff>().gameOver = true;
    }
}
