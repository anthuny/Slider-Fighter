using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{
    [SerializeField] private Transform rewardsParent;
    [SerializeField] private GameObject reward;
    [SerializeField] private Sprite goldSprite;
        
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        ResetRewardsTable();
    }

    public void FillRewardsTable(int roomDifficulty)
    {
        ResetRewardsTable();

        GiveGold();


        // Give item(s)

    }

    void ResetRewardsTable()
    {
        foreach (Transform child in rewardsParent)
        {
            Destroy(child.gameObject);
        }
    }

    void GiveGold()
    {       
        // Give gold
        GameObject go = Instantiate(reward, rewardsParent.position, Quaternion.identity);
        go.transform.SetParent(rewardsParent);
        go.transform.localScale = new Vector2(1, 1);

        UIElement uiElement = go.GetComponent<UIElement>();
        uiElement.UpdateContentImage(goldSprite);
        uiElement.UpdateContentText(CalculateGoldRecieved().ToString());
    }

    int CalculateGoldRecieved()
    {
        int gold = (GameManager.instance.goldGainedPerUnit * GameManager.instance.GetEnemiesKilledCount());
        return gold;
    }
}
