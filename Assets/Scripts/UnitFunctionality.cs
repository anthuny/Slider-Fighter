using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFunctionality : MonoBehaviour
{
    RectTransform rt;
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;
    [SerializeField] private UIElement selectionCircle;
    [SerializeField] private Transform powerUIParent;
    [SerializeField] private Image unitHealth;
    public Image unitImage;
    public UIElement curUnitTurnArrow;
    public int curSpeed;
    public int curPower;
    private int curHealth;
    private int maxHealth;
    private int curEnergy;
    private int maxEnergy;
    private int curlevel;
    private int curExp;
    private int maxExp;
    [HideInInspector]
    public int unitStartTurnEnergyGain;
    public EnergyCost energyCostImage;

    [SerializeField] private UIElement unitExpBar;
    [SerializeField] private Text unitLevelText;
    [SerializeField] private Image unitExpBarImage;
    [SerializeField] private Text unitExpGainText;
    [SerializeField] private float fillAmountInterval;
    [SerializeField] private float fillAmountIntervalTimeGap;

    [SerializeField] private UIElement healthBarUIElement;

    [HideInInspector]
    public GameObject prevPowerUI;

    private bool isSelected;


    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ToggleUnitExpVisual(false);
    }

    public void UpdateUnitColour(Color color)
    {
        unitImage.color = color;
    }

    public Color GetUnitColour()
    {
        return unitImage.color;
    }

    public Sprite GetUnitSprite()
    {
        return unitImage.sprite;
    }
    public void SpawnPowerUI(int power = 10)
    {
        // If this is NOT the first power text UI
        if (prevPowerUI != null)
            prevPowerUI = Instantiate(GameManager.instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.instance.powerUIHeightLvInc), Quaternion.identity);
        // If this IS the first power text UI
        else
            prevPowerUI = Instantiate(GameManager.instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);

        prevPowerUI.transform.SetParent(powerUIParent);
        prevPowerUI.transform.localScale = Vector2.one;

        PowerText powerText = prevPowerUI.GetComponent<PowerText>();

        // If power is 0, display that it missed
        if (power <= 0)
        {
            powerText.UpdatePowerTextFontSize(GameManager.instance.powerMissFontSize);
            powerText.UpdatePowerTextColour(GameManager.instance.missPowerTextColour);
            powerText.UpdatePowerText(GameManager.instance.missPowerText);
        }

        // Otherwise, display the power
        else
        {
            powerText.UpdatePowerTextFontSize(GameManager.instance.powerHitFontSize);

            // Change power text colour to offense colour if the type of attack is offense
            if (GameManager.instance.activeSkill.curSkillType == Skill.SkillType.OFFENSE)
                powerText.UpdatePowerTextColour(GameManager.instance.damagePowerTextColour);
            // Change power text colour to support colour if the type of attack is support
            else if (GameManager.instance.activeSkill.curSkillType == Skill.SkillType.SUPPORT)
                powerText.UpdatePowerTextColour(GameManager.instance.healPowerTextColour);

            powerText.UpdatePowerText(power.ToString());   // Update Power Text
        }

    }

    public void ResetPreviousPowerUI()
    {
        prevPowerUI = null;
    }


    public void ResetPosition()
    {
        //transform.position = Vector2.zero;
        rt.localPosition = Vector2.zero;
    }

    public void UpdateUnitName(string unitName)
    {
        gameObject.name = unitName;
    }

    public string GetUnitName()
    {
        return gameObject.name;
    }

    public void UpdateUnitSprite(Sprite sprite)
    {
        unitImage.sprite = sprite;
    }

    public void UpdateUnitType(string unitType)
    {
        if (unitType == "Enemy")
            curUnitType = UnitType.ENEMY;
        else
            curUnitType = UnitType.PLAYER;
    }

    public void ToggleSelected(bool toggle)
    {
        isSelected = toggle;
        
        if (toggle)
            selectionCircle.UpdateAlpha(1);
        else
            selectionCircle.UpdateAlpha(0);
    }
    public bool CheckIfUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0)
        {
            return true;
        }
        else
            return false;
    }

    public void EnsureUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0)
        {
            curHealth = 0;

            GameManager.instance.RemoveUnit(this);
            GameManager.instance.UpdateTurnOrderVisual();
            DestroyUnit();
        }
    }

    public void UpdateUnitExp(int gainedExp)
    {
        StartCoroutine(UpdateUnitExpVisual(gainedExp));
    }

    IEnumerator UpdateUnitExpVisual(int gainedExp)
    {
        //yield return 0;

        float curFillAmount = GetCurExp() / GetMaxExp();
        //float maxFillAmount = (curEnergy + gainedExp) / maxEnergy;
        //Debug.Log(curFillAmount);
        // Update exp bar for current energy
        unitExpBarImage.fillAmount = curFillAmount;

        // Display exp visual
        ToggleUnitExpVisual(true);

        for (int i = 0; i < gainedExp; i++)
        {
            yield return new WaitForSeconds(fillAmountIntervalTimeGap);

            float fillAmountIntervalTemp = (0.01f/100f) * GetMaxExp();
            Debug.Log("Max exp = " + GetMaxExp());

            if (GetCurExp() >= GetMaxExp())
            {
                int remainingExp = gainedExp - i;
                Debug.Log(remainingExp + " " + gameObject.name);
                UpdateUnitLevel(1, remainingExp);
                yield return 0;
            }
            else
            {
                unitExpBarImage.fillAmount += fillAmountIntervalTemp;
                IncreaseCurExp(1);
                //Debug.Log(unitExpBarImage);
                //Debug.Log(unitExpBarImage.fillAmount);
            }
        }

        ToggleUnitExpVisual(false);
    }

    public void ToggleUnitExpVisual(bool toggle)
    {
        if (toggle)
            unitExpBar.UpdateAlpha(1);
        else
            unitExpBar.UpdateAlpha(0);
    }

    public void UpdateUnitLevel(int level, int extraExp = 0)
    {
        curlevel += level;
        ResetUnitExp();
        UpdateUnitMaxExp();

        if (extraExp != 0)
        {
            UpdateUnitExp(extraExp);
        }
    }

    public int GetUnitLevel()
    {
        return curlevel;
    }

    void ResetUnitExp()
    {
        curExp = 0;
    }

    void IncreaseCurExp(int exp)
    {
        curExp += exp;

        if (GetCurExp() >= GetMaxExp())
        {
            curExp = (int)GetMaxExp();
        }
    }

    void UpdateUnitLevelVisual(int level)
    {
        unitLevelText.text = level.ToString();
    }

    public void UpdateUnitMaxExp()
    {
        float temp;
        if (GetUnitLevel() != 1)
        {
            temp = (GameManager.instance.maxExpLevel1 + (GameManager.instance.expIncPerLv / GameManager.instance.maxExpLevel1) * 100f) * GetUnitLevel();
            maxExp = (int)temp;
        }
        else
        {
            temp = GameManager.instance.maxExpLevel1 * GetUnitLevel();
            maxExp = (int)temp;
        }

    }

    public float GetCurExp()
    {
        return curExp;
    }

    public float GetMaxExp()
    {
        UpdateUnitMaxExp();
        return maxExp;
    }

    public void UpdateUnitCurrentHealth(int newCurHealth)
    {
        curHealth += newCurHealth;
        UpdateUnitHealthVisual();
    }

    public void UpdateUnitMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        UpdateUnitHealthVisual();
    }

    void UpdateUnitHealthVisual()
    {
        unitHealth.fillAmount = (float)curHealth / (float)maxHealth;
    }

    public void ToggleUnitHealthBar(bool toggle)
    {
        if (toggle)
            healthBarUIElement.UpdateAlpha(1);
        else
            healthBarUIElement.UpdateAlpha(0);  
    }

    public void UpdateUnitSpeed(int newSpeed)
    {
        curSpeed = newSpeed;
    }
    
    public void UpdateUnitPower(int newPower)
    {
        curPower = newPower;
    }

    public void UpdateUnitHealth(int newCurHealth, int newMaxHealth)
    {
        UpdateUnitCurrentHealth(newCurHealth);
        UpdateUnitMaxHealth(newMaxHealth);
    }

    public float GetUnitCurHealth()
    {
        return curHealth;
    }

    public float GetUnitMaxHealth()
    {
        return maxHealth;
    }

    public float GetUnitCurEnergy()
    {
        return curEnergy;
    }

    public float GetUnitMaxEnergy()
    {
        return maxEnergy;
    }

    public void UpdateUnitEnergy(int curEnergy, int maxEnergy)
    {
        this.curEnergy = curEnergy;
        this.maxEnergy = maxEnergy;
    }

    public void UpdateMaxEnergy(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
    }

    public void UpdateUnitCurEnergy(int energy)
    {
        //Debug.Log(gameObject.name + " - Energy was " + curEnergy);

        this.curEnergy += energy;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;

    //Debug.Log("Increased units energy by " + energy + " . Current energy is now " + curEnergy + " / " + maxEnergy);
    }

    public void UpdateUnitStartTurnEnergy(int newUnitStartTurnEnergyGain)
    {
        unitStartTurnEnergyGain = newUnitStartTurnEnergyGain;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    void DestroyUnit()
    {
        Destroy(gameObject);
    }
}
