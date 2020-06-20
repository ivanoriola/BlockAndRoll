using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    Transform target1;
    Transform target2;

    private Vector3 offset;
    private float offsetX;
    public float offsetY; //20
    public float offsetZ; //-20
    public float smoothTime; //.25
    public float minZoom; //40
    public float maxZoom; //10
    public float zommLimiter; //25

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        newTargets(GameObject.Find(Constants.PLAYER_GAMEOBJECT).transform, GameObject.Find(Constants.PLAYER_GAMEOBJECT).transform);
    }
    public void newTargets(Transform target1, Transform target2)
    {
        this.target1 = target1;
        this.target2 = target2;
    }
    private void LateUpdate()
    {
        offsetX = -8 * Input.acceleration.x;
        offset = new Vector3(offsetX, offsetY, offsetZ);
        if (!target1 && !target2)
            return;
        Move();
        Zoom();
        Quaternion targetRotation = Quaternion.LookRotation(target1.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
    }
    private void Zoom()
    {
        float distance = Vector3.Distance(target1.position, target2.position);
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zommLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    private void Move()
    {
        Vector3 newPosition = target1.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
}
