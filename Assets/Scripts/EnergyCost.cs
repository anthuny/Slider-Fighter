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

    private void Awake()
    {
        ToggleEnergyBar(false);
        ToggleEnergyBGBar(false);
    }

    public void UpdateEnergyBar(int curEnergy, int energyAmountMoved, bool increasing, bool toggle)
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

                StartCoroutine(RemoveEnergyBar(energyAmountMoved));
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

    IEnumerator RemoveEnergyBar(int removeCount)
    {
        // Update energy for the unit
        GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(-removeCount);

        // Remove current bars
        for (int i = 0; i < removeCount; i++)
        {
            if (energyParent.childCount == 0)
                yield break;

            yield return new WaitForSeconds(energyBarDifferenceTime);

            Destroy(energyParent.GetChild(0).gameObject);

            if (i == removeCount - 1)
            {
                yield return new WaitForSeconds(energyBarPostTime);
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
        // Update energy for the unit
        GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurEnergy(addCount);

        // Add Energy Bars
        for (int i = 0; i < addCount; i++)
        {
            // If unit is at maxed energy, stop
            if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() == GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxEnergy())
                yield break;

            yield return new WaitForSeconds(energyBarDifferenceTime);

            GameObject energy = Instantiate(energyBarPrefab, energyParent);
            energy.transform.SetParent(energyParent);
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
