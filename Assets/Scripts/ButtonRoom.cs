using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRoom : MonoBehaviour
{
    [HideInInspector]
    public RoomMapIcon room;

    public void SelectRoom()
    {
        room.SelectRoom();
    }
}
