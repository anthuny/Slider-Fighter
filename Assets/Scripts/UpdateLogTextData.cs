using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Update Log", menuName = "Update Log")]
public class UpdateLogTextData : ScriptableObject
{
    [TextArea(1, 2)]
    public string updateLogPreText;
    [TextArea(4, 8)]
    public string updateLogText;
}
