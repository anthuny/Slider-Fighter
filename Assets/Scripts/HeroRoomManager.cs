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
        GameManager.Instance.SpawnAllies(true);
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

    public void TogglePrompt(bool toggle, bool denied = true)
    {
        promptYesButton.ToggleButton(toggle);
        promptNoButton.ToggleButton(toggle);

        if (!denied)
            GameManager.Instance.AddUnitToTeam(GameManager.Instance.spawnedUnit);


        if (toggle)
        {
            promptUI.UpdateAlpha(1);
            //promptUI.UpdateContentText()
        }
        else
        {
            TogglePlayedOffered(true);

            promptUI.UpdateAlpha(0);

            if (denied)
                RemoveSpawnedUnit();
            else
                StartCoroutine(TimeWaitHerojoining());
        }
    }

    IEnumerator TimeWaitHerojoining()
    {
        yield return new WaitForSeconds(timeWaitAfterHeroJoining);

        //GetHeroRoomUI().UpdateAlpha(0);
        //Destroy(GetHeroRoomUI().gameObject);

        GameManager.Instance.StartCoroutine(GameManager.Instance.SetupRoomPostBattle(true));
        GameManager.Instance.UpdateAllAlliesPosition(true);
        RoomManager.Instance.ToggleInteractable(false);

        // Set position of newly spawned hero room unit, based on how many allies player currently has
        if (GameManager.Instance.activeTeam.Count == 2)
            GameManager.Instance.spawnedUnitFunctionality.SetPositionAndParent(GameManager.Instance.allySpawnPositions[0]);
        else if (GameManager.Instance.activeTeam.Count == 3)
            GameManager.Instance.spawnedUnitFunctionality.SetPositionAndParent(GameManager.Instance.allySpawnPositions[2]);

        // Save earnt ally
        CharacterCarasel.Instance.SaveUnlockedAlly(GameManager.Instance.spawnedUnitFunctionality.GetUnitName());
    }

    void RemoveSpawnedUnit()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.SetupRoomPostBattle(true));
        GameManager.Instance.UpdateAllAlliesPosition(true);
        RoomManager.Instance.ToggleInteractable(false);

        GameManager.Instance.RemoveUnit(GameManager.Instance.spawnedUnitFunctionality, false);

        Destroy(GameManager.Instance.spawnedUnitFunctionality.gameObject);
    }
}
