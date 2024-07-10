using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IconUI : MonoBehaviour
{
    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    [SerializeField] private UIElement mainSpriteIcon;
    [SerializeField] private UIElement selectImage;
    [SerializeField] private UIElement hiddenImage;
    [SerializeField] public TextMeshProUGUI subText;
    [SerializeField] private TextMeshProUGUI mainIconPassiveActiveIcon;
    public UIElement rarityBG;
    [SerializeField] private UIElement rarityBGCommon;
    [SerializeField] private UIElement rarityBGRare;
    [SerializeField] private UIElement rarityBGEpic;
    [SerializeField] private UIElement rarityBGLegendary;
    public UIElement raceIcon;

    [SerializeField] private UIElement skillCooldownUI;

    public string itemName;

    public void UpdateRaceIcon(Sprite sprite)
    {
        raceIcon.UpdateContentImage(sprite);
    }
    public void UpdateRarity(Rarity newRarity, bool disable = false)
    {
        curRarity = newRarity;

        rarityBGCommon.UpdateAlpha(0);
        rarityBGRare.UpdateAlpha(0);
        rarityBGEpic.UpdateAlpha(0);
        rarityBGLegendary.UpdateAlpha(0);

        if (curRarity == Rarity.COMMON)
            rarityBGCommon.UpdateAlpha(1);
        else if (curRarity == Rarity.RARE)
            rarityBGRare.UpdateAlpha(1);
        else if (curRarity == Rarity.EPIC)
            rarityBGEpic.UpdateAlpha(1);
        else if (curRarity == Rarity.LEGENDARY)
            rarityBGLegendary.UpdateAlpha(1);

        if (disable)
        {
            rarityBGCommon.UpdateAlpha(0);
            rarityBGRare.UpdateAlpha(0);
            rarityBGEpic.UpdateAlpha(0);
            rarityBGLegendary.UpdateAlpha(0);
        }
    }

    public Rarity GetCurRarity()
    {
        return curRarity;
    }

    public void ToggleSkillCooldownUI(bool toggle = true)
    {
        if (toggle)
        {
            skillCooldownUI.UpdateAlpha(1);
        }
        else
        {
            skillCooldownUI.UpdateAlpha(0);
        }
    }

    private void Awake()
    {
    }

    public void UpdateItemName(string name)
    {
        itemName = name;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void UpdatePortrait(Sprite sprite)
    {
        mainSpriteIcon.UpdateContentImage(sprite);
    }


    public void RemoveItemFromSlot()
    {
        //Debug.Log("Removing Item from Slot");

        UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
        UpdatePassiveActiveType(false, true);
        UpdateItemName("");
    }


    public void UpdateSubText(int level, bool item = false, bool passive = true, bool remove = true)
    {
        if (item)
        {
            subText.text = level.ToString();

            if (level <= 0)
            {
                subText.text = "";

                if (!passive && remove)
                    RemoveItemFromSlot();
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
