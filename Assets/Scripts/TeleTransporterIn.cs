using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleTransporterIn : MonoBehaviour
{
    [SerializeField] GameObject target;
    private GameObject player;
    private bool activated = false;
    private bool initialActivated = false;
    private Vector3 initialPosition;
    SoundManager soundManager;

    private static string TELETRANSPORT_COROUTINE = "TeleTransport";
    private void Awake()
    {
        initialPosition = transform.position;
        initialActivated = activated;
    }
    private void OnEnable()
    {
        transform.position = initialPosition;
        activated = initialActivated;
    }
    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG) && !other.GetComponent<Player>().moving)
        {
            if (!activated)
            {
                other.GetComponent<Player>().enabled = false;
                StartCoroutine(TELETRANSPORT_COROUTINE);
                activated = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            activated = false;
        }
    }
    IEnumerator TeleTransport()
    {
        soundManager.Play(soundManager.audioTeleTransporterIn);
        for (int i = 0; i < 2; i++)
        {
            MakePlayerVisible(true);
            yield return new WaitForSeconds(.15f);
            MakePlayerVisible(false);
            yield return new WaitForSeconds(.15f);
        }
        yield return new WaitForSeconds(.5f);
        player.transform.position = target.transform.position;
        
        soundManager.Play(soundManager.audioTeleTransporterOut);
        for (int i = 0; i < 2; i++)
        {
            MakePlayerVisible(false);
            yield return new WaitForSeconds(.15f);
            MakePlayerVisible(true);
            yield return new WaitForSeconds(.15f);
        }
        GameObject.Find(Constants.PLAYER_GAMEOBJECT).GetComponent<Player>().enabled = true;
    }
    private void MakePlayerVisible(bool visible)
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }

}