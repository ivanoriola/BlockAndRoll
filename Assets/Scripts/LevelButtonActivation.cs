using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonActivation : MonoBehaviour
{
    private GameObject player;
    private GameObject[] levelButtons;
    private int lastLevel;
    private int topLevel;

    void Start()
    {
        player = GameObject.Find(Constants.PLAYER_GAMEOBJECT);
        lastLevel = transform.childCount;

        levelButtons = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            levelButtons[i] = transform.GetChild(i).gameObject;
        }
    }
    void Update()
    {
        topLevel = player.GetComponent<LevelManager>().topLevel;
        for (int i = 0; i < lastLevel; i++)
        {
            if (i <= topLevel)
            {
                levelButtons[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                levelButtons[i].GetComponent<Button>().enabled = true;
                levelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else
            {
                levelButtons[i].transform.GetChild(0).GetComponent<Text>().color = new Color(0,0,0,0.25f);
                levelButtons[i].GetComponent<Button>().enabled = false;
                levelButtons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
            }
        }
    }
}
