using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] ButtonTarget buttonTarget;
    public bool activated;
    private bool initialActivated;
    SoundManager soundManager;

    private void Awake()
    {
        initialActivated = activated;
    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        activated = initialActivated;
    }
    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
    }
    void Update()
    {
        if (activated)
        {
            gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_ON;
        }
        if (!activated)
        {
            gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_OFF;
        }
        buttonTarget.GetComponent<ButtonTarget>().activated = activated;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            activated = !activated;
            soundManager.Play(soundManager.audioButton);

        }
    }
}
