using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GridTargetGroup : MonoBehaviour
{
    public static GridTargetGroup Instance;

    [SerializeField] private CinemachineTargetGroup targetGroup;

    public List<UnitFunctionality> camFocusedUnits = new List<UnitFunctionality>();

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetTargetGroupTrans()
    {
        return transform;
    }

    public void AddTarget(UnitFunctionality unit, bool playSFX = true)
    {
        if (camFocusedUnits.Contains(unit))
            return;


        if (camFocusedUnits.Count == 1)
        {
            if (playSFX)
                AudioManager.Instance.Play("SFX_CameraZoomOut");
        }

        camFocusedUnits.Add(unit);


        targetGroup.AddMember(unit.transform, 1, 1);
    }

    public void RemoveTarget(UnitFunctionality unit, bool playSFX = true)
    {
        if (!camFocusedUnits.Contains(unit))
            return;

        camFocusedUnits.Remove(unit);
        targetGroup.RemoveMember(unit.transform);
    }

    public void ClearTargetGroupMembers(UnitFunctionality unit)
    {
        //targetGroup.RemoveMember(unit.transform);
        RemoveTarget(unit);
    }

    public void UpdateTargets()
    {
        for (int i = 0; i < camFocusedUnits.Count; i++)
        {
            targetGroup.RemoveMember(camFocusedUnits[i].transform);
        }

        ClearTargets();

        AddTarget(GameManager.Instance.GetActiveUnitFunctionality());
    }

    public void ClearTargets()
    {
        camFocusedUnits.Clear();
    }
}
