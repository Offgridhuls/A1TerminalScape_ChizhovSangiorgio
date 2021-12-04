using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ICombatInterface, IAbilityTarget, IAbilityCaster
{
    [SerializeField]
    public Text statsDisplay;

    private int[] baseStats = new int[(int)EnemyStatType.STATCOUNT];
    private int[] modifiers = new int[(int)CombatModifiers.COMBATMODCOUNT];
    private int level;

    private int[] activeStatValues = new int[2];
    public CombatModule combatModule { get; set; }

    private Ability[] abilities = new Ability[(int)EnemySkills.SKILLCOUNT];


    public void GenerateNew(int difficulty)
    {
        for (int i = 0; i < (int)EnemyStatType.STATCOUNT; i++)
        {
            baseStats[i] = GameStatics._statMinValues[i];
            if (i < 2) activeStatValues[i] = baseStats[i];
        }
        for (int i = 0; i < (int)CombatModifiers.COMBATMODCOUNT; i++)
        {
            modifiers[i] = 0;
        }
        for (int i = 0; i <= difficulty; i++)
        {
            int randomStatIndex = Random.Range(0, (int)EnemyStatType.STATCOUNT);
            baseStats[randomStatIndex] += GameStatics._statIncrementValues[randomStatIndex];
            if (randomStatIndex < 2) activeStatValues[randomStatIndex] += GameStatics._statIncrementValues[randomStatIndex];
        }
        level = difficulty;
        DisplayStats();
    }

    // combat interface

    public void OnBeginTurn()
    {
        activeStatValues[(int)EnemyStatType.Bandwidth] = 
            Mathf.Max(baseStats[(int)EnemyStatType.Bandwidth] - (modifiers[(int)CombatModifiers.Garbage] * 10), 0);
        DisplayStats();
    }
    public void DisplayStats()
    {
        string display = $"{level}\n\n";
        for (int i = 0; i < 2; i++)
        {
            display += $"{activeStatValues[i]}/{baseStats[i]}\n\n";
        }
        for (int i = 0; i < (int)CombatModifiers.COMBATMODCOUNT; i++)
        {
            display += $"{modifiers[i]}\n\n";
        }
        statsDisplay.text = display;
    }
    public int RollInitiative()
    {
        return baseStats[(int)EnemyStatType.ConnectionSpeed];
    }
    public void OnDeath()
    {
        Debug.Log("Enemy killed");
        combatModule.OnEnemyDeath();
    }

    //ability caster
    public int GetCurrentBandwidth()
    {
        return activeStatValues[(int)EnemyStatType.Bandwidth];
    }
    public void PayBandwidthCost(int cost)
    {
        activeStatValues[(int)EnemyStatType.Bandwidth] -= cost;
    }
    public void QueueAbilities()
    {
        //TODO
    }

    //ability target
    public void ApplyStatEffect(bool addsModifier, int statToModify, int modValue)
    {
        if (addsModifier)
        {
            switch ((CombatModifiers)statToModify)
            {
                case CombatModifiers.Garbage:
                    modifiers[statToModify] += modValue;
                    break;
                case CombatModifiers.OpenPorts:
                    modifiers[statToModify] += modValue;
                    break;
                default:
                    Debug.LogError($"No combat modifier with index {statToModify} exists");
                    break;
            }
        }
        else
        {
            switch ((EnemyStatType)statToModify)
            {
                case EnemyStatType.DataIntegrity:
                    activeStatValues[statToModify] += (modValue - modifiers[(int)CombatModifiers.OpenPorts] * 10);
                    if (activeStatValues[statToModify] <= 0)
                        OnDeath();
                    break;
                default:
                    Debug.LogError($"No stat with index {statToModify} exists");
                    break;
            }
        }
        DisplayStats();
    }

    public void AttemptExfil()
    {
        Debug.LogError("Enemy should never attempt exfil");
    }

    public void AttemptZeroDay()
    {
        Debug.LogError("Enemy should never attempt zero day");
    }
}
