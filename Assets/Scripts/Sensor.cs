using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool wallDetection;
    public bool floorDetection;
    private void OnTriggerExit(Collider other)
    {
        floorDetection = false;
        wallDetection = false;
    }
    private void OnTriggerStay(Collider other)
    {
        floorDetection = other.gameObject.CompareTag(Constants.FLOOR_TAG);
        wallDetection = other.gameObject.CompareTag(Constants.WALL_TAG);
    }
}