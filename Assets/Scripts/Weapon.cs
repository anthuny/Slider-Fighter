using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public enum HitAreaType { PERFECT, GREAT, GOOD, BAD, MISS }
    public HitAreaType curHitAreaType;

    public static Weapon instance;

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

    public float calculatedPower;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHitAreaType(HitAreaType hitAreaType)
    {
        curHitAreaType = hitAreaType;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopped)
            return;

        // If user is on mobile, detect input differently then it would otherwise
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount > 0)
                ResetWeapon();
        }
        else 
            if (Input.GetMouseButton(0))
                ResetWeapon();
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

    void ResetWeapon()
    {
        isStopped = true;
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

        GameManager.Instance.UpdateEnemyPosition(false);

        GameManager.Instance.ResetButton(GameManager.Instance.skill1Button);
        GameManager.Instance.ResetButton(GameManager.Instance.skill2Button);
        GameManager.Instance.ResetButton(GameManager.Instance.skill3Button);
        GameManager.Instance.ResetButton(GameManager.Instance.endTurnButton);
    }
    public IEnumerator StopHitLine()
    {
        if (!isStopped)
            yield break;

        for (int i = 0; i < weaponHitAreas.Count; i++)
        {
            if (weaponHitAreas[i].CheckIfHitLineHit(hitLine.gameObject))
            {
                // Power calculated here
                weaponHitAreas[i].HitArea();
                break;
            }       
        }

        ToggleAttackButtonInteractable(false);

        yield return new WaitForSeconds(timePostHit);

        GameManager.Instance.SetupPlayerPostHitUI();

        GameManager.Instance.ResetButton(GameManager.Instance.weaponBackButton);    // Enable weapon back button only when damage has gone through

        isStopped = false;  // Resume attack bar 

        // Adjust power based on skill effect amp on target then send it 

        int hitMulticount = 1;
        if (curHitAreaType == HitAreaType.PERFECT)
            hitMulticount = 4;
        else if (curHitAreaType == HitAreaType.GREAT)
            hitMulticount = 3;
        else if (curHitAreaType == HitAreaType.GOOD)
            hitMulticount = 2;
        else if (curHitAreaType == HitAreaType.BAD)
            hitMulticount = 1;
        else if (curHitAreaType == HitAreaType.MISS)
            hitMulticount = 0;

        StartCoroutine(GameManager.Instance.WeaponAttackCommand((int)calculatedPower, hitMulticount));
    }

    public void DisableAlertUI()
    {
        hitAlertText.DisableAlertUI();
    }

    public void CalculatePower(WeaponHitArea.HitAreaType curHitAreaType)
    {
        float currentPower = GameManager.Instance.GetActiveUnitFunctionality().curPower;

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

        calculatedPower *= 10;      
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
