using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeroWeapon : MonoBehaviour
{
    public int lineSpeed;
    public string ownedUnitName;
    public Transform hitLine;
    public Transform topBarBorder;
    public Transform botBarBorder;
    public List<WeaponHitArea> weaponHitAreas = new List<WeaponHitArea>();
    public UIElement hitAlertText;
    public UIElement weaponUI;
    public HitAreaManager hitAreaManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
