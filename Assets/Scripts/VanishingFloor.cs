using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingFloor : MonoBehaviour 
{
    private void OnEnable()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.tag = Constants.FLOOR_TAG;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.tag = Constants.UNTAGGED_TAG;
        }
    }
}