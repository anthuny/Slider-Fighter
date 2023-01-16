using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public enum EffectTrigger { ONGOING, TURNSTART, DAMAGERECIEVED }
    public EffectTrigger curEffectTrigger;

    public enum EffectDeplete { TURNSTART, TURNEND, DAMAGERECIEVED }
    public EffectDeplete curEffectDeplete;

    public string effectName;
    public string effectDesc;

    public int turnCountRemaining;

    private Image effectIconImage;

    private void Awake()
    {
        effectIconImage = GetComponent<Image>();
    }
    public void Setup(EffectData effect)
    {
        // curEffectTrigger = effect.curEffectTrigger;
        // curEffectDeplete = effect.curEffectDeplete;
        effectName = effect.effectName; 
        effectDesc = effect.effectDesc;
        UpdateEffectIcon(effect);
    }

    void UpdateEffectIcon(EffectData effect)
    {
        effectIconImage.sprite = effect.effectIcon;
    }
}

