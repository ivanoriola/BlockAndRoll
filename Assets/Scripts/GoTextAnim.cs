using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoTextAnim : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool activated = false;
    private bool initialActivated;
    private static string ANIMATE_COROUTINE = "Animate";
    private void Awake()
    {
        initialPosition = transform.position;
        initialActivated = activated;
    }
    private void OnEnable()
    {
        transform.position = initialPosition;
        activated = initialActivated;
    }
    void Update()
    {
        if (!activated)
        {
            StartCoroutine(ANIMATE_COROUTINE);
            activated = true;
        }
    }
    IEnumerator Animate()
    {
        while (true)
        {
            float delay = .25f;
            float steps = 2f;
            for (int i = 0; i < steps * 2; i++)
            {
                transform.Translate(Vector3.right * .5f);
                yield return new WaitForSeconds(delay);
            }
            string originalText = gameObject.GetComponent<TextMesh>().text;
            gameObject.GetComponent<TextMesh>().text = "";
            yield return new WaitForSeconds(2 * delay);
            transform.Translate(Vector3.left * steps);
            gameObject.GetComponent<TextMesh>().text = originalText;
            yield return new WaitForSeconds(delay);
        }
    }
}