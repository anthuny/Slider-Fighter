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

                if (ItemRewardManager.Instance.selectedItem.curRarity == Item.Rarity.LEGENDARY)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.legendaryColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == Item.Rarity.EPIC)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.epicColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == Item.Rarity.RARE)
                {
                    uIElement.UpdateRarityBorderColour(ItemRewardManager.Instance.rareColour);
                }
                else if (ItemRewardManager.Instance.selectedItem.curRarity == Item.Rarity.COMMON)
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

                Gear gear = go.GetComponent<Gear>();

                //gear.gameObject.GetComponent<UIElement>().AnimateUI();

                gear.isGold = false;


                gear.UpdateGearStatis(Gear.GearStatis.REWARD);

                // Determien Gear rarity
                int rarityChance = Random.Range(0, 101);

                if (gearLegendaryPerc >= rarityChance)
                {
                    gear.UpdateRarity(Gear.Rarity.LEGENDARY);
                }
                else if (gearEpicPerc >= rarityChance)
                {
                    gear.UpdateRarity(Gear.Rarity.EPIC);
                }
                else if (gearRarePerc >= rarityChance)
                {
                    gear.UpdateRarity(Gear.Rarity.RARE);
                }
                else if (gearCommonPerc >= rarityChance)
                {
                    gear.UpdateRarity(Gear.Rarity.COMMON);
                }

                GearPiece newGear = null;

                // Determine what gear this Gear is
                if (gear.GetRarity() == Gear.Rarity.COMMON)
                {
                    int rand = Random.Range(0, allGearPiecesCommon.Count);
                    newGear = allGearPiecesCommon[rand];

                    OwnedGearInven.Instance.AddOwnedGear(gear);
                }
                else if (gear.GetRarity() == Gear.Rarity.RARE)
                {
                    int rand = Random.Range(0, allGearPiecesRare.Count);
                    newGear = allGearPiecesRare[rand];
                    gear.UpdateGearImage(newGear.gearIcon);
                    OwnedGearInven.Instance.AddOwnedGear(gear);
                }
                else if (gear.GetRarity() == Gear.Rarity.EPIC)
                {
                    int rand = Random.Range(0, allGearPiecesEpic.Count);
                    newGear = allGearPiecesEpic[rand];
                    gear.UpdateGearImage(newGear.gearIcon);
                    OwnedGearInven.Instance.AddOwnedGear(gear);
                }
                else if (gear.GetRarity() == Gear.Rarity.LEGENDARY)
                {
                    int rand = Random.Range(0, allGearPiecesLegendary.Count);
                    newGear = allGearPiecesLegendary[rand];
                    OwnedGearInven.Instance.AddOwnedGear(gear);
                }

                // Update gear type
                if (newGear.gearType == "helmet")
                {
                    gear.UpdateCurGearType(Gear.GearType.HELMET);
                }
                else if (newGear.gearType == "chestpiece")
                {
                    gear.UpdateCurGearType(Gear.GearType.CHESTPIECE);
                }
                else if (newGear.gearType == "leggings")
                {
                    gear.UpdateCurGearType(Gear.GearType.LEGGINGS);
                }
                else if (newGear.gearType == "boots")
                {
                    gear.UpdateCurGearType(Gear.GearType.BOOTS);
                }

                // Initialize gear base data
                gear.UpdateGearImage(newGear.gearIcon);
                gear.UpdateGearImage(newGear.gearIcon);
                gear.UpdateGearName(newGear.gearName);
                gear.UpdateGearBonusHealth(newGear.bonusHealth);
                gear.UpdateGearBonusDamage(newGear.bonusDamage);
                gear.UpdateGearBonusHealing(newGear.bonusHealing);
                gear.UpdateGearBonusDefense(newGear.bonusDefense);
                gear.UpdateGearBonusSpeed(newGear.bonusSpeed);

                // Disable owned gear button for unowned loot
                gear.ToggleOwnedGearButton(false);
                // Show full visibility of Gear
                gear.UpdateGearAlpha(true);
            }
        }
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
