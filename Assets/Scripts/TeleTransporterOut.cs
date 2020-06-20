using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleTransporterOut : MonoBehaviour
{
    private Vector3 initialPosition;
    private void Awake()
    {
        initialPosition = transform.position;
    }
    private void OnEnable()
    {
        transform.position = initialPosition;
    }

}
