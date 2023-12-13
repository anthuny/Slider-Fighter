using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearRewards : MonoBehaviour
{
    public static GearRewards Instance;

    [SerializeField] private Transform recievedGearParent;
    public GameObject gearGO;
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
        Setup();
    }

    public void Setup()
    {
        ResetRewardsTable();
    }

    public void FillGearRewardsTable()
    {
        ResetRewardsTable();

        GiveGold();


        // Give item(s)
        GiveItems();


        GameManager.Instance.ResetEnemiesKilledCount();

    }

    void ResetRewardsTable()
    {
        foreach (Transform child in recievedGearParent)
        {
            Destroy(child.gameObject);
        }
    }

    void GiveItems()
    {
        int count = GameManager.Instance.GetEnemiesKilledCount();

        // Check if EACH defeated enemy drops an item
        for (int i = 0; i < count; i++)
        {
            int gearChance = Random.Range(0, 101);

            // Roll if item drops
            if (gearChance >= unitDropRatePerc)
            {
                GameObject go = Instantiate(gearGO, recievedGearParent.position, Quaternion.identity);
                go.transform.SetParent(recievedGearParent);
                go.transform.localScale = new Vector2(1, 1);

                Gear gear = go.GetComponent<Gear>();

                gear.isGold = false;


                gear.UpdateGearStatis(Gear.GearStatis.REWARD);

                // Determien item rarity
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

                // Determine what gear this item is
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
                // Show full visibility of item
                gear.UpdateGearAlpha(true);
            }
        }
    }

    void GiveGold()
    {       
        // Give gold
        GameObject go = Instantiate(gearGO, recievedGearParent.position, Quaternion.identity);
        go.transform.SetParent(recievedGearParent);
        go.transform.localScale = new Vector2(1, 1);

        Gear gear = go.GetComponent<Gear>();

        gear.UpdateGearStatis(Gear.GearStatis.REWARD);

        gear.isGold = true;
        gear.UpdateGearImage(goldSprite);
        gear.UpdateGoldText(CalculateGoldRecieved());

        // Disable owned gear button for unowned loot
        gear.ToggleOwnedGearButton(false);

        //Reposition gold image to be lower?


        ShopManager.Instance.UpdatePlayerGold(CalculateGoldRecieved());
    }

    int CalculateGoldRecieved()
    {
        int gold = (GameManager.Instance.goldGainedPerUnit * GameManager.Instance.GetEnemiesKilledCount());
        return gold;
    }
}
