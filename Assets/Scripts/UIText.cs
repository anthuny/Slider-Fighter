using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    TextMeshProUGUI mainText;

    private void Awake()
    {
        mainText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateUIText(string newText)
    {
        mainText.text = newText;
    }
}
