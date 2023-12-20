using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public enum HitAreaType { PERFECT, GREAT, GOOD, BAD, MISS }
    public HitAreaType curHitAreaType;

    public static Weapon Instance;

    [SerializeField] private Transform hitLine;
    [SerializeField] private float minDistanceBorderTrigger;
    [SerializeField] private Transform topBarBorder;
    [SerializeField] private Transform botBarBorder;
    public float weaponLineSpeed = 350f;
    public float timePostHit = .85f;
    bool goingUp;
    bool isStopped;
    [SerializeField] private List<WeaponHitArea> weaponHitAreas = new List<WeaponHitArea>();
    [SerializeField] Image attackButton;

    [Header("Hit Alert")]
    [SerializeField] private UIElement hitAlertText;
    public string perfectHitAlertText;
    public string greatHitAlertText;
    public string goodHitAlertText;
    public string badHitAlertText;
    public string missHitAlertText;
    public Color perfectHitAlertTextColour;
    public Color greatHitAlertTextColour;
    public Color goodHitAlertTextColour;
    public Color badHitAlertTextColour;
    public Color missHitAlertTextColour;
    public float hitAlertTriggerDuration = 1f;

    [SerializeField] private float missMultiplier = 0;
    [SerializeField] private float badMultiplier = 1;
    [SerializeField] private float goodMultiplier = 1;
    [SerializeField] private float greatMultiplier = 1;
    [SerializeField] private float perfectMultiplier = 1;

    public HitAreaManager hitAreaManager;

    public float calculatedPower;
    private bool enabled;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleEnabled(bool toggle)
    {
        enabled = toggle;
    }

    public void UpdateHitAreaType(HitAreaType hitAreaType)
    {
        curHitAreaType = hitAreaType;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (isStopped)
                return;

            // If user is on mobile, detect input differently then it would otherwise
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                if (Input.touchCount > 0)
                    StopWeapon();
            }
            else
                if (Input.GetMouseButton(0))
                    StopWeapon();
        }
    }

    private void FixedUpdate()
    {
        if (isStopped)
            return;

        MoveHitLine();
    }

    void MoveHitLine()
    {
        if (goingUp)
            hitLine.Translate(Vector2.up * weaponLineSpeed * Time.deltaTime);
        else
            hitLine.Translate(Vector2.down * weaponLineSpeed * Time.deltaTime);

        FlipHitLineDirection();
    }

    public void StopWeapon()
    {
        isStopped = true;

        StartCoroutine(StopHitLine());
    }

    public void ToggleAttackButtonInteractable(bool toggle)
    {
        attackButton.raycastTarget = toggle;
    }

    void FlipHitLineDirection()
    {
        if (goingUp)
        {
            if (Vector2.Distance(topBarBorder.position, hitLine.position) <= minDistanceBorderTrigger)
                goingUp = false;
        }
        else
        {
            if (Vector2.Distance(botBarBorder.position, hitLine.position) <= minDistanceBorderTrigger)
                goingUp = true;
        }
    }
    public void StartHitLine()
    {
        isStopped = false;

        hitAreaManager.UpdateHitAreaPos();
        GameManager.Instance.UpdateEnemyPosition(false);

        GameManager.Instance.ResetButton(GameManager.Instance.skill1Button);
        GameManager.Instance.ResetButton(GameManager.Instance.skill2Button);
        GameManager.Instance.ResetButton(GameManager.Instance.skill3Button);

        GameManager.Instance.ToggleAllowSelection(false);

        //GameManager.Instance.ResetButton(GameManager.Instance.endTurnButton);
    }
    public IEnumerator StopHitLine()
    {
        GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks
        GameManager.Instance.DisableButton(GameManager.Instance.weaponBackButton);

        ToggleEnabled(false);
        //GameManager.Instance.UpdateActiveUnitEnergyBar(false);

        GameManager.Instance.EnableSkill0Selection();

        GameManager.Instance.DisableAllSkillSelections(true);

        for (int i = 0; i < weaponHitAreas.Count; i++)
        {
            if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
            {
                // Power calculated here
                weaponHitAreas[i].StartCoroutine("HitArea");    
                break;
            }       
        }

        ToggleAttackButtonInteractable(false);

        yield return new WaitForSeconds(timePostHit);

        GameManager.Instance.ToggleAllowSelection(true);

        GameManager.Instance.SetupPlayerPostHitUI();

        GameManager.Instance.ResetButton(GameManager.Instance.weaponBackButton);    // Enable weapon back button only when damage has gone through

        isStopped = false;  // Resume attack bar 

        // Adjust power based on skill effect amp on target then send it 

        int effectHitAccuracy = 1;
        if (curHitAreaType == HitAreaType.PERFECT)
            effectHitAccuracy = 4;
        else if (curHitAreaType == HitAreaType.GREAT)
            effectHitAccuracy = 3;
        else if (curHitAreaType == HitAreaType.GOOD)
            effectHitAccuracy = 2;
        else if (curHitAreaType == HitAreaType.BAD)
            effectHitAccuracy = 1;
        else if (curHitAreaType == HitAreaType.MISS)
            effectHitAccuracy = 0;

        int hitAccuracy = 1;
        if (curHitAreaType == HitAreaType.PERFECT)
            hitAccuracy = 4;
        else if (curHitAreaType == HitAreaType.GREAT)
            hitAccuracy = 3;
        else if (curHitAreaType == HitAreaType.GOOD)
            hitAccuracy = 2;
        else if (curHitAreaType == HitAreaType.BAD)
            hitAccuracy = 1;
        else if (curHitAreaType == HitAreaType.MISS)
            hitAccuracy = 0;

        int finalHitCount = 0;

        if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
            finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillAttackCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitDamageHits();
        else
            finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillAttackCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitHealingHits();

        StartCoroutine(GameManager.Instance.WeaponAttackCommand((int)calculatedPower, finalHitCount, effectHitAccuracy));
    }

    public void DisableAlertUI()
    {
        hitAlertText.DisableAlertUI();
    }

    public void CalculatePower(WeaponHitArea.HitAreaType curHitAreaType)
    {
        float currentPower = GameManager.Instance.GetActiveUnitFunctionality().curPower;

        /*
        if (curHitAreaType == WeaponHitArea.HitAreaType.PERFECT)
            calculatedPower = perfectMultiplier * (GameManager.Instance.activeSkill.skillPower * (currentPower / 100f));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.GREAT)
            calculatedPower = greatMultiplier * (GameManager.Instance.activeSkill.skillPower * (currentPower / 100f));
        else if(curHitAreaType == WeaponHitArea.HitAreaType.GOOD)
            calculatedPower = goodMultiplier * (GameManager.Instance.activeSkill.skillPower * (currentPower / 100f));
        else if(curHitAreaType == WeaponHitArea.HitAreaType.BAD)
            calculatedPower = badMultiplier * (GameManager.Instance.activeSkill.skillPower * (currentPower / 100f));
        else if(curHitAreaType == WeaponHitArea.HitAreaType.MISS)
            calculatedPower = missMultiplier * (GameManager.Instance.activeSkill.skillPower * (currentPower / 100f));
        */

        calculatedPower = GameManager.Instance.GetActiveSkill().skillPower + currentPower;
        calculatedPower += GameManager.Instance.randomBaseOffset*2;
        calculatedPower = GameManager.Instance.RandomisePower((int)calculatedPower);
    }

    public void TriggerHitAlertText(WeaponHitArea.HitAreaType curHitAreaType)
    {
        if (curHitAreaType == WeaponHitArea.HitAreaType.PERFECT)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, perfectHitAlertText, perfectHitAlertTextColour));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.GREAT)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, greatHitAlertText, greatHitAlertTextColour));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.GOOD)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, goodHitAlertText, goodHitAlertTextColour));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.BAD)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, badHitAlertText, badHitAlertTextColour));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.MISS)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, missHitAlertText, missHitAlertTextColour));
    }

    /*
    private float GetPowerMultiplier()
    {

    }
    */
}
