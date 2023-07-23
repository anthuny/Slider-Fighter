using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [SerializeField] private GameObject shopForest;
    [SerializeField] private GameObject combatForest;

    //[SerializeField] private GameObject BackgroundParent;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject GetShopForest()
    {
        return shopForest;
    }

    public GameObject GetCombatForest()
    {
        return combatForest;
    }

    public void UpdateBackground(GameObject background)
    {
        // Disable all other BGs
        if (background == combatForest)
        {
            shopForest.gameObject.GetComponent<TilemapRenderer>().enabled = false;
            shopForest.gameObject.GetComponent<UIElement>().UpdateAlpha(0);

        }
        else if (background == shopForest)
        {
            combatForest.gameObject.GetComponent<TilemapRenderer>().enabled = false;
            combatForest.gameObject.GetComponent<UIElement>().UpdateAlpha(0);
        }
          
        // Enable background user wants
        background.gameObject.GetComponent<TilemapRenderer>().enabled = true;
        background.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
    }
}
