using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject effectPrefab;
    public List<EffectData> allEffects = new List<EffectData>();
    public TMP_ColorGradient gradientEffectAlert;
    public TMP_ColorGradient gradientEffectTrigger;
    public TMP_ColorGradient gradientEffectDeplete;
    [SerializeField] private int maxEffectTurnsRemaining = 200;

    private void Awake()
    {
        instance = this;
    }

    public int GetMaxEffectTurnsRemaining()
    {
        return maxEffectTurnsRemaining;
    }

    public EffectData GetEffect(string name)
    {
        for (int i = 0; i < allEffects.Count; i++)
        {
            if (allEffects[i].effectName == name)
                return allEffects[i];
        }

        return null;
    }
}
