using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject middleOfPath;
    public RoomMapIcon startingRoom;
    public List<RoomMapIcon> goalRooms = new List<RoomMapIcon>();
    public bool isRevealed;
    public int belongsToFloorCount = 1;

    public GameObject display;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void TogglePathVisibility(bool toggle = true)
    {
        lr.enabled = toggle;
    }

    public void UpdateMapPath(Vector2 posA, Vector2 posB)
    {
        lr.SetPosition(0, posA);
        lr.SetPosition(1, posB);

        middleOfPath.transform.position = Vector2.Lerp(posA, posB, .5f);
        
        /*
        GameObject Go = Instantiate(display, posA, Quaternion.identity);
        GameObject Go2 = Instantiate(display, posB, Quaternion.identity);
        Go.transform.SetParent(this.gameObject.transform);
        Go2.transform.SetParent(this.gameObject.transform);
        Go.transform.position = posA;
        Go2.transform.position = posB;
        Go.transform.localScale = Vector2.one;
        Go2.transform.localScale = Vector2.one;
        */
    }

    public void UpdateStartingRoom(RoomMapIcon room)
    {
        startingRoom = room;
    }

    public void AddGoalRooms(RoomMapIcon room)
    {
        goalRooms.Add(room);
    }

    public void ToggleHiddenMode(bool toggle)
    {
        if (toggle)
        {
            lr.enabled = false;
            isRevealed = false;
        }
        else
        {
            lr.enabled = true;
            isRevealed = true;

            for (int i = 0; i < goalRooms.Count; i++)
            {
                // Enable each goal room connected to this path
                goalRooms[i].ToggleHiddenMode(false);

                // Make the main room of the goal rooms the next revealed room
                if (goalRooms[i].isMainRoom)
                {
                    //Debug.Log("updating revealed room to " + goalRooms[i].name);
                    GameManager.Instance.map.UpdateRevealedMainRoom(goalRooms[i]);
                    break;
                }
            }
        }
    }

    public void ToggleConnectingRoomsDiscovered(bool toggle)
    {
        lr.enabled = true;
        isRevealed = true;

        for (int i = 0; i < goalRooms.Count; i++)
        {
            //Debug.Log(this.name);
            // Enable each goal room connected to this path
            //goalRooms[i].ToggleHiddenMode(false);
            if (!toggle)
            {
                if (goalRooms[i].GetDiscovered())
                    continue;
                else
                    goalRooms[i].ToggleDiscovered(toggle);
            }
            else
            {
                goalRooms[i].ToggleDiscovered(toggle);
            }
        }
    }
}
