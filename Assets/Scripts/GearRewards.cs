using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearRewards : MonoBehaviour
{
    public static GearRewards Instance;

    [SerializeField] private float timeInbetweenRewards = .2f;
    [SerializeField] private Transform recievedGearParent;
    [SerializeField] private UIElement uiElement;
    [SerializeField] private UIElement rewardsTextUI;
    public GameObject gearGO;
    public GameObject itemGO;
    public GameObject goldRewardGO;
    [SerializeField] private Sprite goldSprite;

    [Tooltip("% chance of a unit dropping 1 piece of gear (Flip the %)")]
    [SerializeField] private int unitDropRatePerc;

    [Space(5)]
    [Header("Gear Rarity")]

    [Tooltip("% chance of gear dropping as rarity Common")]
    [SerializeField] private int gearCommonPerc;
    [Tooltip("% chance of gear dropping as rarity Rare")]
    [SerializeField] private int gearRarePerc;
    [Tooltip("% chance of gear dropping as rarity Epic")]
    [SerializeField] private int gearEpicPerc;
    [Tooltip("% chance of gear dropping as rarity Legendary")]
    [SerializeField] private int gearLegendaryPerc;

    [Space(5)]
    [SerializeField] private List<GearPiece> allGearPiecesCommon = new List<GearPiece>();
    [Space(3)]
    [SerializeField] private List<GearPiece> allGearPiecesRare = new List<GearPiece>();
    [Space(3)]
    [SerializeField] private List<GearPiece> allGearPiecesEpic = new List<GearPiece>();
    [Space(3)]
    [SerializeField] private List<GearPiece> allGearPiecesLegendary = new List<GearPiece>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ToggleGearRewardsTab(false);

        Setup();
    }

    public void ResetRewards()
    {
        ResetRewardsTable();
        rewardsTextUI.UpdateAlpha(0);
        ToggleGearRewardsTab(false);
    }

    public void Setup()
    {
        ResetRewards();

        rewardsTextUI.UpdateAlpha(1);
        ToggleGearRewardsTab(true);

        ResetRewardsTable();
    }

    public void ToggleGearRewardsTab(bool toggle)
    {
        if (uiElement)
        {
            if (toggle)
            {
                uiElement.UpdateAlpha(1);

                rewardsTextUI.UpdateAlpha(1);
            }
            else
            {
                uiElement.UpdateAlpha(0);
            }
        }
    }

    public void FillGearRewardsTable()
    {
        Setup();

        // Give Gold
        StartCoroutine(GiveGold());

        // Give Item and Gear
        StartCoroutine(GiveRewards());

        GameManager.Instance.ResetEnemiesKilledCount();
    }

    public void ResetRewardsTable()
    {
        foreach (Transform child in recievedGearParent)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator GiveRewards()
    {
        int count = GameManager.Instance.GetEnemiesKilledCount();
        bool spawnedItem = false; 

        // Check if EACH defeated enemy drops an Gear
        for (int i = 0; i < count; i++)
        {
            // If max rewards have been given, do not give more
            if (i == 4)
                break;

            // If item rewards are NOT disabled
            // Ensure spawned item only spawns once
            if (!ItemRewardManager.Instance.disableItemRewards && !spawnedItem)
            {
                spawnedItem = true;

                yield return new WaitForSeconds(timeInbetweenRewards);

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                GameObject go = Instantiate(ItemRewardManager.Instance.itemGO, recievedGearParent.position, Quaternion.identity);
                go.transform.SetParent(recievedGearParent);
                go.transform.localScale = new Vector2(1, 1);

                UIElement uIElement = go.GetComponent<UIElement>();

                // Set item
                uIElement.UpdateContentImage(ItemRewardManager.Instance.selectedItem.itemSprite);
                uIElement.UpdateItemName(ItemRewardManager.Instance.selectedItem.itemName);

                if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.LEGENDARY)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.legendaryColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.EPIC)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.epicColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.RARE)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.rareColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == ItemPiece.Rarity.COMMON)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.commonColour);
                }

                uIElement.ToggleButton(false);
            }

            int gearChance = Random.Range(0, 101);

            // Roll if Gear drops
            if (gearChance >= unitDropRatePerc)
            {
                yield return new WaitForSeconds(timeInbetweenRewards);

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");

                GameObject go = Instantiate(gearGO, recievedGearParent.position, Quaternion.identity);
                go.transform.SetParent(recievedGearParent);
                go.transform.localScale = new Vector2(1, 1);

                Slot slot = go.GetComponent<Slot>();

                //gear.gameObject.GetComponent<UIElement>().AnimateUI();

                slot.isGold = false;


                slot.UpdateGearStatis(Slot.SlotStatis.REWARD);

                // Determien Gear rarity
                int rarityChance = Random.Range(1, 101);

                if (rarityChance >= gearLegendaryPerc)
                {
                    slot.UpdateRarity(Slot.Rarity.LEGENDARY);
                }
                else if (rarityChance >= gearEpicPerc)
                {
                    slot.UpdateRarity(Slot.Rarity.EPIC);
                }
                else if (rarityChance >= gearRarePerc)
                {
                    slot.UpdateRarity(Slot.Rarity.RARE);
                }
                else
                {
                    slot.UpdateRarity(Slot.Rarity.COMMON);
                }

                GearPiece newGear = null;

                // Determine what gear this Gear is
                if (slot.GetRarity() == Slot.Rarity.COMMON)
                {
                    int rand = Random.Range(0, allGearPiecesCommon.Count);
                    newGear = allGearPiecesCommon[rand];
                    slot.UpdateSlotImage(newGear.gearIcon);
                    OwnedLootInven.Instance.AddOwnedGear(slot);
                }
                else if (slot.GetRarity() == Slot.Rarity.RARE)
                {
                    int rand = Random.Range(0, allGearPiecesRare.Count);
                    newGear = allGearPiecesRare[rand];
                    slot.UpdateSlotImage(newGear.gearIcon);
                    OwnedLootInven.Instance.AddOwnedGear(slot);
                }
                else if (slot.GetRarity() == Slot.Rarity.EPIC)
                {
                    int rand = Random.Range(0, allGearPiecesEpic.Count);
                    newGear = allGearPiecesEpic[rand];
                    slot.UpdateSlotImage(newGear.gearIcon);
                    OwnedLootInven.Instance.AddOwnedGear(slot);
                }
                else if (slot.GetRarity() == Slot.Rarity.LEGENDARY)
                {
                    int rand = Random.Range(0, allGearPiecesLegendary.Count);
                    newGear = allGearPiecesLegendary[rand];
                    OwnedLootInven.Instance.AddOwnedGear(slot);
                }

                // Update gear type
                if (newGear.gearType == "helmet")
                {
                    slot.UpdateCurSlotType(Slot.SlotType.HELMET);
                }
                else if (newGear.gearType == "chestpiece")
                {
                    slot.UpdateCurSlotType(Slot.SlotType.CHESTPIECE);
                }
                else if (newGear.gearType == "boots")
                {
                    slot.UpdateCurSlotType(Slot.SlotType.BOOTS);
                }

                // Initialize gear base data
                slot.UpdateSlotImage(newGear.gearIcon);
                slot.UpdateSlotImage(newGear.gearIcon);
                slot.UpdateSlotName(newGear.gearName);
                slot.UpdateGearBonusHealth(newGear.bonusHealth);
                slot.UpdateGearBonusDamage(newGear.bonusDamage);
                slot.UpdateGearBonusHealing(newGear.bonusHealing);
                slot.UpdateGearBonusDefense(newGear.bonusDefense);
                slot.UpdateGearBonusSpeed(newGear.bonusSpeed);

                // Disable owned gear button for unowned loot
                slot.ToggleOwnedGearButton(false);
                // Show full visibility of Gear
                slot.UpdateLootGearAlpha(true);
            }
        }

        // After last reward given, enable post battle TO MAP - Button
        //StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(true));
    }

    IEnumerator GiveGold()
    {       
        // Give gold
        GameObject go = Instantiate(goldRewardGO, recievedGearParent.position, Quaternion.identity);
        go.transform.SetParent(recievedGearParent);
        go.transform.localScale = new Vector2(1, 1);

        UIElement uiElement = go.GetComponent<UIElement>();

        uiElement.UpdateContentImage(goldSprite);
        uiElement.UpdateContentText(CalculateGoldRecieved().ToString());
        uiElement.AnimateUI(false);

        ShopManager.Instance.UpdatePlayerGold(CalculateGoldRecieved());

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");

        yield return new WaitForSeconds(timeInbetweenRewards);
    }

    int CalculateGoldRecieved()
    {
        int gold = (GameManager.Instance.goldGainedPerUnit * GameManager.Instance.GetEnemiesKilledCount());
        return gold;
    }
}
