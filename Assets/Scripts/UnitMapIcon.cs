using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMapIcon : MonoBehaviour
{
    public void UpdateUnitPosition(Vector2 pos)
    {
        //gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }
}
