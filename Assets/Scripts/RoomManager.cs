using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager roomManager;
    public List<Room> totalRooms = new List<Room>();
    private Room activeRoom;

    private void Awake()
    {
        roomManager = this;
    }

    private void Start()
    {
        //ChooseRoom();
    }

    public void ChooseRoom()
    {
        int rand = Random.Range(0, totalRooms.Count - 1);
        activeRoom = totalRooms[rand];

        GameManager.instance.StartRoom(activeRoom);
    }
}
