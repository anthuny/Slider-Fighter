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
    [SerializeField] private TextMeshProUGUI subText;
    [SerializeField] private TextMeshProUGUI mainIconPassiveActiveIcon;


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

    public void RemoveItemFromSlot()
    {
        Debug.Log("Removing Item from Slot");

        UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
        UpdatePassiveActiveType(false, true);
    }

    public void UpdateSubText(int level, bool item = false)
    {
        if (item)
        {
            subText.text = level.ToString();

            if (level == 0)
            {
                subText.text = "";
            }

            return;
        }

        subText.text = level.ToString();
    }

    public void UpdatePassiveActiveType(bool active = true, bool clear = false)
    {
        if (active)
        {
            mainIconPassiveActiveIcon.text = "A";
            mainIconPassiveActiveIcon.color = GameManager.Instance.activeSkillColour;
        }
        else
        {
            mainIconPassiveActiveIcon.text = "P";
            mainIconPassiveActiveIcon.color = GameManager.Instance.passiveSkillColour;
        }

        if (clear)
        {
            mainIconPassiveActiveIcon.text = "";
        }
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
