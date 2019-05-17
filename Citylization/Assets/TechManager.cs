using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechManager : MonoBehaviour
{
    public static TechManager instance;
    public Technology currentlyResearching;
    public Technology lastCompleted;
    public float floatingScience;

    public List<Technology> allTechnologies;
    public List<Technology> availableTechnologies;

    public Dictionary<Technology, TechInfo> technologyProgress = new Dictionary<Technology, TechInfo>();

    [SerializeField]
    private int totalNumberOfTechs;
    public int TotalNumberOfTechs
    {
        get { return totalNumberOfTechs; }
        set
        {
            totalNumberOfTechs = value;
        }
    }

    [SerializeField]
    public int numberOfTechsResearched;
    public int NumberOfTechsResearched
    {
        get { return numberOfTechsResearched; }
        set
        {
            numberOfTechsResearched = value;
        }
    }

    public float PercentageResearched
    {
        get
        {
            float p = NumberOfTechsResearched / (float)TotalNumberOfTechs;
            return p;
        }
    }


    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
        AddAllTechs();
    }

    public void AddAllTechs()
    {
        //Add all techs to the Tech Manager with initial science values
        foreach (Technology tech in allTechnologies)
        {
            if (Player.instance.learnedTechnologies.Contains(tech))
                technologyProgress.Add(tech, new TechInfo(tech.costToResearch, tech.costToResearch, TechStatus.Completed));
            else if (CanLearnTech(tech))
            {
                technologyProgress.Add(tech, new TechInfo(0f, tech.costToResearch, TechStatus.Available));
                availableTechnologies.Add(tech);
            }
            else
                technologyProgress.Add(tech, new TechInfo(0f, tech.costToResearch, TechStatus.Unavailable));
        }
    }

    public bool CanLearnTech(Technology technology)
    {
        foreach (Technology requiredTech in technology.requiredTechs)
        {
            //If the player doesn't have any of the techs, return false
            if (!Player.instance.learnedTechnologies.Contains(requiredTech))
                return false;
        }
        return true;
    }


    public void UpdateCurrentTechByAmount(float amount)
    {
        if (currentlyResearching != null)
        {
            technologyProgress[currentlyResearching].progress += amount;
            if (technologyProgress[currentlyResearching].progress >= currentlyResearching.costToResearch)
                AwardCurrentTechToPlayer();
        }
        else floatingScience += amount;
    }

    public void AwardTechToPlayer(Technology technology)
    {
        Player.instance.learnedTechnologies.Add(technology);
        lastCompleted = technology;
        technologyProgress[lastCompleted].techStatus = TechStatus.Completed;
        UnlockManager.instance.UnlockFromTech(technology);
        if (technology == currentlyResearching)
            currentlyResearching = null;
    }


    public void AwardCurrentTechToPlayer()
    {
        AwardTechToPlayer(currentlyResearching);
    }

    //Add all floating science to the technology
    public void AddFloatingToTech()
    {
        technologyProgress[currentlyResearching].progress += floatingScience;
        floatingScience = 0;
    }

    //Switch from one tech to another when clicking on the tech tree for example
    public void SwitchTechs(Technology technology)
    {
        if (technologyProgress[technology].techStatus == TechStatus.Available)
        {


            currentlyResearching = technology;
            technologyProgress[currentlyResearching].techStatus = TechStatus.Researching;
            AddFloatingToTech();
        }
        /*
        if (technologyProgress[technology].techStatus == TechStatus.Available)
        {
            //Switch current researching
            if (technologyProgress[currentlyResearching].techStatus == TechStatus.Researching)
                technologyProgress[currentlyResearching].techStatus = TechStatus.Available;
            

            currentlyResearching = technology;
            technologyProgress[technology].techStatus = TechStatus.Researching;
            AddFloatingToTech();
        }
        */
    }
}
