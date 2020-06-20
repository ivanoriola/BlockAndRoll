using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTarget_TimingDoor : ButtonTarget
{
    [SerializeField] TextMesh doorTextFront;
    [SerializeField] TextMesh doorTextBack;
    [SerializeField] int countdownTime;
    private int countdown;
    private enum Estado
    {
        Up,
        Down
    }
    Estado estado = Estado.Down;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    public override void OnEnable()
    {
        doorTextFront.text = "";
        doorTextBack.text = "";
        estado = Estado.Down;
        base.OnEnable();
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
    public override void DeactivatedAction()
    {
        gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_OFF;
        countdown = 0;
    }
    public override void ActivatedAction()
    {
        gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_ON;
        if (estado == Estado.Down)
        {
            estado = Estado.Up;
            //gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine("GoUp");
        }
    }
    IEnumerator GoUp()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.position += Vector3.up * 0.25f;
            yield return new WaitForSeconds(4 * Time.deltaTime);
        }
        countdown = countdownTime;
        while (countdown > 0)
        {
            soundManager.Play(soundManager.audioCountDown);
            doorTextFront.text = "" + countdown;
            doorTextBack.text = "" + countdown;
            countdown--;
            yield return new WaitForSeconds(1);
        }
        soundManager.Play(soundManager.audioCountDownEnd);
        StartCoroutine("GoDown");
        button.GetComponent<ButtonObject>().activated = false;
    }
    IEnumerator GoDown()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.position += Vector3.down * 0.25f;
            yield return new WaitForSeconds(4 * Time.deltaTime);
        }
        estado = Estado.Down;
        doorTextFront.text = "";
        doorTextBack.text = "";
    }
}
