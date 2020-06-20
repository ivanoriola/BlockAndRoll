using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingUnvanishingFloor : MonoBehaviour
{
    SoundManager soundManager;
    private static string UNVANISH_COROUTINE = "Unvanish";

    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.tag = Constants.UNTAGGED_TAG;
            Invoke(UNVANISH_COROUTINE, 3);
        }
    }
    private void Unvanish()
    {
        soundManager.Play(soundManager.audioCrystal);
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.tag = Constants.FLOOR_TAG;
    }
}