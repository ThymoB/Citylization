using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopManager : MonoBehaviour
{
    public static PopManager instance;
    public int totalPop;
    public GameObject householdsParent;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
    }

    //For testing, now count pop at start
    private void Start()
    {
        RecountAllPop();
    }

    public void RecountAllPop()
    {
        totalPop = 0;
        Household[] households = householdsParent.GetComponentsInChildren<Household>();

        foreach (Household household in households)
        {
            totalPop += household.popAmount;
        }

    }
}
