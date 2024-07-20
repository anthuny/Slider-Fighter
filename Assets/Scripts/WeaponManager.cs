using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [HideInInspector]
    public enum HitAreaType { PERFECT, GREAT, GOOD, BAD, MISS }
    public HitAreaType curHitAreaType;

    public static WeaponManager Instance;

    [SerializeField] private List<HeroWeapon> heroWeapons = new List<HeroWeapon>();
    [SerializeField] private HeroWeapon enemyWeapon;
    public Transform hitLine;
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
    public bool isStopped;
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
    public Color flashOnColour;
    public Color flashOffColour;
    public float flashDuration;
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
    public int hitsRemaining;

    public bool autoHitPerfect;
    public bool autoHitGood;
    public bool autoHitBad;
    public bool autoHitMiss;

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

    public void SetEnemyWeapon(UnitFunctionality unit, bool resetWeapon = true)
    {
        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY || unit.reanimated)
        {
            hitLine = unit.heroWeapon.hitLine;
            topBarBorder = unit.heroWeapon.topBarBorder;
            botBarBorder = unit.heroWeapon.botBarBorder;
            weaponHitAreas = unit.heroWeapon.weaponHitAreas;
            //hitAlertText = enemyWeapon.hitAlertText;
            weaponUI = unit.heroWeapon.weaponUI;
            weaponLineSpeed = unit.heroWeapon.lineSpeed;
            hitAreaManager = unit.heroWeapon.hitAreaManager;
            hitAlertText = unit.heroWeapon.hitAlertText;
            hitsRemainingText = unit.heroWeapon.hitsRemainingText;
            hitsAccumulatedText = unit.heroWeapon.hitsAccumulatedText;
            unit.ToggleHeroWeapon();
        }

        //Debug.Log("a");
        if (unit.curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            unit.TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false, "", false, true);
        }

        StartHitLine(resetWeapon);
        StartCoroutine(CalculateEnemyHitAcc());
    }

    IEnumerator CalculateEnemyHitAcc()
    {
        //Debug.Log("1");
        yield return new WaitForSeconds(Random.Range(0.3f, 0.75f));

        int rand = Random.Range(1, 101);

        // high rand == lower accuracy
        // low rand = higher accuracy

        rand -= (GameManager.Instance.GetActiveUnitFunctionality().GetUnitLevel() * 3);
        rand += 6;

        if (rand > 100)
            rand = 100;

        if (rand < 1)
            rand = 1;

        // Force Re-animated units to hit good if they were going to miss
        if (GameManager.Instance.GetActiveUnitFunctionality().reanimated && rand >= 95)
            rand = 40;

        // Perfect
        if (rand >= 1 && rand <= 4 && !GameManager.Instance.GetActiveUnitFunctionality().hitPerfect) // old high = 4
        {
            GameManager.Instance.GetActiveUnitFunctionality().hitPerfect = true;

            curHitAreaType = HitAreaType.PERFECT;
            autoHitPerfect = true;
        }
        else if (rand >= 1 && rand <= 4 && GameManager.Instance.GetActiveUnitFunctionality().hitPerfect) // old high = 4
        {
            curHitAreaType = HitAreaType.GOOD;
            autoHitGood = true;
        }
        // Good
        else if (rand > 0 && rand <= 48)
        {
            curHitAreaType = HitAreaType.GOOD;
            autoHitGood = true;
        }
        // Bad
        else if (rand > 48 && rand <= 95)
        {
            curHitAreaType = HitAreaType.BAD;
            autoHitBad = true;
        }
        // Miss
        else if (rand > 95 && rand <= 100) // old miss 92
        {
            curHitAreaType = HitAreaType.MISS;
            autoHitMiss = true;
        }

        //Debug.Log("rand = " + rand);
    }
    private void FixedUpdate()
    {
        if (isStopped)
            return;

        // Detect needed hit accs
        if (autoHitPerfect)
        {
            for (int i = 0; i < weaponHitAreas.Count; i++)
            {
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.PERFECT)
                {
                    if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
                    {
                        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                            isStopped = true;



                        //yield return new WaitForSeconds(flashDuration / 2);

                        StartCoroutine(StopHitLine());
                        weaponHitAreas[i].SetHitLinePosition();
                        break;
                    }
                }
            }
        }
        else if (autoHitGood)
        {
            for (int i = 0; i < weaponHitAreas.Count; i++)
            {
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.GOOD)
                {
                    if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
                    {
                        //GameManager.Instance.GetActiveUnitFunctionality().heroWeapon.WeaponFlash(flashOnColour, flashOffColour, flashDuration);

                        //yield return new WaitForSeconds(flashDuration / 2);

                        StartCoroutine(StopHitLine());
                        weaponHitAreas[i].SetHitLinePosition();
                        break;
                    }
                }
            }
        }
        else if (autoHitBad)
        {
            for (int i = 0; i < weaponHitAreas.Count; i++)
            {
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.BAD)
                {
                    if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
                    {
                        //GameManager.Instance.GetActiveUnitFunctionality().heroWeapon.WeaponFlash(flashOnColour, flashOffColour, flashDuration);

                        //yield return new WaitForSeconds(flashDuration / 2);

                        StartCoroutine(StopHitLine());
                        weaponHitAreas[i].SetHitLinePosition();
                        break;
                    }
                }
            }
        }
        else if (autoHitMiss)
        {
            for (int i = 0; i < weaponHitAreas.Count; i++)
            {
                if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.MISS)
                {
                    if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
                    {
                        //GameManager.Instance.GetActiveUnitFunctionality().heroWeapon.WeaponFlash(flashOnColour, flashOffColour, flashDuration);

                        //yield return new WaitForSeconds(flashDuration / 2);

                        StartCoroutine(StopHitLine());
                        //weaponHitAreas[i].SetHitLinePosition();
                        break;
                    }
                }
            }
        }

        AttackBarDisabledTimer();
        MoveHitLine();
    }

    public void SetHeroWeapon(string unitName)
    {
        // Disable all weapons
        for (int x = 0; x < heroWeapons.Count; x++)
        {
            heroWeapons[x].gameObject.SetActive(false);
        }

        // Enable correct weapon
        for (int i = 0; i < heroWeapons.Count; i++)
        {
            if (heroWeapons[i].ownedUnitName == "Knight" && unitName == "Knight")
            {
                heroWeapons[i].gameObject.SetActive(true);

                hitLine = heroWeapons[i].hitLine;
                topBarBorder = heroWeapons[i].topBarBorder;
                botBarBorder = heroWeapons[i].botBarBorder;
                weaponHitAreas = heroWeapons[i].weaponHitAreas;
                hitAlertText = heroWeapons[i].hitAlertText;
                weaponUI = heroWeapons[i].weaponUI;
                weaponLineSpeed = heroWeapons[i].lineSpeed;
                hitAreaManager = heroWeapons[i].hitAreaManager;
                hitsRemainingText = heroWeapons[i].hitsRemainingText;
                hitsAccumulatedText = heroWeapons[i].hitsAccumulatedText;
                break;
            }
            else if (heroWeapons[i].ownedUnitName == "Ranger" && unitName == "Ranger")
            {
                heroWeapons[i].gameObject.SetActive(true);

                hitLine = heroWeapons[i].hitLine;
                topBarBorder = heroWeapons[i].topBarBorder;
                botBarBorder = heroWeapons[i].botBarBorder;
                weaponHitAreas = heroWeapons[i].weaponHitAreas;
                hitAlertText = heroWeapons[i].hitAlertText;
                weaponUI = heroWeapons[i].weaponUI;
                weaponLineSpeed = heroWeapons[i].lineSpeed;
                hitAreaManager = heroWeapons[i].hitAreaManager;
                hitsRemainingText = heroWeapons[i].hitsRemainingText;
                hitsAccumulatedText = heroWeapons[i].hitsAccumulatedText;
                break;
            }
            else if (heroWeapons[i].ownedUnitName == "Cleric" && unitName == "Cleric")
            {
                heroWeapons[i].gameObject.SetActive(true);

                hitLine = heroWeapons[i].hitLine;
                topBarBorder = heroWeapons[i].topBarBorder;
                botBarBorder = heroWeapons[i].botBarBorder;
                weaponHitAreas = heroWeapons[i].weaponHitAreas;
                hitAlertText = heroWeapons[i].hitAlertText;
                weaponUI = heroWeapons[i].weaponUI;
                weaponLineSpeed = heroWeapons[i].lineSpeed;
                hitAreaManager = heroWeapons[i].hitAreaManager;
                hitsRemainingText = heroWeapons[i].hitsRemainingText;
                hitsAccumulatedText = heroWeapons[i].hitsAccumulatedText;
                break;
            }
            else if (heroWeapons[i].ownedUnitName == "Necromancer" && unitName == "Necromancer")
            {
                heroWeapons[i].gameObject.SetActive(true);

                hitLine = heroWeapons[i].hitLine;
                topBarBorder = heroWeapons[i].topBarBorder;
                botBarBorder = heroWeapons[i].botBarBorder;
                weaponHitAreas = heroWeapons[i].weaponHitAreas;
                hitAlertText = heroWeapons[i].hitAlertText;
                weaponUI = heroWeapons[i].weaponUI;
                weaponLineSpeed = heroWeapons[i].lineSpeed;
                hitAreaManager = heroWeapons[i].hitAreaManager;
                hitsRemainingText = heroWeapons[i].hitsRemainingText;
                hitsAccumulatedText = heroWeapons[i].hitsAccumulatedText;
                break;
            }
            else if (heroWeapons[i].ownedUnitName == "Monk" && unitName == "Monk")
            {
                heroWeapons[i].gameObject.SetActive(true);

                hitLine = heroWeapons[i].hitLine;
                topBarBorder = heroWeapons[i].topBarBorder;
                botBarBorder = heroWeapons[i].botBarBorder;
                weaponHitAreas = heroWeapons[i].weaponHitAreas;
                hitAlertText = heroWeapons[i].hitAlertText;
                weaponUI = heroWeapons[i].weaponUI;
                weaponLineSpeed = heroWeapons[i].lineSpeed;
                hitAreaManager = heroWeapons[i].hitAreaManager;
                hitsRemainingText = heroWeapons[i].hitsRemainingText;
                hitsAccumulatedText = heroWeapons[i].hitsAccumulatedText;
                break;
            }
        }
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

    public void UpdateWeaponDetails(bool playerMissed = false, bool hero = true)
    {
        hitsRemaining = GameManager.Instance.GetActiveSkill().skillHitAttempts - hitsPerformed;

        if (hitsRemaining >= 3)
        {
            hitsRemainingText.UpdateContentTextColour(threeHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(threeHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            //if (hero)
                hitAreaManager.UpdateHitAreaPos();
        }
        else if (hitsRemaining == 2)
        {
            hitsRemainingText.UpdateContentTextColour(twoHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(twoHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            //if (hero)
                hitAreaManager.UpdateHitAreaPos();
        }
        else if (hitsRemaining == 1)
        {
            hitsRemainingText.UpdateContentTextColour(oneHitRemainingTextColour);
            hitsRemainingText.UpdateContentSubTextTMPColour(oneHitRemainingTextColour);
            hitsRemainingText.UpdateAlpha(1);
            //if (hero)
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

    public IEnumerator UpdateWeaponAccumulatedHits(int hits, bool doExtras = true)
    {
        for (int i = 0; i < hits; i++)
        {
            accumulatedHits++;
            hitsAccumulatedText.UpdateContentText(accumulatedHits.ToString());

            if (doExtras)
            {
                hitsAccumulatedText.AnimateUI();

                // Button Click SFX
                AudioManager.Instance.Play("Button_Click");
            }

            if (hits <= 5)
            {
                yield return new WaitForSeconds(accumulatedHitsTimeBetween);
            }
            else
                yield return new WaitForSeconds(0.01f);
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



    void MoveHitLine()
    {
        if (!isStopped)
        {
            FlipHitLineDirection();

            if (goingUp)
                hitLine.Translate(Vector2.up * weaponLineSpeed * Time.deltaTime);
            else
                hitLine.Translate(Vector2.down * weaponLineSpeed * Time.deltaTime);
        }
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
    public void StartHitLine(bool resetAcc = true)
    {
        //Debug.Log("starting hit line");
        isStopped = false;

        autoHitMiss = false;
        autoHitPerfect = false;
        autoHitGood = false;
        autoHitBad = false;

        if (resetAcc)
        {
            ResetAcc();
            ResetWeaponAccHits();
        }

        //hitAreaManager.UpdateHitAreaPos();
        
        if (resetAcc)
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                StartCoroutine(UpdateWeaponAccumulatedHits(hitAccuracy + GameManager.Instance.GetActiveSkill().skillBaseHitOutput + GameManager.Instance.GetActiveUnitFunctionality().GetUnitLevel()-1, true));
            else
                StartCoroutine(UpdateWeaponAccumulatedHits(hitAccuracy + GameManager.Instance.GetActiveSkill().skillBaseHitOutput - 2, true));
        }
        else
        {
            //StartCoroutine(UpdateWeaponAccumulatedHits(hitAccuracy, true));
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY || GameManager.Instance.GetActiveUnitFunctionality().reanimated)
        {
            ToggleHitAreasDisplay(true);
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
        {
            CombatGridManager.Instance.ToggleCombatGrid(false);
        }

        ToggleWeaponAccHits(true);
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
        //sDebug.Log("stopping hit line");

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.ENEMY)
        {
            CombatGridManager.Instance.ToggleCombatGrid(false);
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)         
            stopHitLine = false;
        else
        {
            isStopped = true;
            stopHitLine = false;

            GameManager.Instance.GetActiveUnitFunctionality().heroWeapon.WeaponFlash(flashOnColour, flashOffColour, flashDuration);

            //yield return new WaitForSeconds(flashDuration / 2f);
        }

        if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
        {
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



                    if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER)
                        yield return new WaitForSeconds(pausedMovementTime);

                    ToggleWeaponAccHits(true);

                    isStopped = false;

                    if (curHitAreaType == HitAreaType.MISS)
                    {
                        ToggleWeaponHitsRemainingText(false);
                        UpdateWeaponDetails(true, false);
                    }
                    else
                    {
                        //if (stopHitLine == false)
                        //{
                        UpdateWeaponDetails(false, true);
                        //}
                    }

                    DisableAttackBar();

                    break;
                }
            }
        }
        else
        {
            if (autoHitPerfect)
            {
                for (int i = 0; i < weaponHitAreas.Count; i++)
                {
                    if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.PERFECT)
                    {
                        // Power calculated here
                        weaponHitAreas[i].StartCoroutine("HitArea");
                        weaponHitAreas[i].SetHitLinePosition();
                        UpdateWeaponDetails(false, false);
                        break;
                    }
                }
            }
            else if (autoHitGood)
            {
                for (int i = 0; i < weaponHitAreas.Count; i++)
                {
                    if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.GOOD)
                    {
                        hitsPerformed++;

                        // Power calculated here
                        weaponHitAreas[i].StartCoroutine("HitArea");
                        weaponHitAreas[i].SetHitLinePosition();
                        UpdateWeaponDetails(false, false);
                        break;
                    }
                }
            }
            else if (autoHitBad)
            {
                for (int i = 0; i < weaponHitAreas.Count; i++)
                {
                    if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.BAD)
                    {
                        hitsPerformed++;

                        // Power calculated here
                        weaponHitAreas[i].StartCoroutine("HitArea");
                        weaponHitAreas[i].SetHitLinePosition();
                        UpdateWeaponDetails(false, false);
                        break;
                    }
                }
            }
            else if (autoHitMiss)
            {
                for (int i = 0; i < weaponHitAreas.Count; i++)
                {
                    if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.MISS)
                    {
                        hitsPerformed++;

                        // Power calculated here
                        weaponHitAreas[i].StartCoroutine("HitArea");
                        ToggleHitAreasDisplay(false);
                        //weaponHitAreas[i].SetHitLinePosition();

                        // If player missed with recent tap, cause it to be the last
                        if (weaponHitAreas[i].curHitAreaType == WeaponHitArea.HitAreaType.MISS)
                            stopHitLine = true;

                        UpdateWeaponDetails(true, false);
                        break;
                    }
                }
            }

            int acc = 0;

            // If unit hit perfect, give them another hit
            if (curHitAreaType == HitAreaType.PERFECT)
            {
                hitAccuracy += 3;
                acc += 3;
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

            //

            if (curHitAreaType == HitAreaType.MISS)
            {
                ToggleWeaponHitsRemainingText(false);
                UpdateWeaponDetails(true, false);
            }
            else
            {
                if (stopHitLine == false)
                {
                    UpdateWeaponDetails(false, false);
                }
            }

            DisableAttackBar();

            //isStopped = false;

            if (hitsRemaining >= 1)
            {
                SetEnemyWeapon(GameManager.Instance.GetActiveUnitFunctionality(), false);
            }

            if (curHitAreaType != HitAreaType.MISS)
                UpdateWeaponDetails(false, false);

            if (hitsRemaining <= 0)
            {
                stopHitLine = true;
                isStopped = true;
            }
            //else
                //isStopped = false;
        }


        if (stopHitLine && hitsRemaining <= 0)
        {
            isStopped = true;

            ResetHitLineSpeed();

            GameManager.Instance.ResetButton(GameManager.Instance.attackButton);    // Allow attack button clicks
            GameManager.Instance.DisableButton(GameManager.Instance.weaponBackButton);

            ToggleEnabled(false);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            {
                AudioManager.Instance.StopAttackBarMusic();
            }


            //GameManager.Instance.EnableFreeSkillSelection();
            GameManager.Instance.DisableAllMainSlotSelections(true);

            ToggleAttackButtonInteractable(false);

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                yield return new WaitForSeconds(timePostHit);

            GameManager.Instance.SetupPlayerPostHitUI();

            GameManager.Instance.ResetButton(GameManager.Instance.weaponBackButton);    // Enable weapon back button only when damage has gone through

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                isStopped = true;  // Resume attack bar 
            else
                isStopped = true;  // Resume attack bar 

            int finalHitCount = 0;

            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillBaseHitOutput + GameManager.Instance.GetActiveUnitFunctionality().GetUnitPowerHits() + GameManager.Instance.GetActiveSkill().upgradeIncHitsCount-1;
            else
                finalHitCount = hitAccuracy + GameManager.Instance.GetActiveSkill().skillBaseHitOutput + GameManager.Instance.GetActiveUnitFunctionality().GetUnitHealingHits() + GameManager.Instance.GetActiveSkill().upgradeIncHitsCount-1;
            // If user missed on first hit, send 1 hit count
            bool miss = false;

            CalculatePower(GameManager.Instance.isSkillsMode);

            if (hitAccuracy == 0)
            {
                miss = true;
                finalHitCount = 1;
                calculatedPower = 0;
            }

            if (effectHitAccuracy == 0)
            {
                finalHitCount = 1;
                calculatedPower = 0;
            }

            int effectCount = 0;


            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                effectCount = GameManager.Instance.GetActiveSkill().baseEffectApplyCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitPowerHits() + effectHitAccuracy-1;
            else
                effectCount = GameManager.Instance.GetActiveSkill().baseEffectApplyCount + GameManager.Instance.GetActiveUnitFunctionality().GetUnitHealingHits() + effectHitAccuracy-1;

            if (GameManager.Instance.GetActiveUnitFunctionality().curUnitType == UnitFunctionality.UnitType.PLAYER && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
            {
                finalHitCount++;
                effectCount += 2;
            }
            else
                effectCount++;

            StartCoroutine(GameManager.Instance.WeaponAttackCommand((int)calculatedPower, finalHitCount, effectCount, miss));
        }
    }

    public void ToggleHitAreasDisplay(bool toggle = true)
    {
        for (int i = 0; i < weaponHitAreas.Count; i++)
        {
            weaponHitAreas[i].ToggleHitAreaDisplay(toggle);
        }
    }
    public void DisableAlertUI()
    {
        if (hitAlertText != null)
            hitAlertText.DisableAlertUI();
    }

    public void CalculatePower(bool skill = true)
    {
        float currentPower = GameManager.Instance.GetActiveUnitFunctionality().curPower;

        if (skill)
        {
            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT && GameManager.Instance.GetActiveSkill().curSkillPower != 0 ||
                GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.SUPPORT && GameManager.Instance.GetActiveSkill().healPowerAmount != 0)
                currentPower += GameManager.Instance.GetActiveUnitFunctionality().curHealingPower;

            calculatedPower = (GameManager.Instance.GetActiveSkill().GetCalculatedSkillPowerStat() + currentPower);
            //calculatedPower += GameManager.Instance.randomBaseOffset*2;
            calculatedPower = GameManager.Instance.RandomisePower((int)calculatedPower);
        }
        else
        {
            calculatedPower = currentPower + GameManager.Instance.GetActiveItem().itemPower;
            //calculatedPower += GameManager.Instance.randomBaseOffset*2;
            calculatedPower = GameManager.Instance.RandomisePower((int)calculatedPower);
        }
    }

    public void TriggerHitAlertText(WeaponHitArea.HitAreaType curHitAreaType)
    {
        if (hitAlertText.displayingAlert)
            hitAlertText.DisableAlertUI();

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
