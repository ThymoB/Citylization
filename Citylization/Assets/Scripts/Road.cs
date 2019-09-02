using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TravelType { Foot, Bike, Car };

public class Road : MonoBehaviour
{
    public GameObject modelShell;
    public int lanes = 1;
    public float speedLimit = 5f;
    public TravelType travelType;
    //public GameObject roadSegment;


}
