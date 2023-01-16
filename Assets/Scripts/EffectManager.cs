using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public GameObject effectPrefab;
    public List<GameObject> allEffects = new List<GameObject>();
    public TMP_ColorGradient gradientEffectAlert;
    public TMP_ColorGradient gradientEffectTrigger;
    public TMP_ColorGradient gradientEffectDeplete;

    private void Awake()
    {
        instance = this;
    }
}
