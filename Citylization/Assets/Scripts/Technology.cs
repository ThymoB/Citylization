using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Technology : MonoBehaviour
{
    public Sprite icon;
    public string description;
    public List<Technology> requiredTechs = new List<Technology>();

    public abstract void Unlocks();

}
