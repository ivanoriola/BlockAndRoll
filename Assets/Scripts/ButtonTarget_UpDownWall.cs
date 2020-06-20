using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTarget_UpDownWall : ButtonTarget
{
    private enum Estado
    {
        Up,
        Down
    }
    Estado estado = Estado.Up;

    public override void  DeactivatedAction()
    {
        gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_OFF;
        if (estado == Estado.Down)
        {
            estado = Estado.Up;
            gameObject.GetComponent<Collider>().enabled = true;
            StartCoroutine("GoUp");
        }
    }
    public override void ActivatedAction()
    {
        gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_ON;
        if (estado == Estado.Up)
        {
            estado = Estado.Down;
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine("GoDown");
        }
    }
    IEnumerator GoDown()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.position += Vector3.down * 0.25f;
            yield return new WaitForSeconds(4 * Time.deltaTime);
        }
        //gameObject.GetComponent<Renderer>().enabled = false;
        MakeVisible(false);
    }
    IEnumerator GoUp()
    {
        //gameObject.GetComponent<Renderer>().enabled = true;
        MakeVisible(true);
        for (int i = 0; i < 4; i++)
        {
            transform.position += Vector3.up * 0.25f;
            yield return new WaitForSeconds(4 * Time.deltaTime);
        }
    }
}
