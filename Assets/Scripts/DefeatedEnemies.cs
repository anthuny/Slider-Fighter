using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedEnemies : MonoBehaviour
{
    [SerializeField] private float timeWaitAfterTitle = .2f;
    [SerializeField] private float timeBetweenEnemiesDisplay = .25f;

    [SerializeField] private Transform defeatedEnemiesParent;
    [SerializeField] private GameObject enemiesPrefab;
    [SerializeField] private UIElement defeatedUnitsTextUI;

    public List<UnitFunctionality> defeatedEnemies = new List<UnitFunctionality>();

    private void Awake()
    {
        ToggleDefeatedUnitsText(false);
    }

    private void Start()
    {
        Setup();
    }

    public void ToggleDefeatedUnitsText(bool toggle)
    {
        if (toggle)
        {
            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            defeatedUnitsTextUI.UpdateAlpha(1);
        }
        else
            defeatedUnitsTextUI.UpdateAlpha(0);
    }

    public void Setup()
    {
        ResetDefeatedEnemies();
    }

    public void AddDefeatedEnemies(UnitFunctionality unit)
    {
        defeatedEnemies.Add(unit);
    }

    public void ResetDefeatedEnemies()
    {
        defeatedEnemies.Clear();

        foreach (Transform child in defeatedEnemiesParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayDefeatedEnemies()
    {
        ToggleDefeatedUnitsText(true);

        StartCoroutine(DisplayDefeatedEnemiesTime());
    }

    IEnumerator DisplayDefeatedEnemiesTime()
    {
        yield return new WaitForSeconds(timeWaitAfterTitle);

        for (int i = 0; i < defeatedEnemies.Count; i++)
        {
            GameObject go = Instantiate(enemiesPrefab, defeatedEnemiesParent.position, Quaternion.identity);
            go.transform.SetParent(defeatedEnemiesParent);
            go.transform.localScale = new Vector2(1, 1);

            UnitPortrait unitPortrait = go.GetComponent<UnitPortrait>();
            unitPortrait.UpdatePortrait(defeatedEnemies[i].GetUnitIcon());
            //unitPortrait.UpdatePortraitColour(defeatedEnemies[i].GetUnitColour());
            //unitPortrait.ToggleBg(true);
            unitPortrait.uIElement.AnimateUI(false);

            // Button Click SFX
            AudioManager.Instance.Play("Button_Click");

            yield return new WaitForSeconds(timeBetweenEnemiesDisplay);
        }
    }
}
