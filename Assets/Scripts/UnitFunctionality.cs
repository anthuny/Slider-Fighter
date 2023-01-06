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
    [SerializeField] private UIElement unitVisuals;
    [SerializeField] private Transform unitVisualsParent;
    [SerializeField] private Transform powerUIParent;
    [SerializeField] private UIElement statUI;
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
    private float curExp;
    private float maxExp;
    [HideInInspector]
    public int unitStartTurnEnergyGain;
    public EnergyCost energyCostImage;

    [SerializeField] private UIElement unitExpBar;
    [SerializeField] private Text unitLevelText;
    [SerializeField] private Image unitExpBarImage;
    [SerializeField] private Text unitExpGainText;
    [SerializeField] private float fillAmountInterval;
    [SerializeField] private UIElement unitBg;


    [SerializeField] private UIElement healthBarUIElement;
    [SerializeField] private UIElement unitUIElement;

    [HideInInspector]
    public GameObject prevPowerUI;

    private bool isSelected;
    private int unitValue;
    public bool idleBattle;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ToggleUnitExpVisual(false);
        ToggleUnitBG(false);
    }

    public void ToggleIdleBattle(bool toggle)
    {
        idleBattle = toggle;
    }

    public void TriggerSkillAlert()
    {
        statUI.UpdateContentText(GameManager.instance.GetActiveSkill().skillName);
        statUI.UpdateAlpha(1);
    }

    public bool GetIdleBattle()
    {
        return idleBattle;
    }

    public IEnumerator StartUnitTurn()
    {
        // Do unit's turn automatically if its on idle battle
        if (GetIdleBattle())
        {
            yield return new WaitForSeconds(GameManager.instance.enemyThinkTime + 1);

            // If unit has energy to choose a skill, choose one
            GameManager.instance.UpdateActiveSkill(ChooseRandomSkill());

            // If the energy DOESNT cost any energy, make energy cost ui appear on casting unit DOESNT APPEAR
            if (GameManager.instance.activeSkill.skillEnergyCost != 0)
            {
                if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.instance.activeSkill.skillEnergyCost)
                {
                    // Trigger current unit's turn energy count to deplete for skill use
                    GameManager.instance.UpdateActiveUnitEnergyBar(true, false, GameManager.instance.activeSkill.skillEnergyCost, true);
                    GameManager.instance.UpdateActiveUnitHealthBar(false);
                }
                else
                {
                    // End turn
                    GameManager.instance.ToggleEndTurnButton(false);
                    GameManager.instance.UpdateTurnOrder();
                    yield break;
                }
            }


            // Select units
            GameManager.instance.UpdateUnitSelection(GameManager.instance.activeSkill);

            TriggerSkillAlert();

            yield return new WaitForSeconds(GameManager.instance.enemyAttackWaitTime);

            // Attack
            StartCoroutine(GameManager.instance.WeaponAttackCommand(GameManager.instance.activeSkill.skillPower));

            // Attack again
            StartCoroutine(StartUnitTurn());

            // If unit has energy for another attack, go back to first step

                // If unit no longer has energy for any skills, end turn.
        }
    }

    SkillData ChooseRandomSkill()
    {
        int rand = Random.Range(1, 4);

        if (rand == 1)  // Skill 1
        {
            if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.instance.GetActiveUnit().GetSkill1().skillEnergyCost)
                return GameManager.instance.GetActiveUnit().GetSkill1();
            else
                return GameManager.instance.GetActiveUnit().basicSkill;
        }
        else if (rand == 2)  // Skill 2
        {
            if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.instance.GetActiveUnit().GetSkill2().skillEnergyCost)
                return GameManager.instance.GetActiveUnit().GetSkill2();
            else
                return GameManager.instance.GetActiveUnit().basicSkill;
        }
        else if (rand == 3)  // Skill 3
        {
            if (GameManager.instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.instance.GetActiveUnit().GetSkill3().skillEnergyCost)
                return GameManager.instance.GetActiveUnit().GetSkill3();
            else
                return GameManager.instance.GetActiveUnit().basicSkill;
        }
        else
        {
            return null;
        }
    }

    public void UpdateUnitValue(int val)
    {
        unitValue = val;
    }

    public int GetUnitValue()
    {
        return unitValue;
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
            if (GameManager.instance.activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                powerText.UpdatePowerTextColour(GameManager.instance.damagePowerTextColour);
            // Change power text colour to support colour if the type of attack is support
            else if (GameManager.instance.activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
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

    public void UpdateUnitSprite(GameObject go)
    {
        Instantiate(go, unitVisualsParent);
        //go.transform.SetParent(unitVisualsParent);
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);
        //unitImage.sprite = sprite;
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

    void ToggleExpGainedText(bool toggle, string text)
    {
        unitExpGainText.gameObject.SetActive(toggle);
        unitExpGainText.text = "+ " + text;
    }

    public void ToggleUnitBG(bool toggle)
    {
        if (toggle)
            unitBg.UpdateAlpha(.1f);
        else
            unitBg.UpdateAlpha(0);
    }

    public IEnumerator UpdateUnitExpVisual(int gainedExp)
    {
        ToggleExpGainedText(true, gainedExp.ToString());

        float curFillAmount = GetCurExp() / GetMaxExp();

        // Update exp bar for current energy
        unitExpBarImage.fillAmount = curFillAmount;

        // Display exp visual
        ToggleUnitExpVisual(true);

        for (int i = 0; i < gainedExp; i++)
        {
            yield return new WaitForSeconds(GameManager.instance.fillAmountIntervalTimeGap);

            if (GetCurExp() >= GetMaxExp())
            {
                int remainingExp = gainedExp - i;
                UpdateUnitLevel(1, remainingExp);
                yield break;
            }
            else
            {
                IncreaseCurExp(1);
                unitExpBarImage.fillAmount = GetCurExp() / GetMaxExp();
            }
        }

        yield return new WaitForSeconds(GameManager.instance.timePostExp);
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
        UpdateUnitLevelVisual(GetUnitLevel());

        ResetUnitExp();
        UpdateUnitMaxExp();

        if (extraExp != 0)
        {
            //Debug.Log(gameObject.name + " extra exp = " + extraExp);
            UpdateUnitExp(extraExp);
        }
    }

    void UpdateUnitLevelVisual(int level)
    {
        unitLevelText.text = level.ToString();
    }

    public int GetUnitLevel()
    {
        return curlevel;
    }

    void ResetUnitExp()
    {
        curExp = 0;
    }

    void IncreaseCurExp(float exp)
    {
        curExp += exp;

        if (GetCurExp() >= GetMaxExp())
        {
            curExp = (int)GetMaxExp();
        }
    }

    public void UpdateUnitMaxExp()
    {
        float temp;
        if (GetUnitLevel() != 1)
        {
            temp = GameManager.instance.maxExpStarting + (GameManager.instance.expIncPerLv * (GetUnitLevel()-1));
            //temp = (GameManager.instance.maxExpLevel1 + ((GameManager.instance.expIncPerLv / GameManager.instance.maxExpLevel1) * 100f)) * GetUnitLevel();
            maxExp = (int)temp;
            //Debug.Log(gameObject.name + " " + maxExp);
        }
        else
        {
            temp = GameManager.instance.maxExpStarting * GetUnitLevel();
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
        ToggleUnitHealthBar(true);

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
        this.curEnergy += energy;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;
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
        unitVisuals.UpdateAlpha(0); 
        //Destroy(gameObject);
    }

    public int GetUnitExpKillGained()
    {
        int expGained = (GetUnitLevel() * GameManager.instance.expKillGainedPerLv) + GameManager.instance.expKillGainedStarting;
        return expGained;
    }
}
