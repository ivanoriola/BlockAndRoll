using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonTarget : MonoBehaviour
{
    [SerializeField] public ButtonObject button;
    public bool activated;
    private bool initialActivated;

    public SoundManager soundManager;

    public virtual void Awake()
    {
        initialActivated = activated;
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
    }
    public virtual void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        activated = initialActivated;
    }
    void Update()
    {
        if (activated)
        {
            ActivatedAction();
        }
        if (!activated)
        {
            DeactivatedAction();
        }
    }
    public void MakeVisible(bool visible)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
    public abstract void ActivatedAction();
    public abstract void DeactivatedAction();

}