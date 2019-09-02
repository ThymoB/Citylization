using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDescription : Description
{
    public string prefix;
    public Road road;

    public override string ObjectDescription(bool AddDetails) {
        return AddDetails ? prefix : prefix + "\n\nTravel Type: " + road.travelType + "\nSpeed Limit: " + road.speedLimit + "\nLanes: " + road.lanes;  
    }
}
