using System.Collections.Generic;
using UnityEngine;

public enum PopAge { Junior, Adult, Senior}
public enum PopEducation { None, Primary, Secondary, High }


public class Pop : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float lifeSpan;
    public Household home;
    public int age;
    public PopAge ageGroup;
    public PopEducation education;

    [Header("Actions")]
    //Actions to do in a day
    public List<Building> actions = new List<Building>();

    [Header("Misc")]
    public bool isTourst;

}
