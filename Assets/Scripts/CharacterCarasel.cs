using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterCarasel : MonoBehaviour
{
    public static CharacterCarasel Instance;

    [SerializeField] private List<UnitData> allAllies = new List<UnitData>();

    [SerializeField] private List<MenuUnitDisplay> allAlliesMenu = new List<MenuUnitDisplay>();

    [SerializeField] private UIElement mainMenuUI;
    //[SerializeField] private UIElement highestFloorReachedText;
    public UIElement highestFloorReachedCountText;
    [SerializeField] private UIElement leftArrowUI;
    [SerializeField] private UIElement rightArrowUI;
    [SerializeField] private UIElement startButtonUI;

    [SerializeField] private TextMeshProUGUI allyNameText;
    [SerializeField] private Color lockedNameColour;

    [SerializeField] private bool unlockedKnight = false;
    [SerializeField] private bool unlockedRanger = false;
    [SerializeField] private bool unlockedCleric = false;
    [SerializeField] private bool unlockedNecromancer = false;
    [SerializeField] private bool unlockedMonk = false;

    public RuntimeAnimatorController warriorAnimator;
    public RuntimeAnimatorController archerAnimator;
    public RuntimeAnimatorController clericAnimator;
    public RuntimeAnimatorController necromancerAnimator;
    public RuntimeAnimatorController monkAnimator;

    public Color unlockedUnitColour;
    public Color lockedUnitColour;

    [SerializeField] private bool resetSave = false;

    public void UpdateHighestFloorReached(int count)
    {
        highestFloorReachedCountText.UpdateContentText(count.ToString());
    }

    public UnitData GetAlly(int index)
    {
        return allAllies[index];
    }

    public List<UnitData> GetAllAllies()
    {
        return allAllies;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (resetSave)
        {
            resetSave = false;
            ResetSave();
        }

        //ResetMenu();
    }

    public void ResetMenu()
    {
        ToggleMenu(true);

        SaveUnlockedAlly("Knight");
        SaveUnlockedAlly("Ranger");
        SaveUnlockedAlly("Necromancer");
        SaveUnlockedAlly("Monk");
        SaveUnlockedAlly("Cleric");

        LoadSave();
        LoadCarasel();

        UpdateLog.Instance.ToggleUpdateLogbutton(true);

        RoomManager.Instance.SaveLoadHighestFloor();
    }

    public void ToggleMenu(bool toggle)
    {
        if (toggle)
        {
            mainMenuUI.UpdateAlpha(1);
            leftArrowUI.ToggleButton(true);
            leftArrowUI.UpdateAlpha(1);
            rightArrowUI.ToggleButton(true);
            rightArrowUI.UpdateAlpha(1);

            startButtonUI.UpdateAlpha(1);
            startButtonUI.ToggleButton(true);
        }
        else
        {
            mainMenuUI.UpdateAlpha(0);
            leftArrowUI.ToggleButton(false);
            leftArrowUI.UpdateAlpha(0);
            rightArrowUI.ToggleButton(false);
            rightArrowUI.UpdateAlpha(0);

            startButtonUI.UpdateAlpha(0);
            startButtonUI.ToggleButton(false);

            MapManager.Instance.Setup();
        }
    }

    public void SelectAlly(ButtonFunctionality button)
    {
        if (!allAlliesMenu[0].GetLocked())
        {
            GameManager.Instance.AddUnitToTeam(allAllies[0]);

            // Play unit attack animation
            allAlliesMenu[0].StartAttackAnim();

            button.buttonLocked = true;

            button.StartCoroutine("MenuUnitSelection");
        }
    }

    public void SpinCarasel(bool leftDir)
    {
        // Right Arrow
        if (!leftDir)
        {
            UnitData unitData = allAllies[0];
            allAllies.RemoveAt(0);
            allAllies.Insert(allAllies.Count, unitData);
        }
        // Left Arrow
        else
        {
            UnitData unitData = allAllies[allAllies.Count-1];
            allAllies.RemoveAt(allAllies.Count-1);
            allAllies.Insert(0, unitData);
        }

        LoadCarasel();
    }

    public void LoadCarasel()
    {
        for (int i = 0; i < allAlliesMenu.Count; i++)
        {
            // If there are total allies remaining for the carasel
            if (i < allAllies.Count)
                allAlliesMenu[i].UpdateUnitDisplay(allAllies[i].unitName);
            // If no unlocked characters remain, have all unlocked characters be locked as hidden warriors
            else
            {
                allAlliesMenu[i].UpdateUnitDisplay("Locked");
                FighterStatManager.Instance.UpdateFighterStats(null);
                continue;
            }

            if (allAllies[i].unitName == "Knight")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                if (IsKnightUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
            }

            if (allAllies[i].unitName == "Necromancer")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                if (IsNecromancerUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
            }

            if (allAllies[i].unitName == "Ranger")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 140);

                if (IsRangerUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                {
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
                }
            }

            if (allAllies[i].unitName == "Cleric")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                if (IsClericUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                {
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
                }
            }

            if (allAllies[i].unitName == "Monk")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                if (IsMonkUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                {
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
                }
            }

            // Do walk/idle animation
            if (i == 0 && !allAlliesMenu[i].GetLocked())
            {
                allAlliesMenu[0].StartWalkAnim();

                UpdateAllyDisplayName(allAllies[0]);
            }
            else
            {
                allAlliesMenu[i].StartIdleAnim();
            }

            // Update unit display name
            if (i == 0 && allAlliesMenu[i].GetLocked())
            {
                UpdateAllyDisplayName(allAllies[0], true);
            }
        }

        FighterStatManager.Instance.UpdateFighterStats(allAllies[0]);

        PostBattle.Instance.toMapButton.postBattleButtonPressed = false;
    }

    public void UpdateAllyDisplayName(UnitData unit, bool locked = false)
    {
        allyNameText.text = unit.unitName;

        if (!locked)
            allyNameText.color = unit.unitColour;
        else
            allyNameText.color = lockedNameColour;
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("UnlockedKnight", 0);
        PlayerPrefs.SetInt("UnlockedRanger", 0);
        PlayerPrefs.SetInt("UnlockedCleric", 0);
        PlayerPrefs.SetInt("UnlockedNecromancer", 0);
        PlayerPrefs.SetInt("UnlockedMonk", 0);
        unlockedKnight = false;
        unlockedRanger = false;
        unlockedCleric = false;
        unlockedNecromancer = false;
        unlockedMonk = false;
    }

    public void LoadSave()
    {
        if (PlayerPrefs.GetInt("UnlockedKnight") == 1)
            unlockedKnight = true;
        if (PlayerPrefs.GetInt("UnlockedRanger") == 1)
            unlockedRanger = true;
        if (PlayerPrefs.GetInt("UnlockedCleric") == 1)
            unlockedCleric = true;
        if (PlayerPrefs.GetInt("UnlockedNecromancer") == 1)
            unlockedNecromancer = true;
        if (PlayerPrefs.GetInt("UnlockedMonk") == 1)
            unlockedMonk = true;
    }

    public void SaveUnlockedAlly(string allyName)
    {
        if (allyName == "Knight")
        {
            PlayerPrefs.SetInt("UnlockedKnight", 1);
            unlockedKnight = true;
        }
        else if (allyName == "Ranger")
        {
            PlayerPrefs.SetInt("UnlockedRanger", 1);
            unlockedRanger = true;
        }
        else if (allyName == "Cleric")
        {
            PlayerPrefs.SetInt("UnlockedCleric", 1);
            unlockedCleric = true;
        }
        else if (allyName == "Necromancer")
        {
            PlayerPrefs.SetInt("UnlockedNecromancer", 1);
            unlockedNecromancer = true;
        }
        else if (allyName == "Monk")
        {
            PlayerPrefs.SetInt("UnlockedMonk", 1);
            unlockedMonk = true;
        }
    }

    public bool IsNecromancerUnlocked()
    {
        return unlockedNecromancer;
    }
    public bool IsMonkUnlocked()
    {
        return unlockedMonk;
    }

    public bool IsKnightUnlocked()
    {
        return unlockedKnight;
    }

    public bool IsRangerUnlocked()
    {
        return unlockedRanger;
    }

    public bool IsClericUnlocked()
    {
        return unlockedCleric;
    }
}
