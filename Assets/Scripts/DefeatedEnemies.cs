using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedEnemies : MonoBehaviour
{
    [SerializeField] private Transform defeatedEnemiesParent;
    [SerializeField] private GameObject enemiesPrefab;

    public List<UnitFunctionality> defeatedEnemies = new List<UnitFunctionality>();
    [SerializeField] private float timeBetweenEnemiesDisplay;

    private void Start()
    {
        Setup();
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
        StartCoroutine(DisplayDefeatedEnemiesTime());
    }

    IEnumerator DisplayDefeatedEnemiesTime()
    {
        for (int i = 0; i < defeatedEnemies.Count; i++)
        {
            GameObject go = Instantiate(enemiesPrefab, defeatedEnemiesParent.position, Quaternion.identity);
            go.transform.SetParent(defeatedEnemiesParent);
            go.transform.localScale = new Vector2(1, 1);

            UnitPortrait unitPortrait = go.GetComponent<UnitPortrait>();
            unitPortrait.UpdatePortrait(defeatedEnemies[i].GetUnitSprite());
            unitPortrait.UpdatePortraitColour(defeatedEnemies[i].GetUnitColour());
            unitPortrait.ToggleBg(true);

            yield return new WaitForSeconds(timeBetweenEnemiesDisplay);
        }
    }
}
