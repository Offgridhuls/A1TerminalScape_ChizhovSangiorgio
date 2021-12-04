using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemystats
{
    DataIntegrity = 0,
    BandwidthCap = 1,
    ConnectionSpeed = 2,
    Backups = 3,
    SystemKnowledge = 4,
    ExfilChance = 5,
    STATCOUNT = 6
}

public static class Enemy
{
    public static int[] stats = new int[(int)Enemystats.STATCOUNT];
}
