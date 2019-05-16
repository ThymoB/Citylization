using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool canBePickedUp = false;

    [Header("Models")]
    public List<GameObject> models;

    [Header("Abilities")]
    public List<Ability> abilities;
}
