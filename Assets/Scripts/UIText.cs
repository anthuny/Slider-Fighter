using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    Text mainText;

    private void Awake()
    {
        mainText = GetComponent<Text>();
    }

    public void UpdateUIText(string newText)
    {
        mainText.text = newText;
    }
}
