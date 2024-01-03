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
    private float originalWeaponLineSpeed;
    [SerializeField] private float lockoutTime;
    [SerializeField] private float lineSpeedIncPerHit = 50f;
    [SerializeField] private float pausedMovementTime = .2f;
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

    public HitAreaManager hitAreaManager;

    public float calculatedPower;
    private bool enabled;

    private bool attackBarDisabled;
    private float timerElapsed;

    int effectHitAccuracy = 0;
    int hitAccuracy = 0;

    private void Awake()
    {
        Instance = this;

        originalWeaponLineSpeed = weaponLineSpeed;
    }

    public void ResetAcc()
    {
        hitAccuracy = 0;
        effectHitAccuracy = 0;
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
                    HitWeapon();
            }
            else
                if (Input.GetMouseButton(0))
                    HitWeapon();
        }
    }

    private void FixedUpdate()
    {
        AttackBarDisabledTimer();

        if (isStopped)
            return;

        MoveHitLine();
    }

    void MoveHitLine()
    {
        FlipHitLineDirection();

        if (goingUp)
            hitLine.Translate(Vector2.up * weaponLineSpeed * Time.deltaTime);
        else
            hitLine.Translate(Vector2.down * weaponLineSpeed * Time.deltaTime);
    }

    public void HitWeapon()
    {
        // If attackbar is on cooldown, do not allow hit input
        if (attackBarDisabled)
            return;

        //isStopped = true;
        isStopped = true;

        //AudioManager.Instance.PauseAttackBarMusic(true);

        //Increase Hit Line Speed
        IncreaseHitLineSpeed(lineSpeedIncPerHit);

        StartCoroutine(StopHitLine());
    }

    void IncreaseHitLineSpeed(float time)
    {
        weaponLineSpeed += time;
    }

    void ResetHitLineSpeed()
    {
        weaponLineSpeed = originalWeaponLineSpeed;
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

            // If line is above top border
            if (topBarBorder.position.y < hitLine.position.y)
                goingUp = false;
        }
        else
        {
            if (Vector2.Distance(botBarBorder.position, hitLine.position) <= minDistanceBorderTrigger)
                goingUp = true;

            // If line is below bottom border
            if (botBarBorder.position.y > hitLine.position.y)
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

    void AttackBarDisabledTimer()
    {
        if (attackBarDisabled)
        {
            timerElapsed += Time.deltaTime;

            if (timerElapsed >= lockoutTime)
                ResetAttackBar();
        }

        else
        {
            if (timerElapsed != 0)
                timerElapsed = 0;
        }
    }

    void DisableAttackBar()
    {
        attackBarDisabled = true;
    }

    void ResetAttackBar()
    {
        attackBarDisabled = false;
    }

    public IEnumerator StopHitLine()
    {
        bool stopHitLine = false;

        for (int i = 0; i < weaponHitAreas.Count; i++)
        {
            if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
            {
                // Power calculated here
                weaponHitAreas[i].StartCoroutine("HitArea");

                // If player missed with recent tap, cause it to be the last
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.MISS)
                    stopHitLine = true;

                if (curHitAreaType == HitAreaType.PERFECT)
                    effectHitAccuracy += 4;
                else if (curHitAreaType == HitAreaType.GREAT)
                    effectHitAccuracy += 3;
                else if (curHitAreaType == HitAreaType.GOOD)
                    effectHitAccuracy += 2;
                else if (curHitAreaType == HitAreaType.BAD)
                    effectHitAccuracy += 1;
                else if (curHitAreaType == HitAreaType.MISS)
                    effectHitAccuracy += 0;

                if (curHitAreaType == HitAreaType.PERFECT)
                    hitAccuracy += 4;
                else if (curHitAreaType == HitAreaType.GREAT)
                    hitAccuracy += 3;
                else if (curHitAreaType == HitAreaType.GOOD)
                    hitAccuracy += 2;
                else if (curHitAreaType == HitAreaType.BAD)
                    hitAccuracy += 1;
                else if (curHitAreaType == HitAreaType.MISS)
                    hitAccuracy += 0;

                yield return new WaitForSeconds(pausedMovementTime);

                //AudioManager.Instance.PauseAttackBarMusic(false);

                isStopped = false;

                DisableAttackBar();

                break;
            }
        }

        if (stopHitLine)
        {
            isStopped = true;

            ResetHitLineSpeed();

            GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks
            GameManager.Instance.DisableButton(GameManager.Instance.weaponBackButton);

            ToggleEnabled(false);

            AudioManager.Instance.StopAttackBarMusic();

            GameManager.Instance.EnableSkill0Selection();
            GameManager.Instance.DisableAllSkillSelections(true);

            ToggleAttackButtonInteractable(false);

            yield return new WaitForSeconds(timePostHit);

            GameManager.Instance.ToggleAllowSelection(true);

            GameManager.Instance.SetupPlayerPostHitUI();

            GameManager.Instance.ResetButton(GameManager.Instance.weaponBackButton);    // Enable weapon back button only when damage has gone through

            isStopped = false;  // Resume attack bar 
           
            int finalHitCount = 0;

            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillAttackCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitDamageHits();
            else
                finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillAttackCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitHealingHits();

            // If user missed on first hit, send 1 hit count
            if (hitAccuracy == 0)
            {
                finalHitCount = 1;
                calculatedPower = 0;
            }

            if (effectHitAccuracy == 0)
            {
                finalHitCount = 1;
                calculatedPower = 0;
            }

            StartCoroutine(GameManager.Instance.WeaponAttackCommand((int)calculatedPower, finalHitCount, effectHitAccuracy));
        }
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
        */
        calculatedPower = (GameManager.Instance.GetActiveSkill().skillPower + currentPower) * 3;
        //calculatedPower += GameManager.Instance.randomBaseOffset*2;
        calculatedPower = GameManager.Instance.RandomisePower((int)calculatedPower);

        //if (curHitAreaType == WeaponHitArea.HitAreaType.MISS)
        //    calculatedPower = 0;
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
