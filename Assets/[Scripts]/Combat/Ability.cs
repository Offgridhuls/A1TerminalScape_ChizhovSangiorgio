using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public int abilityType, statToModify, modValue;
    public int bandwidthCost;
    public IAbilityCaster caster;
    public IAbilityTarget target;
    public CombatModule combatModule;
    public bool addsModifier;
    public bool CheckBandwidthAvailable()
    {
        return caster.GetCurrentBandwidth() >= bandwidthCost;
    }
    public void Resolve()
    {
        caster.PayBandwidthCost(bandwidthCost);
        switch (abilityType)
        {
            case (int)PlayerSkills.Exfiltrate:
                combatModule.BattleNotify("Attempting Exfiltration");
                target.AttemptExfil();
                break;
            case (int)PlayerSkills.ZeroDay:
                combatModule.BattleNotify("Attempting Zero Day");
                target.AttemptZeroDay();
                break;
            default:
                target.ApplyStatEffect(addsModifier, statToModify, modValue);
                break;
        }
    }
}
