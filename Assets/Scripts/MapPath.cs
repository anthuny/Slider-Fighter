using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject middleOfPath;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void UpdateMapPath(Vector2 posA, Vector2 posB)
    {
        lr.SetPosition(0, posA);
        lr.SetPosition(1, posB);

        middleOfPath.transform.position = Vector2.Lerp(posA, posB, .5f);
    }
}
