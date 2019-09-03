using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnlockableType { Building, Zone, Road, Ability, Bonus }
public enum UnlockableMenuCategory { Roads, Residential, Commercial, Industrial, Scientific, Cultural, Economic, Governmental, Health, Service, Protection }

public class Unlockable : MonoBehaviour
{
    public Sprite iconOnTechTree;
    public Sprite iconOnMenu;
    public Description description;
    public UnlockableType type;
    public UnlockableMenuCategory menuCategory;
    public Technology obsolete;
    public bool useMaxAmount;
    public int maxAmount;
}
