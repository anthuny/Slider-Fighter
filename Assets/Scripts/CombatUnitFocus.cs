using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitFocus : MonoBehaviour
{
    [SerializeField] private GameObject focusImageGO;
    [SerializeField] private UIElement focusImageUI;
    [SerializeField] private CanvasGroup cg;

    [SerializeField] private int startingFocusScale;
    [SerializeField] private int endingFocusScale;

    [SerializeField] private float growSpeed = 1;
    [SerializeField] private float alphaDecreaseInterval;
    [SerializeField] private float alphaDecreaseSpeed;

    private bool allowFocusGrow = false;
    private bool allowFocusGrowStart = false;




    float timer;

    public void FocusUnit()
    {
        focusImageUI.UpdateAlpha(1);

        allowFocusGrow = true;
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FocusUnit();
        }

        // On allow grow, Set focus image to starter size, ONCE
        if (allowFocusGrow)
        {
            allowFocusGrow = false;
            focusImageGO.transform.localScale = new Vector2(startingFocusScale, startingFocusScale);

            allowFocusGrowStart = true;
            timer = 0;
            cg.alpha = 1;
        }


        // Increase size of focus image, CONTINUESLY
        if (allowFocusGrowStart)
        {
            timer += Time.deltaTime * growSpeed;

            // Decrease alpha after a set time
            if (timer >= alphaDecreaseInterval)
                cg.alpha -= Time.deltaTime * alphaDecreaseSpeed;

            // Increase scale overtime
            focusImageGO.transform.localScale = Vector2.Lerp(new Vector2(startingFocusScale, startingFocusScale), new Vector2(endingFocusScale, endingFocusScale), timer);



            // If focus image growth is done, reset
            if (cg.alpha == 0)
            {
                allowFocusGrowStart = false;
                focusImageUI.UpdateAlpha(0);
            }
        }
    }

}
