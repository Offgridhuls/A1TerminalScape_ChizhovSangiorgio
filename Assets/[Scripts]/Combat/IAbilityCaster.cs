using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityCaster
{
    public int GetCurrentBandwidth();
    public void PayBandwidthCost(int cost);
    public void QueueAbilities();
}
