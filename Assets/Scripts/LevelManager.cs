using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Text levelTxt;
    [SerializeField] Text topLevelTxt;
    [SerializeField] Text movesTxt;
    [SerializeField] Image titleBGColor;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject[] levels;

    private static string CHANGE_BG_COLOR_COROUTINE = "ChangeBackgroundColor";
    private static string INITIATE_COROUTINE = "Initiate";
    private static string RESET_CURRENT_LEVEL_COROUTINE = "ResetCurrentLevelCoroutine";

    public int level;
    public int topLevel;
    private int remainingMoves;
    private int extraMoves = 0;
    public bool goalReached = false;
    public int hard;

    private float minDistance = .075f;
    private float speed = 15;
    private int height = 5;

    public bool go = false;
    private bool goingUp = false;
    private Vector3 targetUp;
    private bool goingDown = false;
    private Vector3 targetDown;
    private bool goingNextPos = false;
    private Vector3 targetNext;

    SoundManager soundManager;
    MusicManager musicManager;

    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
        musicManager = GameObject.Find(Constants.MUSICMANAGER_GAMEOBJECT).GetComponent<MusicManager>();
        LevelsOnOff();
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCurrentLevel();
        }
        if (level >= levels.Length)
        {
            level = levels.Length - 1;
            GameObject.Find("UI").GetComponent<UIScripts>().GameOver();
        }
        if (hard == 1)
        {
            remainingMoves = extraMoves + CurrentLevelMaxMoves() - GetComponent<Player>().moves;
            movesTxt.text = "Moves " + remainingMoves.ToString();

            if (!goalReached && remainingMoves == 0)
            {
                ResetCurrentLevel();
            }
        }
        else
        {
            movesTxt.text = "";
        }
        if (go)
        {
            go = false;

            levelTxt.text = "Level " + level;
            CheckIfTopLevel();
            GetComponent<Player>().Resetmoves();
            StartCoroutine(CHANGE_BG_COLOR_COROUTINE);
            StartCoroutine(INITIATE_COROUTINE);
            soundManager.Play(soundManager.audioNewLevel);
            if (musicManager) musicManager.NewMusic(level);
        }
        if (goingUp)
        {
            GoUp();
        }
        else if (goingNextPos)
        {
            GoToNextPosition();
        }
        else if (goingDown)
        {
            GoDown();
        }
    }
    IEnumerator Initiate()
    {
        CollidersOnOff(false);
        do { yield return null; }
        while (GetComponent<Player>().moving);
        goingUp = true;
        goingNextPos = false;
        goingDown = false;
        targetUp = transform.position + new Vector3(0, height, 0);
        targetNext = levels[level].transform.GetChild(0).transform.position + new Vector3(0, height, 0);
        targetDown = levels[level].transform.GetChild(0).transform.position;
    }
    private void GoUp()
    {
        transform.LookAt(targetUp);
        float distance = Vector3.Distance(transform.position, targetUp);
        if (distance > minDistance)
        {
            transform.position += transform.forward * speed * (distance / 3) * Time.deltaTime;
        }
        else
        {
            LevelsAllOn();
            mainCamera.GetComponent<CameraScript>().newTargets(transform, levels[level].transform.GetChild(1).transform);
            GetComponent<Player>().GridAdjustment();
            goingUp = false;
            goingNextPos = true;
            goingDown = false;
        }
    }
    private void GoToNextPosition()
    {
        transform.LookAt(targetNext);
        float distance = Vector3.Distance(gameObject.transform.position, targetNext);
        if (distance > minDistance)
        {
            gameObject.transform.position += gameObject.transform.forward * speed * (distance / 4) * Time.deltaTime;
        }
        else
        {
            LevelsOnOff();
            GetComponent<Player>().GridAdjustment();
            goingUp = false;
            goingNextPos = false;
            goingDown = true;
            soundManager.Play(soundManager.audioDown);
        }
    }
    private void GoDown()
    {
        gameObject.transform.LookAt(targetDown);
        float distance = Vector3.Distance(gameObject.transform.position, targetDown);
        if (distance > minDistance)
        {
            gameObject.transform.position += gameObject.transform.forward * speed * (distance / 3) * Time.deltaTime;
        }
        else
        {
            CollidersOnOff(true);
            GetComponent<Player>().GridAdjustment();
            GetComponent<Player>().waiting = false;
            goingUp = false;
            goingNextPos = false;
            goingDown = false;
        }
        PlayerPrefs.SetInt(Constants.SAVED_LEVEL, level);
        PlayerPrefs.SetInt(Constants.SAVED_TOP_LEVEL, topLevel);
    }

    private void CollidersOnOff(bool activate)
    {
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = activate;
        }
    }

    private void LevelsAllOn()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            ActivateLevel(i);
        }
    }
    private void LevelsOnOff()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == level)
            {
                DeactivateLevel(level);
                ActivateLevel(level);
            }
            else if (level - 2 < i && level + 2 > i)
            {
                ActivateLevel(i);
            }
            else
            {
                DeactivateLevel(i);
            }
        }
    }
    public void ResetCurrentLevel()
    {
        StartCoroutine(RESET_CURRENT_LEVEL_COROUTINE);
    }
    IEnumerator ResetCurrentLevelCoroutine()
    {
        GetComponent<Player>().waiting = true;
        do { yield return null; }
        while (GetComponent<Player>().moving);
        DeactivateLevel(level);
        go = true;
        ActivateLevel(level);
        GetComponent<Player>().Resetmoves();
    }
    private void ActivateLevel(int level)
    {
        if (levels[level].activeInHierarchy == false)
        {
            levels[level].SetActive(true);
        }
    }
    private void DeactivateLevel(int level)
    {
        if (levels[level].activeInHierarchy == true)
        {
            levels[level].SetActive(false);
        }
    }
    private void CheckIfTopLevel()
    {
        if (level > topLevel)
        {
            topLevel = level;
        }
        topLevelTxt.text = "TopLevel " + topLevel;
    }
    IEnumerator ChangeBackgroundColor()
    {
        float duration = 3;
        float smoothness = 0.05f;
        float progress = 0;
        float increment = smoothness / duration;
        Color currentColor = mainCamera.backgroundColor;
        Color newLevelColor;
        if (level == 0)
        {
            newLevelColor = Constants.INTRO_COLOR;
        }
        else if (level > 0 && level < 5)
        {
            newLevelColor = Constants.PHASE01_COLOR;
        }
        else if (level >= 5 && level < 9)
        {
            newLevelColor = Constants.PHASE02_COLOR;
        }
        else if (level >= 9 && level < 13)
        {
            newLevelColor = Constants.PHASE03_COLOR;
        }
        else if (level >= 13 && level < 17)
        {
            newLevelColor = Constants.PHASE04_COLOR;
        }
        else if (level >= 17 && level < 21)
        {
            newLevelColor = Constants.PHASE05_COLOR;
        }
        else if (level >= 21 && level < 25)
        {
            newLevelColor = Constants.PHASE06_COLOR;
        }
        else if (level >= 25 && level < 29)
        {
            newLevelColor = Constants.PHASE07_COLOR;
        }
        else if (level >= 29 && level < 33)
        {
            newLevelColor = Constants.PHASE08_COLOR;
        }
        else if (level >= 33 && level < 37)
        {
            newLevelColor = Constants.PHASE09_COLOR;
        }
        else newLevelColor = Constants.INTRO_COLOR;
        titleBGColor.color = newLevelColor;
        while (progress < 1)
        {
            mainCamera.backgroundColor = Color.Lerp(currentColor, newLevelColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
    private int CurrentLevelMaxMoves()
    {
        if (level == 0) return 3;
        if (level == 1) return 14;
        if (level == 2) return 16;
        if (level == 3) return 22;
        if (level == 4) return 32;
        if (level == 5) return 10;
        if (level == 6) return 40;
        if (level == 7) return 28;
        if (level == 8) return 52;
        if (level == 9) return 14;
        if (level == 10) return 23;
        if (level == 11) return 55;
        if (level == 12) return 104;
        if (level == 13) return 24;
        if (level == 14) return 30;
        if (level == 15) return 47;
        if (level == 16) return 66;
        if (level == 17) return 10;
        if (level == 18) return 74;
        if (level == 19) return 52;
        if (level == 20) return 102;
        if (level == 21) return 8;
        if (level == 22) return 51;
        if (level == 23) return 9;
        if (level == 24) return 22;
        if (level == 25) return 4;
        if (level == 26) return 16;
        if (level == 27) return 33;
        if (level == 28) return 55;
        if (level == 29) return 53;
        if (level == 30) return 111;
        if (level == 31) return 1000;
        if (level == 32) return 1000;
        if (level == 33) return 1000;
        if (level == 34) return 1000;
        if (level == 35) return 1000;
        if (level == 36) return 1000;
        if (level == 37) return 1000;
        if (level == 38) return 1000;
        if (level == 39) return 1000;
        if (level == 40) return 1000;
        return 0;
    }
}