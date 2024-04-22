using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAlertText : MonoBehaviour
{
    public bool canMoveUp;
    private Vector3 startingPos;
    [SerializeField] private float hideSpeed;
    [SerializeField] private float travelSpeed;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private UIElement uiElement;
    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    bool movingUp = false;

    public void AllowMovingUp(string name, float alpha, bool effect, string gradient = null)
    {
        uiElement.UpdateContentText(name);
        uiElement.UpdateAlpha(alpha, false, 0, true);

        if (alpha != 0)
        {
            // Set correct text colour gradient
            if (effect)
            {
                if (gradient == "Inflict")
                    uiElement.UpdateContentTextColour(EffectManager.instance.gradientEffectAlert);
                else if (gradient == "Trigger")
                    uiElement.UpdateContentTextColour(EffectManager.instance.gradientEffectTrigger);
            }
            else
                uiElement.UpdateContentTextColour(GameManager.Instance.gradientSkillAlert);

            movingUp = true;

            StartCoroutine(WaitToStopMovingUp());
        }

    }

    float time = 0;
    bool startHiding = false;
    IEnumerator WaitToStopMovingUp()
    {
        yield return new WaitForSeconds(.8f);

        time = 0;

        startHiding = true;
    }

    void HideTextOvertime()
    {
        if (startHiding)
        {
            time += Time.deltaTime * hideSpeed;
            cg.alpha = 1 - time;

            if (cg.alpha <= 0)
            {
                startHiding = false;
                movingUp = false;

                Destroy(this.gameObject);
            }
        }
    }

    void Update()
    {
        MovingUp();
        HideTextOvertime();
    }

    void MovingUp()
    {
        if (movingUp)
        {
            float time = Time.deltaTime * travelSpeed;
            transform.localPosition += new Vector3(0, time, 0);
        }
    }

    public void ResetY()
    {
        transform.localPosition = startingPos;

        if (cg != null && canMoveUp)
            cg.alpha = 1;
    }
}
