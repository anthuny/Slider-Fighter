using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRoomManager : MonoBehaviour
{
    public static HeroRoomManager Instance;

    [SerializeField] private UIElement promptUI;
    private UIElement heroRoomUI;

    [SerializeField] private Transform spawnLocation;
    [SerializeField] private ButtonFunctionality promptYesButton;
    [SerializeField] private ButtonFunctionality promptNoButton;

    [SerializeField] private float timeWaitAfterHeroJoining = 1.5f;

    private bool playerOffered;
    public bool playerInHeroRoomView;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHero(UIElement ui)
    {
        heroRoomUI = ui;
    }

    public UIElement GetHeroRoomUI()
    {
        return heroRoomUI;
    }

    public void SpawnHero()
    {
        GameManager.Instance.SpawnHero(true);





        GameManager.Instance.UpdateAllysPositionCombat();

        playerInHeroRoomView = true;

        GameManager.Instance.EnsureHeroIsDead();
        StartCoroutine(GameManager.Instance.HeroRetrievalScene(false));

        GameManager.Instance.UpdateActiveSkill(null);

        GameManager.Instance.ResetSelectedUnits();

        GameManager.Instance.combatOver = true;

        // Toggle player overlay and skill ui off
        GameManager.Instance.ToggleUIElement(GameManager.Instance.playerAbilities, false);
        GameManager.Instance.ToggleUIElement(GameManager.Instance.fighterSelectedMainSlotDesc, false);
        GameManager.Instance.ToggleUIElement(GameManager.Instance.endTurnButtonUI, false);
        //GameManager.Instance.ToggleUIElement(GameManager.Instance.turnOrder, false);
        GameManager.Instance.ResetActiveUnitTurnArrow();
        GameManager.Instance.ToggleAllAlliesStatBar(false);

        // Remove all unit effects and level image for item rewards
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            GameManager.Instance.activeRoomHeroes[i].ResetEffects();
        }
    }

    public Transform GetSpawnLocTrans()
    {
        return spawnLocation;
    }
    
    public void TogglePlayedOffered(bool toggle)
    {
        playerOffered = toggle;
    }

    public bool GetPlayerOffered()
    {
        return playerOffered;
    }

    public void TogglePrompt(bool toggle, bool denied = true, bool overrideSpawning = true)
    {
        promptYesButton.ToggleButton(toggle);
        promptNoButton.ToggleButton(toggle);

        GameManager.Instance.UpdateActiveSkill(null);

        if (!denied && overrideSpawning)
            GameManager.Instance.AddUnitToTeam(GameManager.Instance.spawnedUnit);


        if (toggle)
        {
            promptUI.UpdateAlpha(1);
            //promptUI.UpdateContentText()
        }
        else
        {
            GameManager.Instance.ResetAlliesExpVisual();

            //SpawnHeroGameManager();

            if (overrideSpawning)
            {
                TogglePlayedOffered(true);
                promptUI.UpdateAlpha(0);
                StartCoroutine(TimeWaitHerojoining());
            }
            else
            {
                TogglePlayedOffered(true);
                promptUI.UpdateAlpha(0);
                RemoveSpawnedUnit();
                playerInHeroRoomView = false;

                //GameManager.Instance.StartCoroutine(GameManager.Instance.SetupPostBattleUI(true));
            }

            GameManager.Instance.StartCoroutine(GameManager.Instance.SetupPostBattleUI(true));
        }
    }

    public void SpawnHeroGameManager()
    {
        if (GameManager.Instance.activeRoomHeroes.Count < 3)
            SpawnHero();
        else
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.SetupPostBattleUI(true));
        }
    }
    IEnumerator TimeWaitHerojoining()
    {
        yield return new WaitForSeconds(timeWaitAfterHeroJoining);

        //GetHeroRoomUI().UpdateAlpha(0);
        //Destroy(GetHeroRoomUI().gameObject);

        //GameManager.Instance.StartCoroutine(GameManager.Instance.SetupPostBattleUI(true));
        //StartCoroutine(GameManager.Instance.HeroRetrievalScene());
        GameManager.Instance.UpdateAllAlliesPosition(true);
        RoomManager.Instance.ToggleInteractable(false);

        // Set position of newly spawned hero room unit, based on how many allies player currently has
        if (GameManager.Instance.activeTeam.Count == 2)
            GameManager.Instance.spawnedUnitFunctionality.SetPositionAndParent(GameManager.Instance.allySpawnPositions[0]);
        else if (GameManager.Instance.activeTeam.Count == 3)
            GameManager.Instance.spawnedUnitFunctionality.SetPositionAndParent(GameManager.Instance.allySpawnPositions[2]);

        // Save earnt ally
        CharacterCarasel.Instance.SaveUnlockedAlly(GameManager.Instance.spawnedUnitFunctionality.GetUnitName());

        playerInHeroRoomView = false;
    }

    void RemoveSpawnedUnit()
    {
        GameManager.Instance.UpdateAllAlliesPosition(true);
        RoomManager.Instance.ToggleInteractable(false);

        GameManager.Instance.RemoveUnit(GameManager.Instance.spawnedUnitFunctionality);
        Debug.Log("Destroying unit " + GameManager.Instance.spawnedUnitFunctionality);



        StartCoroutine(DestroySpawnedUnit());
    }

    IEnumerator DestroySpawnedUnit()
    {
        yield return new WaitForSeconds(.5f);

        Destroy(GameManager.Instance.spawnedUnitFunctionality.gameObject);
    }
}
