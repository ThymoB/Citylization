using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Building building;
    [SerializeField]
    public List<ResourceCost> resourcesCost;
    public float cooldownInHours;
    public float curCooldown = 0;

    public abstract void OnActivate();

    public bool CanActivate()
    {
        //If it is on cooldown
        if (curCooldown > 0) return false;

        //If the player doesn't have enough money
        foreach (ResourceCost resourceCost in resourcesCost)
        {
            PlayerResource playerResource = ResourceSystem.instance.FindPlayerResource(resourceCost.resource);
            if (playerResource.Amount < resourceCost.cost) return false;
        }

        //Return true if passed all the flags
        return true;
    }
}
