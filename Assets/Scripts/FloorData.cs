using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Floor", menuName = "Floor")]
public class FloorData : ScriptableObject
{
    public string floorName;
    public Color floorColour;
    public int floorLevel;
    public int minRoomCount = 6;
    public int maxRoomCount = 8;
    public int sideRoomAmount = 5;
    public int shopRoomCount = 1;
    public int heroRoomCount = 1;
    public int minEnemyRoomCount = 1;
    public int maxEnemyRoomCount = 3;

    public List<UnitData> enemyUnits = new List<UnitData>();

}
