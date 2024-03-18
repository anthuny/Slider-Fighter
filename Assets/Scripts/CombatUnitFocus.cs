using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitFocus : MonoBehaviour
{
    [SerializeField] private GameObject focusImageGO;
    [SerializeField] private UIElement focusImageUI;
    [SerializeField] private UIElement loadingTextUI;
    [SerializeField] private CanvasGroup cg;

    [SerializeField] private float startingFocusScale;
    [SerializeField] private float endingFocusScale;

    [SerializeField] private float growSpeed = 1;
    [SerializeField] private float alphaDecreaseInterval;
    [SerializeField] private float alphaDecreaseSpeed;

    public bool allowFocusGrow = false;
    public bool allowFocusExit = false;
    private bool allowFocusGrowStart = false;
    [SerializeField] private bool allowFadeAway = true;
    [SerializeField] private Transform innerTransform;
    [SerializeField] private bool doInnerExtra = false;
    public bool resetMap = false;
    public bool goToMap = false;
    public bool incMapFloor = false;
    public bool isMainTransition = false;
    public bool doneOnce;
    public bool doneOnce2;
    public bool doneOnce3;
    public bool doneOnce4;


    public float initialTimer;
    public float lastTimer;

    public void StartFocusTransition()
    {
        focusImageUI.UpdateAlpha(1);

        if (innerTransform)
            innerTransform.GetComponent<UIElement>().UpdateAlpha(1);

        allowFocusGrow = true;
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartFocusTransition();
        }

        // On allow grow, Set focus image to starter size, ONCE
        if (allowFocusGrow && !doneOnce)
        {
            //allowFocusGrow = false;
            doneOnce = true;
            doneOnce2 = false;
            doneOnce3 = false;
            doneOnce4 = false;

            focusImageGO.transform.localScale = new Vector2(endingFocusScale, endingFocusScale);
            
            if (doInnerExtra && innerTransform)
                innerTransform.localScale = new Vector2(0, 0);

            allowFocusGrowStart = true;
            initialTimer = 0;
            lastTimer = 0;

            cg.alpha = 0;

            if (isMainTransition)
                loadingTextUI.UpdateAlpha(1);
        }


        // Increase size of focus image, CONTINUESLY
        if (allowFocusGrowStart)
        {
            if (allowFocusGrow)
            {
                initialTimer += Time.deltaTime * growSpeed;

                // Decrease alpha after a set time
                if (initialTimer >= alphaDecreaseInterval)
                {
                    if (isMainTransition)
                        cg.alpha += Time.deltaTime * alphaDecreaseSpeed;
                    else
                        cg.alpha = 1;
                    //else
                    //{
                    //    if (initialTimer >= .85f)
                    //        cg.alpha -= Time.deltaTime * alphaDecreaseSpeed;
                    //}
                }
            }
            else
            {
                lastTimer += Time.deltaTime * growSpeed;

                // Decrease alpha after a set time
                if (lastTimer >= .35f)
                {
                    cg.alpha -= Time.deltaTime * alphaDecreaseSpeed;
                }
            }
        }

        if (!doneOnce2)
        {
            if (allowFocusGrow && doneOnce)
            {
                // Increase scale overtime
                focusImageGO.transform.localScale = Vector2.Lerp(new Vector2(startingFocusScale, startingFocusScale), new Vector2(endingFocusScale, endingFocusScale), initialTimer);

                if (doInnerExtra && innerTransform)
                    innerTransform.localScale = Vector2.Lerp(new Vector2(0, 0), new Vector2(50, 50), initialTimer / 4f);
            }
            else if (!allowFocusGrow && doneOnce)
            {
                // Increase scale overtime
                focusImageGO.transform.localScale = Vector2.Lerp(new Vector2(endingFocusScale, endingFocusScale), new Vector2(startingFocusScale, startingFocusScale), lastTimer /2f);

                if (doInnerExtra && innerTransform)
                    innerTransform.localScale = Vector2.Lerp(new Vector2(50, 50), new Vector2(0, 0), lastTimer * 2f);
            }
        }

        if (lastTimer >= 1 && !doneOnce2)
        {
            doneOnce = false;
            doneOnce2 = false;
            doneOnce3 = false;
            doneOnce4 = false;
            allowFocusGrowStart = false;
            incMapFloor = false;
            goToMap = false;

            initialTimer = 0;
            lastTimer = 0;

            cg.alpha = 0;

            focusImageGO.transform.localScale = new Vector2(0, 0);

            if (doInnerExtra && innerTransform)
                innerTransform.localScale = new Vector2(0, 0);
        }

        // If focus image growth is done, reset
        if (initialTimer >= 1 && !doneOnce3)
        {
            if (allowFocusGrowStart)
            {
                doneOnce3 = true;

                //allowFocusGrowStart = false;

                if (isMainTransition)
                {
                    if (incMapFloor)
                    {
                        GameManager.Instance.ToggleMap(true, true, true, true);
                    }
                    else if (goToMap)
                    {
                        GameManager.Instance.ToggleMap(true, false, false, true);
                    }
                    else if (!allowFadeAway && resetMap)
                    {
                        StartCoroutine("StartMenu");
                    }
                    else if (!resetMap)
                    {
                        if (!doneOnce4)
                        {
                            doneOnce4 = true;
                            GameManager.Instance.Setup();
                        }
                    }

                    StartCoroutine(HideLoadingText());

                    /*
                    focusImageGO.transform.localScale = new Vector2(0, 0);

                    if (doInnerExtra && innerTransform)
                        innerTransform.localScale = new Vector2(0, 0);
                    */
                }
                else
                {
                    initialTimer = 0;
                    allowFocusGrowStart = false;
                    cg.alpha = 0;
                }
            }
        }
    }

    public void AllowFadeOut()
    {
        /*
focusImageGO.transform.localScale = new Vector2(0, 0);

if (doInnerExtra && innerTransform)
    innerTransform.localScale = new Vector2(0, 0);
*/
        allowFocusGrow = false;
        initialTimer = 0;
        lastTimer = 0;
    }

    IEnumerator HideLoadingText()
    {
        yield return new WaitForSeconds(.5f);

        loadingTextUI.UpdateAlpha(0);
    }
    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(0);

        CharacterCarasel.Instance.ToggleMenu(false);
    }
}
