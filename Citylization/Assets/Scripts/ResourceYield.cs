using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//summary
//If

public class ResourceYield : MonoBehaviour
{
    [Header("General")]
    public Resource resource;
    public float dailyAmount;
    public bool useRandomInterval;

    [Header("Daily Yield")]
    public bool useFixedDailyYield;
    public int fixedDailyYieldTime;

    [Header("Building")]
    public GeneratingBuilding building;

    [Header("Household")]
    public bool useAmountPerHousehold;
    public LayerMask popType;
    public float amountPerHousehold = 1f;
    public float estimateYield;

    [Header("Range")]
    public Range range;

    [Header("Capacity")]
    public bool useDailyCapacity;
    public float dailyCapacity;

    private bool active = false;
    private UnityAction timeListener;
    private float yieldPerHour;
    private float yieldPerDay;
    private float[] totalYieldPerDay = new float[24];
    private float change = 0f;
    private GameObject rangeObject;

    private void Awake()
    {
        timeListener = new UnityAction(EachHour);
    }

    private void OnEnable()
    {
        EventManager.StartListening("time", timeListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("time", timeListener);
    }


    //OPERATING
    public void StartOperating()
    {
        active = true;
        //Start generating resources with an interval if there is no fixed yield time
        if (!useFixedDailyYield)
            StartCoroutine(GiveResources());
        //Show the range of the building if it has one
        if (range != null)
            if (range.showRange)
                ShowRange(true);
    }

    public void StopOperating()
    {
        active = false;
        //Stop generating resources with an interval if there is no fixed yield time
        if (!useFixedDailyYield)
            StopCoroutine(GiveResources());
        //Delete the range object of the building if it has one
        if (range != null)
            if (range.showRange)
                ShowRange(false);
    }

    public void ShowRange(bool show)
    {
        if (show)
        {
            rangeObject = Instantiate(ResourceSystem.instance.showRangeObject, transform);
            rangeObject.transform.localScale = new Vector3(range.effectiveRange, ResourceSystem.instance.rangeHeight, range.effectiveRange);
        }
        else
        {
            Destroy(rangeObject);
        }
    }


    //Gather all objects with the layermask, like households
    public void GatherPopInRange()
    {
        range.popInRange = 0;
        Collider[] foundHouseholds = Physics.OverlapCapsule(transform.position + new Vector3(0, -ResourceSystem.instance.rangeHeight), transform.position + new Vector3(0, ResourceSystem.instance.rangeHeight), range.effectiveRange, popType);
        //Households
        if (popType == ResourceSystem.instance.householdMask)
        {
            foreach (Collider collider in foundHouseholds)
            {
                Household household = collider.GetComponentInParent<Household>();
                range.popInRange += household.residents;
            }

            if (useAmountPerHousehold)
                estimateYield = dailyAmount * amountPerHousehold * range.popInRange;
            else
                estimateYield = dailyAmount;
        }
    }

    public void GiveResource()
    {
        if (!useDailyCapacity || yieldPerDay + yieldPerHour < dailyCapacity)
        {
            yieldPerHour+=dailyAmount;
            ResourceSystem.instance.AddToPlayer(resource, dailyAmount, transform, 1/dailyAmount);
        }
    }

    //Give resources with intervals
    IEnumerator GiveResources()
    {
        float delay = CalculateDelay();
        while (active)
        {
            delay -= (Time.deltaTime * TimeSystem.instance.gameSpeed);
            if (delay <= 0)
            {
                if (!useDailyCapacity || yieldPerDay + yieldPerHour < dailyCapacity)
                {
                    yieldPerHour++;
                    ResourceSystem.instance.AddToPlayer(resource, 1f, transform, dailyAmount);
                }
                delay = CalculateDelay();
            }
            yield return null;
        }
    }


    //Calculate the delay based on the daily amount
    public float CalculateDelay()
    {
        float time;
        //Use building opening times if there is a building attached
        if (building != null) time = building.closeTime - building.openTime;
        //Otherwise calculate for a day
        else time = 24;
        float amount = dailyAmount;
        if (useAmountPerHousehold)
            amount = estimateYield;
        //The delay is the time divided by the (estimated) daily yield 
        float delay = (time / amount);
        //If it used variance, apply it here
        if (useRandomInterval)
            delay = delay * Random.Range(1 - ResourceSystem.instance.intervalVariance, 1 + ResourceSystem.instance.intervalVariance);
        return delay;
    }


    public void EachHour()
    {
        //Gather range if it has one
        if (range != null)
            GatherPopInRange();
        //Give fixed time yield
        if (useFixedDailyYield)
            if (fixedDailyYieldTime == TimeSystem.instance.curHour)
                GiveResource();
        CalculateHourlyYield();
    }


    //Calculate Yield per hour and day
    void CalculateHourlyYield()
    {
        //What did I earn last hour?
        AddYield();
        yieldPerHour = 0;
    }

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
            yieldPerDay += yield;

        Change(yieldPerDay, oldYield);

        return yieldPerDay;
    }

    public void Change(float _new, float _old)
    {
        change = _new - _old;
        ResourceSystem.instance.YieldPerDay(resource, change);
    }



}
