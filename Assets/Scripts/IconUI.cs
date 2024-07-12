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
    [SerializeField] private GameObject rarityBGCommon;
    [SerializeField] private GameObject rarityBGRare;
    [SerializeField] private GameObject rarityBGEpic;
    [SerializeField] private GameObject rarityBGLegendary;
    public UIElement raceIcon;
    public UIElement mainIconBG;

    [SerializeField] private UIElement skillCooldownUI;

    public void UpdateMainIconBGColour(Color color)
    {
        mainIconBG.UpdateColour(color);
    }

    public void UpdateRarityBGColour(Color color)
    {
        rarityBG.UpdateColour(color);
    }

    public void ToggleRarityBG(bool toggle = true)
    {
        if (toggle)
            rarityBG.UpdateAlpha(1);
        else
            rarityBG.UpdateAlpha(0);
    }

    public string itemName;

    public void UpdateRaceIcon(Sprite sprite)
    {
        raceIcon.UpdateContentImage(sprite);
    }
    public void UpdateRarity(Rarity newRarity, bool disable = false)
    {
        curRarity = newRarity;

        rarityBGCommon.SetActive(false);
        rarityBGRare.SetActive(false);
        rarityBGEpic.SetActive(false);
        rarityBGLegendary.SetActive(false);

        if (curRarity == Rarity.COMMON)
        {
            UpdateRarityBGColour(ItemRewardManager.Instance.commonColour);
            rarityBGCommon.SetActive(true);
        }
        else if (curRarity == Rarity.RARE)
        {
            UpdateRarityBGColour(ItemRewardManager.Instance.rareColour);
            rarityBGRare.SetActive(true);
        }
        else if (curRarity == Rarity.EPIC)
        {
            UpdateRarityBGColour(ItemRewardManager.Instance.epicColour);
            rarityBGEpic.SetActive(true);
        }
        else if (curRarity == Rarity.LEGENDARY)
        {
            UpdateRarityBGColour(ItemRewardManager.Instance.legendaryColour);
            rarityBGLegendary.SetActive(true);
        }

        ToggleRarityBG(true);

        if (disable)
        {
            rarityBGCommon.SetActive(false);
            rarityBGRare.SetActive(false);
            rarityBGEpic.SetActive(false);
            rarityBGLegendary.SetActive(false);
            ToggleRarityBG(false);
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

        if (sprite == TeamItemsManager.Instance.clearSlotSprite)
        {
            UpdateRarity(Rarity.COMMON, true);
            UpdateRaceIcon(sprite);
            ToggleRarityBG(false);
        }
    }


    public void RemoveItemFromSlot()
    {
        //Debug.Log("Removing Item from Slot");

        UpdatePortrait(TeamItemsManager.Instance.clearSlotSprite);
        UpdatePassiveActiveType(false, true);
        UpdateItemName("");
        UpdateRaceIcon(TeamItemsManager.Instance.clearSlotSprite);

        //subText.GetComponentInChildren<UIElement>().UpdateAlpha(0);
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
            else if (level > 0)
            {
                //subText.GetComponentInChildren<UIElement>().UpdateAlpha(1);
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
