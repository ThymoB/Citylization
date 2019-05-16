using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopYieldCounter : YieldCounter
{
    public bool forcePopToBuilding;

    public override float YieldAmount()
    {
        float amount = 0f;
        switch (yieldCountRange)
        {
            case YieldCountRange.Global:
                { 
                    amount = PopManager.instance.totalPop * multiplier;
                    break;
                }
            case YieldCountRange.Self:
                {
                    amount = GetComponent<Household>().residents * multiplier;
                    break;
                }

            case YieldCountRange.UseRange:
                {
                    float popInRange = 0;

                    Collider[] foundHouseholds = Physics.OverlapCapsule(transform.position + new Vector3(0, -ResourceSystem.instance.rangeHeight), transform.position + new Vector3(0, ResourceSystem.instance.rangeHeight), range.effectiveRange, ResourceSystem.instance.householdMask);

                    //Households
                    foreach (Collider collider in foundHouseholds)
                    {
                        Household household = collider.GetComponentInParent<Household>();
                        popInRange += household.residents;
                    }
                    amount = popInRange * multiplier;
                    break;
                }

        }
        return amount;
    }


}
