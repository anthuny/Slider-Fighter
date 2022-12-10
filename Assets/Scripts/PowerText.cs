using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerText : MonoBehaviour
{
    Text mainText;
    [SerializeField] public float floatSpeed;
    [SerializeField] private float timeTillFadeStarts;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        mainText = GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(StartFadeCountdown());
    }

    IEnumerator StartFadeCountdown()
    {
        yield return new WaitForSeconds(timeTillFadeStarts);

        StartCoroutine(FadeImage(true));
    }
    private void Update()
    {
        transform.Translate(Vector2.up * floatSpeed * Time.deltaTime);    
    }

    public void UpdateFloatSpeed(float speed)
    {
        floatSpeed = speed;
    }

    public void UpdatePowerText(string power)
    {
        mainText.text = power;
    }

    public void UpdatePowerTextColour(Color color)
    {
        mainText.color = color;
    }

    public void UpdatePowerTextFontSize(int newFontSize)
    {
        mainText.fontSize = newFontSize;
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = fadeDuration; i >= 0; i -= Time.deltaTime * fadeSpeed)
            {
                // set color with i as alpha
                mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, i);

                // When completely faded out, destroy it
                if (i <= 0)
                    RemovePowerText();

                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= fadeDuration; i += Time.deltaTime * fadeSpeed)
            {
                // set color with i as alpha
                mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, i);
            }
        }
    }

    void RemovePowerText()
    {
        Destroy(gameObject);
    }
}
