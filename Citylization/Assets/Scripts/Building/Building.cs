using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public bool canBePickedUp = false;
    public bool needsToBeRoadside = true;
    public Vector2 size = new Vector2(3f,3f);
    [SerializeField]
    public List<ResourceCost> purchaseCosts = new List<ResourceCost>();
        

    [Header("Models")]
    public List<GameObject> models;
    public GameObject selectedModel;

    [Header("Abilities")]
    public List<Ability> abilities;

    [Header("Operational Time")]
    public bool openAllDay = false;
    [Range(0, 24)]
    public int openTime = 8;
    [Range(0, 24)]
    public int closeTime = 18;

    [HideInInspector]
    public bool operating = false;

    [Header("Yields")]
    [SerializeField]
    public List<ResourceYield> resourceYields = new List<ResourceYield>();
    public List<UseResource> resourcesUsed = new List<UseResource>();


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



    //When putting down the building
    public void PlaceDownBuilding()
    {
        EventManager.StartListening("time", timeListener);
        //If it is open all day, start yielding
        if (openAllDay)
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
