using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour, ICombatInterface, IAbilityCaster, IAbilityTarget
{
    [SerializeField]
    public Text statsDisplay;

    private int[] baseStats = new int[(int)PlayerStatType.STATCOUNT];
    private int[] activeStatValues = new int[(int)PlayerStatType.STATCOUNT];
    private int[] modifiers = new int[(int)CombatModifiers.COMBATMODCOUNT];
    public CombatModule combatModule { get; set; }
    private Ability[] abilities = new Ability[(int)PlayerSkills.SKILLCOUNT];

    void OnAwake()
    {
        QueueAbilities();
    }

    public void LoadStats()
    {
        baseStats[0] = GameStatics._currentPlayerStats[0];
        activeStatValues[0] = GameStatics._currentPlayerIntegrity;
        for (int i = 1; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            baseStats[i] = activeStatValues[i] = GameStatics._currentPlayerStats[i];
        }
        for (int i = 0; i < (int)CombatModifiers.COMBATMODCOUNT; i++)
        {
            modifiers[i] = 0;
        }
        DisplayStats();
    }
    public void AttemptPlayAbility(PlayerSkills ability)
    {
        int abilityIndex = (int)ability;
        if (abilities[abilityIndex].CheckBandwidthAvailable())
        {
            abilities[abilityIndex].Resolve();
            DisplayStats();
        }
        else combatModule.BattleNotify("Insufficient Bandwidth");
    }

    /// <summary>
    /// OnPlayerWin called after battle is over to update statics with current active values
    /// </summary>
    public void OnPlayerWin()
    {
        activeStatValues[(int)PlayerStatType.DataIntegrity] =
            Mathf.Min(activeStatValues[(int)PlayerStatType.DataIntegrity] + activeStatValues[(int)PlayerStatType.Backups], baseStats[(int)PlayerStatType.DataIntegrity]);
        GameStatics._currentPlayerIntegrity = activeStatValues[(int)PlayerStatType.DataIntegrity];
    }

    //combat
    public void DisplayStats()
    {
        statsDisplay.text = $"{activeStatValues[0]}/{baseStats[0]}\n\n" +
            $"{activeStatValues[1]}/{baseStats[1]}\n\n" +
            $"{activeStatValues[4]}\n\n" +
            $"{activeStatValues[5]}\n\n" +
            $"{modifiers[0]}\n\n" +
            $"{modifiers[1]}";
    }

    public int RollInitiative()
    {
        return activeStatValues[(int)PlayerStatType.ConnectionSpeed];
    }

    public void OnBeginTurn()
    {
        //regenerate bandwidth
        activeStatValues[(int)PlayerStatType.Bandwidth] = 
            Mathf.Max(baseStats[(int)PlayerStatType.Bandwidth] - (modifiers[(int)CombatModifiers.Garbage] * 10), 0);
        DisplayStats();
    }

    public void OnDeath()
    {
        combatModule.OnPlayerDeath();
    }

    //ability caster
    public int GetCurrentBandwidth()
    {
        return activeStatValues[(int)PlayerStatType.Bandwidth];
    }

    public void QueueAbilities()
    {
        for (int i = 0; i < (int)PlayerSkills.SKILLCOUNT; i++)
        {
            abilities[i] = new Ability();
            abilities[i].caster = this;
            abilities[i].combatModule = combatModule;
        }

        int abilityIndex = (int)PlayerSkills.Compromise;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = false;
        abilities[abilityIndex].statToModify = (int)EnemyStatType.DataIntegrity;
        abilities[abilityIndex].modValue = -25;
        abilities[abilityIndex].bandwidthCost = 35;
        abilities[abilityIndex].target = combatModule.enemy;

        abilityIndex = (int)PlayerSkills.Backdoor;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = true;
        abilities[abilityIndex].statToModify = (int)CombatModifiers.OpenPorts;
        abilities[abilityIndex].modValue = 1;
        abilities[abilityIndex].bandwidthCost = 25;
        abilities[abilityIndex].target = combatModule.enemy;

        abilityIndex = (int)PlayerSkills.DoS;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = true;
        abilities[abilityIndex].statToModify = (int)CombatModifiers.Garbage;
        abilities[abilityIndex].modValue = 1;
        abilities[abilityIndex].bandwidthCost = 25;
        abilities[abilityIndex].target = combatModule.enemy;

        abilityIndex = (int)PlayerSkills.Decrypt;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = false;
        abilities[abilityIndex].statToModify = (int)PlayerStatType.SystemKnowledge;
        abilities[abilityIndex].modValue = GameStatics._statIncrementValues[(int)PlayerStatType.SystemKnowledge];
        abilities[abilityIndex].bandwidthCost = 20;
        abilities[abilityIndex].target = this;

        abilityIndex = (int)PlayerSkills.InitGateway;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = false;
        abilities[abilityIndex].statToModify = (int)PlayerStatType.ExfilChance;
        abilities[abilityIndex].modValue = GameStatics._statIncrementValues[(int)PlayerStatType.ExfilChance];
        abilities[abilityIndex].bandwidthCost = 20;
        abilities[abilityIndex].target = this;

        abilityIndex = (int)PlayerSkills.Exfiltrate;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = false;
        abilities[abilityIndex].statToModify = -1;
        abilities[abilityIndex].modValue = 0;
        abilities[abilityIndex].bandwidthCost = 35;
        abilities[abilityIndex].target = this;

        abilityIndex = (int)PlayerSkills.ZeroDay;
        abilities[abilityIndex].abilityType = abilityIndex;
        abilities[abilityIndex].addsModifier = false;
        abilities[abilityIndex].statToModify = -1;
        abilities[abilityIndex].modValue = 0;
        abilities[abilityIndex].bandwidthCost = 40;
        abilities[abilityIndex].target = this;
    }

    public void PayBandwidthCost(int cost)
    {
        activeStatValues[(int)PlayerStatType.Bandwidth] -= cost;
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
            switch ((PlayerStatType)statToModify)
            {
                case PlayerStatType.DataIntegrity:
                    activeStatValues[statToModify] += (modValue + modifiers[(int)CombatModifiers.OpenPorts] * 10);
                    if (activeStatValues[statToModify] <= 0)
                        OnDeath();
                    break;
                case PlayerStatType.SystemKnowledge:
                    activeStatValues[statToModify] = Mathf.Max(activeStatValues[statToModify] + modValue, 0);
                    break;
                case PlayerStatType.ExfilChance:
                    activeStatValues[statToModify] = Mathf.Max(activeStatValues[statToModify] + modValue, 0);
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
        int rollValue = Random.Range(0, 100);
        if (rollValue <= activeStatValues[(int)PlayerStatType.ExfilChance])
        {
            combatModule.OnPlayerExfilSuccess();
        }
        else combatModule.BattleNotify("Exfil Failed");
    }

    public void AttemptZeroDay()
    {
        int rollValue = Random.Range(0, 101);
        if (rollValue <= activeStatValues[(int)PlayerStatType.SystemKnowledge])
        {
            combatModule.OnZeroDaySuccess();
        }
        else combatModule.BattleNotify("Zero Day failed");
    }
}
