using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Household : Building
{
    [Header("Pop")]
    public int popAmount;
    public int maxPop;
    public List<Pop> pops = new List<Pop>();
    public List<Building> buildingsInRange = new List<Building>();


}
