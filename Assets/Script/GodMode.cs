using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : Singleton<GodMode>
{
    public bool isGod = false;
    public List<CardSO> godPools;
}
