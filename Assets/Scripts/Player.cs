using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum Direccion
    {
        up,
        down,
        left,
        right,
    }
    private Vector3 axisLocation;
    private float moveDelay; //Switch (Application.platform)
    private int moveSteps;
    private int angleAdjust = 90;
    private float turnAngle;
    private int gridAdjust = 1;

    public bool moving = false;
    public bool waiting = false;
    public int moves = 0;

    [SerializeField] private Sensor UpSensorWall;
    [SerializeField] private Sensor UpSensorFloor;
    [SerializeField] private Sensor DownSensorWall;
    [SerializeField] private Sensor DownSensorFloor;
    [SerializeField] private Sensor LeftSensorWall;
    [SerializeField] private Sensor LeftSensorFloor;
    [SerializeField] private Sensor RightSensorWall;
    [SerializeField] private Sensor RightSensorFloor;

    private Touch touch;
    private Vector3 startPosition;
    private float swipeX;
    private float swipeY;
    public float speedAdjustment;

    SoundManager soundManager;

    private static string MOVER_COROUTINE = "Mover";
    private void Start()
    {
        soundManager = GameObject.Find(Constants.SOUNDMANAGER_GAMEOBJECT).GetComponent<SoundManager>();
        //switch (Application.platform)
        //{
        //    case RuntimePlatform.Android:
                moveSteps = 6;
                turnAngle = angleAdjust / moveSteps;
        //        break;
        //    case RuntimePlatform.WindowsEditor:
        //        moveDelay = 2;
        //        moveSteps = 6;
        //        turnAngle = angleAdjust / moveSteps;
        //        break;
        //    case RuntimePlatform.WindowsPlayer:
        //        moveDelay = 2;
        //        moveSteps = 8;
        //        turnAngle = angleAdjust / moveSteps;
        //        break;
        //    default:
        //        moveDelay = 2;
        //        moveSteps = 6;
        //        turnAngle = angleAdjust / moveSteps;
        //        break;
        //}
    }
    private void Update()
    {
        //switch (Application.platform)
        //{
        //    case RuntimePlatform.Android:
                GetTouchInput();
        //        break;
        //    case RuntimePlatform.WindowsEditor:
        //        GetKeyImput();
        //        GetMouseInput();
        //        break;
        //    case RuntimePlatform.WindowsPlayer:
        //        GetKeyImput();
        //        GetMouseInput();
        //        break;
        //    default:
        //        GetKeyImput();
        //        GetMouseInput();
        //        GetTouchInput();
        //        break;
        //}
    }
    void GetTouchInput()
    {
        if (!moving)
        {
            Touch[] touches = Input.touches;
            if (touches.Length > 0)
            {
                touch = touches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPosition = touch.position;
                        break;
                    case TouchPhase.Ended:
                        swipeX = startPosition.x - touch.position.x;
                        swipeY = startPosition.y - touch.position.y;
                        speedAdjustment = 500 / Mathf.RoundToInt(Mathf.Abs(touch.deltaPosition.magnitude));
                        moveDelay = 2 - speedAdjustment;
                        if (Mathf.Abs(swipeY) > Mathf.Abs(swipeX))
                        {
                            if (swipeY < 0)
                            {
                                IniciarMov(Direccion.up);
                            }
                            else
                            {
                                IniciarMov(Direccion.down);
                            }
                        }
                        else
                        {
                            if (swipeX < 0)
                            {
                                IniciarMov(Direccion.right);
                            }
                            else
                            {
                                IniciarMov(Direccion.left);
                            }
                        }
                        break;
                }
            }
        }
    }
    //void GetMouseInput()
    //{
    //    if (!moving)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            startPosition = Input.mousePosition;
    //        }
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            swipeX = startPosition.x - Input.mousePosition.x;
    //            swipeY = startPosition.y - Input.mousePosition.y;
    //            if (Mathf.Abs(swipeY) > Mathf.Abs(swipeX))
    //            {
    //                if (swipeY < 0)
    //                {
    //                    IniciarMov(Direccion.up);
    //                }
    //                else
    //                {
    //                    IniciarMov(Direccion.down);
    //                }
    //            }
    //            else
    //            {
    //                if (swipeX < 0)
    //                {
    //                    IniciarMov(Direccion.right);
    //                }
    //                else
    //                {
    //                    IniciarMov(Direccion.left);
    //                }
    //            }
    //        }
    //    }
    //}
    //private void GetKeyImput()
    //{
    //    if (!moving)
    //    {
    //        if (Input.GetAxis(Constants.INPUT_VERTICAL) > 0)
    //        {
    //            IniciarMov(Direccion.up);
    //        }
    //        else if (Input.GetAxis(Constants.INPUT_VERTICAL) < 0)
    //        {
    //            IniciarMov(Direccion.down);
    //        }
    //        else if (Input.GetAxis(Constants.INPUT_HORIZONTAL) > 0)
    //        {
    //            IniciarMov(Direccion.right);
    //        }
    //        else if (Input.GetAxis(Constants.INPUT_HORIZONTAL) < 0)
    //        {
    //            IniciarMov(Direccion.left);
    //        }
    //    }
    //}
    private void IniciarMov(Direccion dir)
    {
        if (!waiting && TestMovement(dir))
        {
            moving = true;
            locateAxis(dir);
            StartCoroutine(MOVER_COROUTINE, dir);
        }
    }
    private bool TestMovement(Direccion dir)
    {
        if (dir == Direccion.up)
        {
            return (!UpSensorWall.wallDetection && UpSensorFloor.floorDetection);
        }
        else if (dir == Direccion.down)
        {
            return (!DownSensorWall.wallDetection && DownSensorFloor.floorDetection);
        }
        else if (dir == Direccion.left)
        {
            return (!LeftSensorWall.wallDetection && LeftSensorFloor.floorDetection);
        }
        else if (dir == Direccion.right)
        {
            return (!RightSensorWall.wallDetection && RightSensorFloor.floorDetection);
        }
        return false;
    }
    private void locateAxis(Direccion dir)
    {
        if (dir == Direccion.up)
        {
            axisLocation = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z + .5f);
        }
        if (dir == Direccion.down)
        {
            axisLocation = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z - .5f);
        }
        if (dir == Direccion.left)
        {
            axisLocation = new Vector3(transform.position.x - .5f, transform.position.y - .5f, transform.position.z);
        }
        if (dir == Direccion.right)
        {
            axisLocation = new Vector3(transform.position.x + .5f, transform.position.y - .5f, transform.position.z);
        }
    }
    private IEnumerator Mover(Direccion dir)
    {
        for (int i = 0; i < moveSteps; i++)
        {
            if (dir == Direccion.up)
            {
                transform.RotateAround(axisLocation, new Vector3(1, 0, 0), turnAngle);
            }
            if (dir == Direccion.down)
            {
                transform.RotateAround(axisLocation, new Vector3(1, 0, 0), -1 * turnAngle);
            }
            if (dir == Direccion.left)
            {
                transform.RotateAround(axisLocation, new Vector3(0, 0, 1), turnAngle);
            }
            if (dir == Direccion.right)
            {
                transform.RotateAround(axisLocation, new Vector3(0, 0, 1), -1 * turnAngle);
            }
            yield return new WaitForSeconds(moveDelay * Time.deltaTime);
        }
        moving = false;
        GridAdjustment();
        soundManager.Play(soundManager.audioStep);
        moves++;
    }
    public void GridAdjustment()
    {
        float newRotX = angleAdjust * Mathf.Round(transform.eulerAngles.x / angleAdjust);
        float newRotY = angleAdjust * Mathf.Round(transform.eulerAngles.y / angleAdjust);
        float newRotZ = angleAdjust * Mathf.Round(transform.eulerAngles.z / angleAdjust);
        transform.rotation = Quaternion.Euler(newRotX, newRotY, newRotZ);

        float newX = gridAdjust * Mathf.Round(transform.position.x / gridAdjust);
        float newY = gridAdjust * Mathf.Round(transform.position.y / gridAdjust);
        float newZ = gridAdjust * Mathf.Round(transform.position.z / gridAdjust);
        transform.position = new Vector3(newX, newY, newZ);
    }
    public void Resetmoves()
    {
        moves = 0;
    }
}