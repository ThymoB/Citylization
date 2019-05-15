using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneratingBuilding : Building
{

    [Header("Yields")]
    [SerializeField]
    public List<ResourceYield> resourceYields = new List<ResourceYield>();
    [Header("Range")]
    public bool globalEffectiveRange;
    public float effectiveRange;
    public bool showRange;
    public float popInRange;



    [Header("Operational Time")]
    //public bool openAllDay = false;
    [Range(0, 24)]
    public int openTime = 8;
    [Range(0, 24)]
    public int closeTime = 18;
    public bool operating = false;

    [Header("Models")]
    public List<GameObject> models;

    private UnityAction timeListener;

    private void Awake()
    {
        timeListener = new UnityAction(EachHour);
    }

    private void OnEnable()
    {
        //PlaceDownBuilding();
    }

    private void OnDisable()
    {
        //PickUpBuilding();
    }

    private void Start()
    {
        PlaceDownBuilding();
        if (showRange)
            ShowRange();
    }

    public void PlaceDownBuilding()
    {
        EventManager.StartListening("time", timeListener);

    }

    public void PickUpBuilding()
    {
        EventManager.StopListening("time", timeListener);
    }

    public void ShowRange()
    {
        GameObject go = Instantiate(ResourceSystem.instance.showRangeObject, transform);
        go.transform.localScale = new Vector3(effectiveRange, ResourceSystem.instance.rangeHeight, effectiveRange);
    }


    public void GatherPop()
    {
        popInRange = 0;
        Collider[] foundHouseholds = Physics.OverlapCapsule(transform.position + new Vector3(0, -ResourceSystem.instance.rangeHeight), transform.position + new Vector3(0, ResourceSystem.instance.rangeHeight), effectiveRange, ResourceSystem.instance.householdMask);
        foreach (Collider collider in foundHouseholds)
        {
            Household household = collider.GetComponentInParent<Household>();
            popInRange += household.residents;
        }

        foreach (ResourceYield resourceYield in resourceYields)
        {
            if (resourceYield.useAmountPerHousehold)
                resourceYield.estimateYield = resourceYield.amount * resourceYield.amountPerHousehold * popInRange;
            else
                resourceYield.estimateYield = resourceYield.amount;
        }
    }



    //Listen to Unity Event from time system
    public void EachHour()
    {
        //Flag if operating
        if (TimeSystem.instance.curHour >= openTime && TimeSystem.instance.curHour < closeTime)
            operating = true;
        else
            operating = false;

        //Check for pop each hour
        if (operating)
        {
            if (!globalEffectiveRange)
                GatherPop();
        }

        foreach (ResourceYield resourceYield in resourceYields)
        {
            //What did I earn last hour?
            resourceYield.AddYield();
            resourceYield.yieldPerHour = 0;

            if (operating && !resourceYield.active)
                StartCoroutine(GiveResources(resourceYield));

            if (!operating && resourceYield.active)
            {
                StopCoroutine(GiveResources(resourceYield));
                resourceYield.active = false;
            }
        }
    }




    IEnumerator GiveResources(ResourceYield resourceYield)
    {
        resourceYield.active = true;
        float delay = CalculateDelay(resourceYield, closeTime - openTime);

        while (operating)
        {
            delay -= (Time.deltaTime * TimeSystem.instance.gameSpeed);
            if (delay <= 0)
            {
                if(!resourceYield.useCapacity || resourceYield.yieldPerDay+resourceYield.yieldPerHour < resourceYield.capacity)
                {
                    resourceYield.yieldPerHour+=resourceYield.amount;
                    ResourceSystem.instance.AddToPlayer(resourceYield.resource, 1f, transform, 1f);
                }
                delay = CalculateDelay(resourceYield, closeTime - openTime);
            }
            yield return null;
        }

        //Closed
       
    }


    public float CalculateDelay(ResourceYield resourceYield, float time)
    {
        float amount = resourceYield.amount;
        if (resourceYield.useAmountPerHousehold)
            amount = resourceYield.estimateYield;
        float delay = (time / amount);
        if(resourceYield.useRandomInterval)
        {
            delay = delay * Random.Range(1 - ResourceSystem.instance.intervalVariance, 1 + ResourceSystem.instance.intervalVariance);
        }
        return delay;
    }



    /*
    public void GiveResource()
    {
        foreach (ResourceYield resourceYield in resourceYields)
        {
            ResourceSystem.instance.AddToPlayer(resourceYield.resource, resourceYield.amount, transform.position, 1f);

        }
    }
    */
}
