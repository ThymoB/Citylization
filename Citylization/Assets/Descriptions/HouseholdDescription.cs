using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseholdDescription : Description {

    public string prefix;
    public Household household;

    public override string ObjectDescription(bool AddDetails) {
        return AddDetails? prefix : prefix + "\n\nPops " + household.popAmount + " / " + household.maxPop;
    }
}
