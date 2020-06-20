using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] public AudioClip audioStep;
    [SerializeField] public AudioClip audioNewLevel;
    [SerializeField] public AudioClip audioDown;
    [SerializeField] public AudioClip audioButton;
    [SerializeField] public AudioClip audioTransporter;
    [SerializeField] public AudioClip audioTeleTransporterIn;
    [SerializeField] public AudioClip audioTeleTransporterOut;
    [SerializeField] public AudioClip audioCrystal;
    [SerializeField] public AudioClip audioCountDown;
    [SerializeField] public AudioClip audioCountDownEnd;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        if(audioSource) audioSource.PlayOneShot(audioClip);    
    }
}