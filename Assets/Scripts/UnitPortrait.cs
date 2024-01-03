using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPortrait : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image nextUnitArrow;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private CanvasGroup bgCg;

    public UIElement uIElement;

    private void Awake()
    {
        uIElement = GetComponent<UIElement>();
    }

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        UpdateNextUnitArrow(false);
        //ToggleBg(false);
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
        cg.alpha = GameManager.Instance.turnOrderIconAlphas[unitIndex];
    }

    public void ToggleBg(bool toggle)
    {
        if (toggle)
            bgCg.alpha = .05f;
        else
            bgCg.alpha = 0;
    }
}
