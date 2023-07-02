using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

    public UIElement masteryL1;
    public UIElement masteryL2;
    public UIElement masteryL3;
    public UIElement masteryL4;

    public UIElement masteryR1;
    public UIElement masteryR2;
    public UIElement masteryR3;
    public UIElement masteryR4;

    [SerializeField] private UIElement masteryDesc;
    [SerializeField] private UIElement masteryFillAmount;

    private void Awake()
    {
        Instance = this;
    }

    public void SetupTeamSetup(UnitFunctionality unit)
    {
        masteryL1.UpdateContentImage(unit.GetMastery(0).masteryIcon);
        masteryL2.UpdateContentImage(unit.GetMastery(1).masteryIcon);
        masteryL3.UpdateContentImage(unit.GetMastery(2).masteryIcon);
        masteryL4.UpdateContentImage(unit.GetMastery(3).masteryIcon);

        masteryR1.UpdateContentImage(unit.GetMastery(4).masteryIcon);
        masteryR2.UpdateContentImage(unit.GetMastery(5).masteryIcon);
        masteryR3.UpdateContentImage(unit.GetMastery(6).masteryIcon);
        masteryR4.UpdateContentImage(unit.GetMastery(7).masteryIcon);

        masteryL1.UpdateContentSubText(" / " + unit.GetMastery(0).masteryMaxAmount);
        masteryL2.UpdateContentSubText(" / " + unit.GetMastery(1).masteryMaxAmount);
        masteryL3.UpdateContentSubText(" / " + unit.GetMastery(2).masteryMaxAmount);
        masteryL4.UpdateContentSubText(" / " + unit.GetMastery(3).masteryMaxAmount);

        masteryR1.UpdateContentSubText(" / " + unit.GetMastery(4).masteryMaxAmount);
        masteryR2.UpdateContentSubText(" / " + unit.GetMastery(5).masteryMaxAmount);
        masteryR3.UpdateContentSubText(" / " + unit.GetMastery(6).masteryMaxAmount);
        masteryR4.UpdateContentSubText(" / " + unit.GetMastery(7).masteryMaxAmount);
    }



    public void UpdateMasteryDescription(Mastery mastery)
    {
        masteryDesc.UpdateContentText(mastery.masteryName);
        masteryDesc.UpdateContentSubText(mastery.masteryDesc);
        masteryFillAmount.UpdateContentSubText(" / " + mastery.masteryMaxAmount.ToString());
    }
}
