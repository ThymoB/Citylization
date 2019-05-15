using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public Resource resource;
    [SerializeField]
    private float amount;
    public float Amount
    {
        get { return amount; }
        set
        {
            amount = value;
        }
    }

    [SerializeField]
    private float amountPerDay;
    public float AmountPerDay
    {
        get { return amountPerDay; }
        set
        {
            amountPerDay = value;
        }
    }

}
