using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private UIElement hitsRemainingText;
    [SerializeField] private UIElement hitsAccumulatedText;
    [SerializeField] private float accumulatedHitsTimeBetween = 15f;
    [SerializeField] private Color oneHitRemainingTextColour;
    [SerializeField] private Color twoHitRemainingTextColour;
    [SerializeField] private Color threeHitRemainingTextColour;
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
    [SerializeField] private UIElement weaponUI;
    public string perfectHitAlertText;
    public string greatHitAlertText;
    public string goodHitAlertText;
    public string badHitAlertText;
    public string missHitAlertText;
    public Color defaultColour;
    public TMP_ColorGradient perfectHitAlertTextGradient;
    public TMP_ColorGradient greatHitAlertTextGradient;
    public TMP_ColorGradient goodHitAlertTextGradient;
    public TMP_ColorGradient badHitAlertTextGradient;
    public TMP_ColorGradient missHitAlertTextGradient;
    public float hitAlertTriggerDuration = 1f;

    public HitAreaManager hitAreaManager;

    public float calculatedPower;
    private bool enabled;

    private bool attackBarDisabled;
    private float timerElapsed;

    int effectHitAccuracy = 0;
    int hitAccuracy = 0;
    int hitsPerformed;
    int accumulatedHits = 0;

    bool stopHitLine;
    int hitsRemaining;

    private void Awake()
    {
        Instance = this;

        originalWeaponLineSpeed = weaponLineSpeed;
    }

    public void ResetAcc()
    {
        hitAccuracy = 0;
        effectHitAccuracy = 0;
        hitsPerformed = 0;
        accumulatedHits = 0;
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

    public void ResetWeapon()
    {
        hitsAccumulatedText.UpdateAlpha(0);
        hitsRemainingText.UpdateAlpha(0);
    }

    public void UpdateWeaponDetails(bool playerMissed = false)
    {
        hitsRemaining = GameManager.Instance.GetActiveSkill().skillAttackCount - hitsPerformed;

        if (hitsRemaining >= 3)
        {
            hitsRemainingText.UpdateContentTextColour(threeHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(threeHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            hitAreaManager.UpdateHitAreaPos();
        }
        else if (hitsRemaining == 2)
        {
            hitsRemainingText.UpdateContentTextColour(twoHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(twoHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            hitAreaManager.UpdateHitAreaPos();
        }
        else if (hitsRemaining == 1)
        {
            hitsRemainingText.UpdateContentTextColour(oneHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(oneHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            hitAreaManager.UpdateHitAreaPos();
        }
        else if (hitsRemaining == 0)
        {
            stopHitLine = true;
            hitsRemainingText.ToggleContentSubTextTMP(false);
            hitsRemainingText.UpdateAlpha(0);
        }

        if (playerMissed)
        {
            stopHitLine = true;
            hitsRemainingText.ToggleContentSubTextTMP(false);
            hitsRemainingText.UpdateAlpha(0);
        }
        else
        {
            //hitAreaManager.UpdateHitAreaPos();

            hitsRemainingText.UpdateContentText(hitsRemaining.ToString());

            hitsRemainingText.AnimateUI();
        }
    }

    public IEnumerator UpdateWeaponAccumulatedHits(int hits)
    {
        for (int i = 0; i < hits; i++)
        {
            accumulatedHits++;
            hitsAccumulatedText.UpdateContentText(accumulatedHits.ToString());

            hitsAccumulatedText.AnimateUI();

            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            yield return new WaitForSeconds(accumulatedHitsTimeBetween);
        }
    }

    void ResetWeaponAccHits()
    {
        hitsAccumulatedText.UpdateContentText(0.ToString());
        //GameManager.Instance.GetActiveUnitFunctionality().hits
    }

    public void ToggleWeaponAccHits(bool toggle)
    {
        if (toggle)
            hitsAccumulatedText.UpdateAlpha(1);
        else
            hitsAccumulatedText.UpdateAlpha(0);
    }

    public void ToggleWeaponHitsRemainingText(bool toggle)
    {
        if (toggle)
        {
            hitsRemainingText.UpdateAlpha(1);
            hitsRemainingText.ToggleContentSubTextTMP(true);
        }
        else
        {
            hitsRemainingText.UpdateAlpha(0);
            hitsRemainingText.ToggleContentSubTextTMP(false);
        }
    }

    public void AnimateWeaponUI()
    {
        weaponUI.AnimateUI(false);
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

        ResetAcc();

        ResetWeaponAccHits();

        hitAreaManager.UpdateHitAreaPos();

        ToggleWeaponHitsRemainingText(true);

        UpdateWeaponDetails();
        //StartCoroutine(UpdateWeaponAccumulatedHits());
        //ToggleWeaponAccHits(false);
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
        stopHitLine = false;

        for (int i = 0; i < weaponHitAreas.Count; i++)
        {
            if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
            {
                // Power calculated here
                weaponHitAreas[i].StartCoroutine("HitArea");

                // If player missed with recent tap, cause it to be the last
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.MISS)
                    stopHitLine = true;

                int acc = 0;

                // If unit hit perfect, give them another hit
                if (curHitAreaType == HitAreaType.PERFECT)
                {
                    hitAccuracy += 3;
                    acc += 3;
                }
                else if (curHitAreaType == HitAreaType.GREAT)
                {
                    hitAccuracy += 3;
                    acc += 3;
                    hitsPerformed++;
                }
                else if (curHitAreaType == HitAreaType.GOOD)
                {
                    hitAccuracy += 2;
                    acc += 2;
                    hitsPerformed++;
                }
                else if (curHitAreaType == HitAreaType.BAD)
                {
                    hitAccuracy += 1;
                    acc += 1;
                    hitsPerformed++;
                }
                else if (curHitAreaType == HitAreaType.MISS)
                {
                    hitAccuracy += 0;
                    acc += 0;
                    hitsPerformed++;
                }

                if (curHitAreaType == HitAreaType.PERFECT)
                    effectHitAccuracy += 3;
                else if (curHitAreaType == HitAreaType.GREAT)
                    effectHitAccuracy += 3;
                else if (curHitAreaType == HitAreaType.GOOD)
                    effectHitAccuracy += 2;
                else if (curHitAreaType == HitAreaType.BAD)
                    effectHitAccuracy += 1;
                else if (curHitAreaType == HitAreaType.MISS)
                    effectHitAccuracy += 0;

                AnimateWeaponUI();

                StartCoroutine(UpdateWeaponAccumulatedHits(acc));



                yield return new WaitForSeconds(pausedMovementTime);

                ToggleWeaponAccHits(true);

                isStopped = false;

                if (curHitAreaType == HitAreaType.MISS)
                {
                    ToggleWeaponHitsRemainingText(false);
                    UpdateWeaponDetails(true);
                }
                else
                {
                    if (stopHitLine == false)
                    {
                        UpdateWeaponDetails();
                    }
                }

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
                finalHitCount = hitAccuracy;
            else
                finalHitCount = hitAccuracy;

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
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, perfectHitAlertText, perfectHitAlertTextGradient));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.GREAT)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, greatHitAlertText, greatHitAlertTextGradient));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.GOOD)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, goodHitAlertText, goodHitAlertTextGradient));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.BAD)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, badHitAlertText, badHitAlertTextGradient));
        else if (curHitAreaType == WeaponHitArea.HitAreaType.MISS)
            StartCoroutine(hitAlertText.TriggerUIAlert(hitAlertTriggerDuration, missHitAlertText, missHitAlertTextGradient));
    }

    /*
    private float GetPowerMultiplier()
    {

    }
    */
}
