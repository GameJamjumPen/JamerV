using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileRoom : Room
{
    public string _rewardPoint;
    public RoomType roomType;
}

public enum RoomType{Easy, Medium , Hard}