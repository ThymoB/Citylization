using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnlockableType { Building, Zone, Road, Ability, Bonus }

[RequireComponent(typeof(Description))]
public class Unlockable : MonoBehaviour
{
    public Sprite iconOnTechTree;
    public bool isUnlocked = false;
    public UnlockableType unlockableType;
    public Technology obsolete;
}
