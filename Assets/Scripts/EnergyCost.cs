using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCost : MonoBehaviour
{
    [SerializeField] private Transform energyParent;
    [SerializeField] private Transform energyBGParent;
    [SerializeField] private CanvasGroup energyParentCG;
    [SerializeField] private CanvasGroup energyBGParentCG;
    [SerializeField] private GameObject energyBarPrefab;
    [SerializeField] private GameObject energyBarBGPrefab;
    [SerializeField] private float energyBarDifferenceTime;
    [SerializeField] private float energyBarPostTime;

    [HideInInspector]
    public UnitFunctionality unitFunctionality;

    OverlayUI abilityDetailsUI;

    private void Awake()
    {
        abilityDetailsUI = GameManager.Instance.abilityDetailsUI;

        ToggleEnergyBar(false);
        ToggleEnergyBGBar(false);
    }

    public void UpdateEnergyBar(int curEnergy, int energyAmountMoved, bool increasing, bool toggle, bool enemy = false)
    {
        if (toggle)
        {
            if (!increasing)    // If Energy Decreasing
            {
                // If unit is at 0 energy, stop
                if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == 0)
                    return;

                // Remove all potential previous energy bars
                foreach (Transform child in energyParent)
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in energyBGParent)
                {
                    Destroy(child.gameObject);
                }

                int maxEnergy = (int)GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy();

                // Add BG
                for (int i = 0; i < maxEnergy; i++)
                {
                    GameObject energyBG = Instantiate(energyBarBGPrefab, energyBGParent);
                    energyBG.transform.SetParent(energyBGParent);
                }

                // Add all current bars
                for (int i = 0; i < curEnergy; i++)
                {
                    // Add Energy Bars
                    GameObject energy = Instantiate(energyBarPrefab, energyParent);
                    energy.transform.SetParent(energyParent);
                }

                // Display Energy Bars
                ToggleEnergyBar(true);
                ToggleEnergyBGBar(true);

                GameManager.Instance.GetActiveUnitFunctionality().effects.UpdateAlpha(0);
                StartCoroutine(RemoveEnergyBar(energyAmountMoved, enemy));
            }

            else    // If Energy Increasing
            {
                // If unit is at maxed energy, stop
                if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy())
                {
                    GameManager.Instance.GetActiveUnitFunctionality().effects.UpdateAlpha(1);
                    return;
                }

                // Remove all potential previous energy bars
                foreach (Transform child in energyParent)
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in energyBGParent)
                {
                    Destroy(child.gameObject);
                }

                int maxEnergy = (int)GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy();

                // Add BG
                for (int i = 0; i < maxEnergy; i++)
                {
                    GameObject energyBG = Instantiate(energyBarBGPrefab, energyBGParent);
                    energyBG.transform.SetParent(energyBGParent);
                }

                // Add Current Energy Bars
                for (int i = 0; i < curEnergy; i++)
                {
                    GameObject energy = Instantiate(energyBarPrefab, energyParent);
                    energy.transform.SetParent(energyParent);
                }

                // Display Energy Bars
                ToggleEnergyBar(true);
                ToggleEnergyBGBar(true);

                StartCoroutine(AddEnergyBar(energyAmountMoved));
            }
        }

        else
        {
            ToggleEnergyBar(false);
            ToggleEnergyBGBar(false);
        }
    }

    IEnumerator RemoveEnergyBar(int removeCount, bool enemy)
    {
        // Remove current bars
        for (int i = 0; i < removeCount; i++)
        {
            if (energyParent.childCount == 0)
                yield break;

            yield return new WaitForSeconds(energyBarDifferenceTime);

            // Update energy for the unit
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(-1);

            if (GameManager.Instance.GetActiveUnitType())
                Destroy(energyParent.GetChild(0).gameObject);

            // Update Unit Overlay Energy
            abilityDetailsUI.UpdateUnitOverlayEnergyUI(GameManager.Instance.GetActiveUnitFunctionality(),
                GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy(),
                GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy());

            GameManager.Instance.UpdateAllSkillIconAvailability();      // Update Unit Overlay Skill Availability

            if (i == removeCount - 1)
            {
                yield return new WaitForSeconds(energyBarPostTime);

                if (!enemy)
                {
                    Weapon.instance.StartHitLine();
                    GameManager.Instance.SetupPlayerWeaponUI();
                }

                ToggleEnergyBar(false);
                ToggleEnergyBGBar(false);

                // Renable the Healthbar after removing the energy bar
                GameManager.Instance.UpdateActiveUnitHealthBar(true);
                GameManager.Instance.GetActiveUnitFunctionality().effects.UpdateAlpha(1);
                yield break;
            }
        }
    }

    IEnumerator AddEnergyBar(int addCount)
    {
        // Add Energy Bars
        for (int i = 0; i < addCount; i++)
        {
            yield return new WaitForSeconds(energyBarDifferenceTime);

            // Update energy for the unit
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(1);

            GameObject energy = Instantiate(energyBarPrefab, energyParent);
            energy.transform.SetParent(energyParent);

            // Update Unit Overlay Energy
            abilityDetailsUI.UpdateUnitOverlayEnergyUI(GameManager.Instance.GetActiveUnitFunctionality(),
                GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy(), 
                GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy());

            GameManager.Instance.UpdateAllSkillIconAvailability();      // Update Unit Overlay Skill Availability

            // If unit is at maxed energy, stop
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxEnergy())
            {
                i = addCount;
            }
        }

        yield return new WaitForSeconds(energyBarPostTime);

        // Renable the Healthbar after removing the energy bar
        GameManager.Instance.UpdateActiveUnitHealthBar(true);
        GameManager.Instance.GetActiveUnitFunctionality().effects.UpdateAlpha(1);

        ToggleEnergyBar(false);
        ToggleEnergyBGBar(false);
    }

    void ToggleEnergyBar(bool toggle)
    {
        if (toggle)
            energyParentCG.alpha = 1;
        else
            energyParentCG.alpha = 0;
    }

    void ToggleEnergyBGBar(bool toggle)
    {
        if (toggle)
            energyBGParentCG.alpha = 1;
        else
            energyBGParentCG.alpha = 0;
    }
}
