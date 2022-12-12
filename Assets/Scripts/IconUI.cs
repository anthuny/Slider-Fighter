using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdatePortrait(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void UpdateColour(Color colour)
    {
        image.color = colour;
    }
}
