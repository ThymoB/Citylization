using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{

    public static UnlockManager instance;

    public List<Unlockable> allUnlockables = new List<Unlockable>();

    public Dictionary<Unlockable, UnlockInfo> unlocksInfo = new Dictionary<Unlockable, UnlockInfo>();
    public List<BuildMenuButton> buildMenuButtons = new List<BuildMenuButton>();

    public Color unlockedColor;
    public Color lockedColor;
    public Color cantAffordColor;
    public Color atMaxColor;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (Unlockable unlockable in allUnlockables)
        {
            unlocksInfo.Add(unlockable, new UnlockInfo(CheckStatus(unlockable), 0, 0));
            AddToMenu(unlockable);
        }

    }

    //Add an unlockable to the bottom menu
    public void AddToMenu(Unlockable unlockable)
    {
        foreach (BuildMenuButton buildMenuButton in buildMenuButtons)
        {
            if(unlockable.menuCategory==buildMenuButton.unlockableMenuCategory)
            {
                buildMenuButton.AddUnlockable(unlockable);
                return;
            }

        }
    }

    //Unlocks all the unlockables from this technology
    public void UnlockFromTech(Technology technology, UnlockStatus status = UnlockStatus.Unlocked)
    {
        foreach (Unlockable unlockable in technology.unlocks)
        {
            unlocksInfo[unlockable].unlockStatus = status;
        }

    }



    //Check if it is unlocked
    public UnlockStatus CheckStatus(Unlockable unlockable)
    {
        UnlockStatus unlockStatus;

        //if it already exists
        if (unlocksInfo.TryGetValue(unlockable, out UnlockInfo existingInfo))
        {
            unlockStatus = unlocksInfo[unlockable].unlockStatus;

            if (existingInfo.currentAmount >= unlockable.maxAmount)
            {
                unlockStatus = UnlockStatus.AtMax;
            }
            else if (!CanAffordUnlockable(unlockable))
            {
                unlockStatus = UnlockStatus.CantAfford;
            }

        }
        //If it is a new status
        else
        {
            //Default to locked
            unlockStatus = UnlockStatus.Locked;

            //Check if it researched it
            foreach (Technology technology in Player.instance.learnedTechnologies)
            {
                if (technology.unlocks.Contains(unlockable))
                {
                    unlockStatus = UnlockStatus.Unlocked;
                    break;
                }
            }
        }
        return unlockStatus;
    }

    public bool CanAffordUnlockable(Unlockable unlockable)
    {
        if (unlockable.type == UnlockableType.Building)
        {
            Building building = unlockable.GetComponent<Building>();
            //Look through the costs
            foreach (ResourceCost resourceCost in building.purchaseCosts)
            {
                //If the player doesn't have one of these costs, return false
                foreach (PlayerResource playerResource in Player.instance.resources)
                {
                    if(resourceCost.resource==playerResource)
                        if (resourceCost.cost > playerResource.Amount)
                            return false;
                }
            }
        }

        //otherwise return true
        return true;

    }
}
