using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostBattle : MonoBehaviour
{
    [SerializeField] private Text postBattleConditionText;
    private UIElement postBattleUI;

    private void Awake()
    {
        postBattleUI = GetComponent<UIElement>();
    }
    void Start()
    {
        TogglePostBattleUI(false);
    }

    public void TogglePostBattleUI(bool toggle)
    {
        GameManager.instance.ToggleUIElement(postBattleUI, toggle);
    }
}
