using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IconUI : MonoBehaviour
{
    Image image;
    [SerializeField] private UIElement selectImage;
    [SerializeField] private UIElement hiddenImage;
    [SerializeField] private TextMeshProUGUI skillLevelText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdatePortrait(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void UpdateColour(Color colour)
    {
        image.color = colour;
    }

    public void UpdateSkillLevelText(int level)
    {
        skillLevelText.text = level.ToString();
    }
    public void ToggleSelectImage(bool toggle)
    {
        if (toggle)
            selectImage.UpdateAlpha(1);
        else
            selectImage.UpdateAlpha(0);
    }

    public void ToggleHiddenImage(bool toggle)
    {
        if (toggle)
            hiddenImage.UpdateAlpha(1);
        else
            hiddenImage.UpdateAlpha(0);
    }
}
