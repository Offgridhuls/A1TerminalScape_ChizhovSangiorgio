using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatType
{
    DataIntegrity = 0,
    Bandwidth = 1,
    ConnectionSpeed = 2,
    Backups = 3,
    SystemKnowledge = 4,
    ExfilChance = 5,
    STATCOUNT = 6
}

public enum EnemyStatType
{
    DataIntegrity = 0,
    Bandwidth = 1,
    ConnectionSpeed = 2,
    STATCOUNT = 3
}

public enum CombatModifiers
{
    OpenPorts = 0,
    Garbage = 1,
    COMBATMODCOUNT = 2
}

public enum PlayerSkills
{
    Compromise = 0,
    Backdoor = 1,
    DoS = 2,
    Decrypt = 3,
    InitGateway = 4,
    Exfiltrate = 5,
    ZeroDay = 6,
    SKILLCOUNT = 7
}

public enum EnemySkills
{
    Compromise = 0,
    Backdoor = 1,
    DoS = 2,
    Ping = 3,
    SKILLCOUNT = 4
}
public static class GameStatics
{
    //base stats to build player and enemy from
    public static int[] _statMinValues = new int[] { 100, 100, 3, 20, 2, 10 };
    public static int[] _statIncrementValues = new int[] { 10, 10, 1, 5, 1, 2 };
}

public static class PlayerStats
{
    public static string _currentSaveFileName = "";
    public static bool _fileLoaded = false;
    public static int[] _currentPlayerStats = new int[(int)PlayerStatType.STATCOUNT];
    //public static List<int>[] _modifiers = new List<int>[(int)PlayerStatType.STATCOUNT];
    public static int _currentPlayerIntegrity;
    public static int _playerUpgradePoints = 0;
    public static int _enemyDifficulty = 0;
}

public static class EnemyStats
{
    public static int[] _stats = new int[(int)EnemyStatType.STATCOUNT];
    public static List<int>[] _modifiers = new List<int>[(int)EnemyStatType.STATCOUNT];
}
