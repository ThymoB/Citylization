using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GeneratingBuilding : Building
{

    [Header("Yields")]
    [SerializeField]
    public List<ResourceYield> resourceYields = new List<ResourceYield>();

    [Header("Operational Time")]
    public bool openAllDay = false;
    [Range(0, 24)]
    public int openTime = 8;
    [Range(0, 24)]
    public int closeTime = 18;

    [HideInInspector]
    public bool operating = false;
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
    }

    //When putting down the building
    public void PlaceDownBuilding()
    {
        EventManager.StartListening("time", timeListener);
        //If it is open all day, start yielding
        if(openAllDay)
        {
            operating = true;
            foreach (ResourceYield resourceYield in resourceYields)
                resourceYield.StartOperating();
        }

    }

    //When picking up the building
    public void PickUpBuilding()
    {
        EventManager.StopListening("time", timeListener);
        //If it is open all day, stop yielding
        if (openAllDay)
        {
            operating = false;
            foreach (ResourceYield resourceYield in resourceYields)
                resourceYield.StopOperating();
        }
    }

    //Listen to Unity Event from time system
    public void EachHour()
    {
        //If it isn't open all day
        if (!openAllDay)
        {
            //Turn buildings On and Off
            if (TimeSystem.instance.curHour >= openTime && TimeSystem.instance.curHour < closeTime)
            {
                if (!operating)
                {
                    operating = true;
                    foreach (ResourceYield resourceYield in resourceYields)
                        resourceYield.StartOperating();
                }
            }
            else
            {
                if (operating)
                {
                    operating = false;
                    foreach (ResourceYield resourceYield in resourceYields)
                        resourceYield.StopOperating();
                }
            }
        }

    }
}
