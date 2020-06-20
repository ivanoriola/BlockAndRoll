using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool levelChanged = false;
    GameObject player;
    GameObject uiScripts;
    private bool visible = false;
    private bool triggered = false;
    private bool tempAudioState;
    private bool tempMusicState;

    private static string LEVELCHANGE_COROUTINE = "LevelChange";
    private static string MAKEVISIBLE_VOID = "MakeVisible";

    private void OnEnable()
    {
        visible = true; MakeVisible();
        levelChanged = false;
        triggered = false;
    }
    private void Start()
    {
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
        uiScripts = GameObject.Find(Constants.UI_GAMEOBJECT);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !triggered)
        {
            triggered = true;
            player.GetComponent<LevelManager>().goalReached = true;
            StartCoroutine("LevelChange");

        }
    }
    IEnumerator LevelChange()
    {
        player.GetComponent<Player>().waiting = true;
        do { yield return null; }
        while (player.GetComponent<Player>().moving);
        if (!levelChanged)
        {
            player.GetComponent<LevelManager>().level += 1;
            levelChanged = true;
            player.GetComponent<LevelManager>().go = true;
            triggered = false;
            visible = false; Invoke(MAKEVISIBLE_VOID, .5f);
            player.GetComponent<LevelManager>().goalReached = false;
        }
    }
    private void MakeVisible()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}