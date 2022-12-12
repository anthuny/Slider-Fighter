using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPortraitTurnOrder : MonoBehaviour
{
    Image image;
    [SerializeField] private Image nextUnitArrow;
    private CanvasGroup cg;

    private void Awake()
    {
        image = GetComponent<Image>();
        cg = GetComponent<CanvasGroup>();
    }
    public void UpdatePortrait(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void UpdatePortraitColour(Color color)
    {
        image.color = color;
    }

    public void UpdateNextUnitArrow(bool toggle)
    {
        nextUnitArrow.enabled = toggle;
    }

    public void UpdateIconFade(int unitIndex)
    {
        cg.alpha = GameManager.instance.turnOrderIconAlphas[unitIndex];
    }
}
