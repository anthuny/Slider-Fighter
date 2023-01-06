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
        abilityDetailsUI = GameManager.instance.abilityDetailsUI;

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
                if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == 0)
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

                int maxEnergy = (int)GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy();

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

                StartCoroutine(RemoveEnergyBar(energyAmountMoved, enemy));
            }

            else    // If Energy Increasing
            {
                // If unit is at maxed energy, stop
                if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy())
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

                int maxEnergy = (int)GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy();

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
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(-1);

            Destroy(energyParent.GetChild(0).gameObject);

            // Update Unit Overlay Energy
            abilityDetailsUI.UpdateUnitOverlayEnergyUI(GameManager.instance.GetActiveUnitFunctionality(),
                GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy(),
                GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy());

            GameManager.instance.UpdateAllSkillIconAvailability();      // Update Unit Overlay Skill Availability

            if (i == removeCount - 1)
            {
                yield return new WaitForSeconds(energyBarPostTime);

                if (!enemy)
                    GameManager.instance.SetupPlayerWeaponUI();

                ToggleEnergyBar(false);
                ToggleEnergyBGBar(false);

                // Renable the Healthbar after removing the energy bar
                GameManager.instance.UpdateActiveUnitHealthBar(true);
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
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(1);

            GameObject energy = Instantiate(energyBarPrefab, energyParent);
            energy.transform.SetParent(energyParent);

            // Update Unit Overlay Energy
            abilityDetailsUI.UpdateUnitOverlayEnergyUI(GameManager.instance.GetActiveUnitFunctionality(),
                GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy(), 
                GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy());

            GameManager.instance.UpdateAllSkillIconAvailability();      // Update Unit Overlay Skill Availability

            // If unit is at maxed energy, stop
            if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy())
                i = addCount;
        }

        yield return new WaitForSeconds(energyBarPostTime);

        ToggleEnergyBar(false);
        ToggleEnergyBGBar(false);

        // Renable the Healthbar after removing the energy bar
        GameManager.instance.UpdateActiveUnitHealthBar(true);
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
