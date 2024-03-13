using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitFocus : MonoBehaviour
{
    [SerializeField] private GameObject focusImageGO;
    [SerializeField] private UIElement focusImageUI;
    [SerializeField] private CanvasGroup cg;

    [SerializeField] private float startingFocusScale;
    [SerializeField] private float endingFocusScale;

    [SerializeField] private float growSpeed = 1;
    [SerializeField] private float alphaDecreaseInterval;
    [SerializeField] private float alphaDecreaseSpeed;

    private bool allowFocusGrow = false;
    private bool allowFocusGrowStart = false;
    [SerializeField] private bool allowFadeAway = true;
    [SerializeField] private Transform innerTransform;
    [SerializeField] private bool doInnerExtra = false;
    public bool resetMap = false;
    public bool doneOnce;



    float timer;

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
        if (allowFocusGrow)
        {
            allowFocusGrow = false;
            doneOnce = false;

            focusImageGO.transform.localScale = new Vector2(startingFocusScale, startingFocusScale);
            
            if (doInnerExtra && innerTransform)
                innerTransform.localScale = new Vector2(0, 0);

            allowFocusGrowStart = true;
            timer = 0;
            cg.alpha = 0;
        }


        // Increase size of focus image, CONTINUESLY
        if (allowFocusGrowStart)
        {
            timer += Time.deltaTime * growSpeed;

            // Decrease alpha after a set time
            if (timer >= alphaDecreaseInterval)
            {
                cg.alpha += Time.deltaTime * alphaDecreaseSpeed;
            }
        }

        // Increase scale overtime
        focusImageGO.transform.localScale = Vector2.Lerp(new Vector2(startingFocusScale, startingFocusScale), new Vector2(endingFocusScale, endingFocusScale), timer);
        
        if (doInnerExtra && innerTransform)
            innerTransform.localScale = Vector2.Lerp(new Vector2(0, 0), new Vector2(50, 50), timer/5f);

        // If focus image growth is done, reset
        if (focusImageGO.transform.localScale.x == endingFocusScale)
        {
            if (allowFocusGrowStart)
            {
                allowFocusGrowStart = false;

                if (!allowFadeAway && resetMap)
                {
                    StartCoroutine("StartMenu");
                }
                else if (!resetMap)
                {
                    focusImageUI.UpdateAlpha(0);
                    GameManager.Instance.DisableTransitionSequence();

                    if (innerTransform)
                        innerTransform.GetComponent<UIElement>().UpdateAlpha(0);

                    if (doInnerExtra && innerTransform)
                        innerTransform.localScale = new Vector2(0, 0);

                    /*
                    if (!doneOnce)
                    {
                        doneOnce = true;
                        //GameManager.Instance.StartRoom(RoomManager.Instance.GetActiveRoom(), RoomManager.Instance.GetActiveFloor());
                    }
                    */
                }      
            }
        }
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(0);

        CharacterCarasel.Instance.ToggleMenu(false);
    }
}
