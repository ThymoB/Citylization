using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceYield : MonoBehaviour
{
    public Resource resource;
    public float amount;
    public bool useRandomInterval;
    public bool active = false;

    [Header("Household")]
    public bool useAmountPerHousehold;
    public float amountPerHousehold = 1f;
    public float estimateYield;

    [Header("Yield")]
    public float yieldPerHour;
    public float yieldPerDay;
    public float[] totalYieldPerDay = new float[24];
    public float change = 0f;

    [Header("Capacity")]
    public bool useCapacity;
    public int capacity;


    public void AddYield()
    {
        totalYieldPerDay[TimeSystem.instance.curHour] = yieldPerHour;
        YieldPerDay();

    }

    public float YieldPerDay()
    {
        float oldYield = yieldPerDay;
        yieldPerDay = 0;
        foreach (float yield in totalYieldPerDay)
        {
            yieldPerDay += yield;
        }
        Change(yieldPerDay, oldYield);

        return yieldPerDay;
    }

    public void Change(float _new, float _old)
    {
        change = _new - _old;
        ResourceSystem.instance.YieldPerDay(resource, change);
    }



}
