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
    [SerializeField] private UIElement leftArrowUI;
    [SerializeField] private UIElement rightArrowUI;
    [SerializeField] private UIElement startButtonUI;

    [SerializeField] private TextMeshProUGUI allyNameText;
    [SerializeField] private Color lockedNameColour;

    [SerializeField] private bool unlockedWarrior = false;
    [SerializeField] private bool unlockedArcher = false;

    public RuntimeAnimatorController warriorAnimator;
    public RuntimeAnimatorController archerAnimator;

    public Color unlockedUnitColour;
    public Color lockedUnitColour;

    [SerializeField] private bool resetSave = false;

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

        ResetMenu();
    }

    public void ResetMenu()
    {
        ToggleMenu(true);

        SaveUnlockedAlly("Warrior");
        LoadSave();
        LoadCarasel();
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
                continue;
            }

            if (allAllies[i].unitName == "Warrior")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                if (IsWarriorUnlocked())
                {
                    allAlliesMenu[i].ToggleUnitLocked(false, false);
                }
                else
                    allAlliesMenu[i].ToggleUnitLocked(true, true);
            }

            if (allAllies[i].unitName == "Archer")
            {
                allAlliesMenu[i].gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 140);

                if (IsArcherUnlocked())
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
        PlayerPrefs.SetInt("UnlockedWarrior", 0);
        PlayerPrefs.SetInt("UnlockedArcher", 0);
        unlockedWarrior = false;
        unlockedArcher = false;
    }

    public void LoadSave()
    {
        if (PlayerPrefs.GetInt("UnlockedWarrior") == 1)
            unlockedWarrior = true;
        if (PlayerPrefs.GetInt("UnlockedArcher") == 1)
            unlockedArcher = true;
    }

    public void SaveUnlockedAlly(string allyName)
    {
        if (allyName == "Warrior")
        {
            PlayerPrefs.SetInt("UnlockedWarrior", 1);
            unlockedWarrior = true;
        }
        else if (allyName == "Archer")
        {
            PlayerPrefs.SetInt("UnlockedArcher", 1);
            unlockedArcher = true;
        }
    }

    public bool IsWarriorUnlocked()
    {
        return unlockedWarrior;
    }

    public bool IsArcherUnlocked()
    {
        return unlockedArcher;
    }
}
