using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatInterface
{
    CombatModule combatModule { get; set; }
    void DisplayStats();
    public int RollInitiative();
    public void OnBeginTurn();
    public void OnDeath();
}
