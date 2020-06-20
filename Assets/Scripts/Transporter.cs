using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] int steps;
    float delay;
    private bool activated = false;
    private bool initialActivated = false;
    private bool exit = false;
    private bool initialExit = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    SoundManager soundManager;

    private static string GOFORWARD_COROUTINE = "GoForward";

    private void Awake()
    {
        initialActivated = activated;
        initialExit = exit;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        activated = initialActivated;
        exit = initialExit;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
        delay = 4 / steps * Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG) && !other.GetComponent<Player>().moving)
        {
            other.gameObject.transform.SetParent(transform);
            if (!activated)
            {
                other.GetComponent<Player>().enabled = false;
                StartCoroutine(GOFORWARD_COROUTINE);
                activated = true;
                exit = false;
                soundManager.Play(soundManager.audioTransporter);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!exit && other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            other.gameObject.transform.SetParent(null);
            transform.Rotate(Vector3.right, 180);
            activated = false;
            exit = true;
        }
    }
    IEnumerator GoForward()
    {
        for (int i = 0; i < 4 * steps; i++)
        {
            transform.Translate(Vector3.forward * 0.25f);
            yield return new WaitForSeconds(delay);
        }
        GameObject.Find(Constants.PLAYER_GAMEOBJECT).GetComponent<Player>().enabled = true;
    }
}

