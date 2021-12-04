using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatType
{
    DataIntegrity = 0,
    BandwidthCap = 1,
    ConnectionSpeed = 2,
    Backups = 3,
    SystemKnowledge = 4,
    ExfilChance = 5,
    STATCOUNT = 6
}

public static class PlayerStats
{
    public static string _fileName = "";
    public static bool _loaded = false;
    public static int[] _stats = new int[(int)PlayerStatType.STATCOUNT];
    public static List<int>[] _modifiers = new List<int>[(int)PlayerStatType.STATCOUNT];
}
