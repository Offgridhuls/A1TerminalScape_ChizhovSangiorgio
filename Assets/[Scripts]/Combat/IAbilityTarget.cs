using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTarget
{
    public void ApplyStatEffect(bool addsModifier, int statToModify, int modValue);
    public void AttemptExfil();
    public void AttemptZeroDay();

}
