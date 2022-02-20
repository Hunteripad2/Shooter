using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingScreen : MonoBehaviour
{
    [HideInInspector] private Color currentColor;

    [Header("General")]
    [SerializeField] private Image screen;
    [SerializeField] public float fadingSpeed = 5f;
    [SerializeField] public bool shouldFade;

    void Update()
    {
        if (!shouldFade && screen.color.a > 0f)
        {
            currentColor = screen.color;
            currentColor.a = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * fadingSpeed);
            screen.color = currentColor;
        }
        else if (shouldFade && screen.color.a < 1f)
        {
            screen.gameObject.SetActive(true);
            currentColor = screen.color;
            currentColor.a = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * fadingSpeed);
            screen.color = currentColor;
        }

        if (screen.color.a == 0f)
        {
            screen.gameObject.SetActive(false);
        }
    }
}
