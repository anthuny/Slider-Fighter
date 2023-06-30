using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    public static TeamSetup Instance;

    [SerializeField] private UIElement masteryL1;
    [SerializeField] private UIElement masteryL2;
    [SerializeField] private UIElement masteryL3;
    [SerializeField] private UIElement masteryL4;

    [SerializeField] private UIElement masteryR1;
    [SerializeField] private UIElement masteryR2;
    [SerializeField] private UIElement masteryR3;
    [SerializeField] private UIElement masteryR4;

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
    }



    public void UpdateMasteryDescription(Mastery mastery)
    {
        masteryDesc.UpdateContentText(mastery.masteryName);
        masteryDesc.UpdateContentSubText(mastery.masteryDesc);
        masteryFillAmount.UpdateContentSubText(" / " + mastery.masteryMaxAmount.ToString());
    }
}
