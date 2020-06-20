using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorsFollowPlayer : MonoBehaviour 
{
    GameObject player;
    private void Start()
    {
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}