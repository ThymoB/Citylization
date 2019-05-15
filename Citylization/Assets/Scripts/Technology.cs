using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Technology", menuName = "Technology", order =1)]
public class Technology : ScriptableObject
{
    public Era era;
    public Sprite icon;
    public string description;
    public List<Technology> requiredTechs = new List<Technology>();
    public float costToResearch;
    public List<Unlockable> unlocks = new List<Unlockable>();

}
