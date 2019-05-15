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
    }

    private void Start()
    {
        AddAllTechs();
    }

    public void AddAllTechs()
    {
        foreach (Technology tech in allTechnologies)
        {
            if (Player.instance.learnedTechnologies.Contains(tech))
                technologyProgress.Add(tech, new TechInfo(0f, TechStatus.Completed));
            else if (CanLearnTech(tech))
                technologyProgress.Add(tech, new TechInfo(0f, TechStatus.Available));
            else
                technologyProgress.Add(tech, new TechInfo(0f, TechStatus.Unavailable));
        }
    }

    public bool CanLearnTech(Technology technology)
    {

        if (!technology.requiredTechs.Contains(technology))
            return false;
        

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

    public void AwardCurrentTechToPlayer()
    {
        Player.instance.learnedTechnologies.Add(currentlyResearching);
        lastCompleted = currentlyResearching;
        technologyProgress[lastCompleted].techStatus = TechStatus.Completed;
        currentlyResearching = null;
    }

    public void AddFloatingToTech()
    {
        technologyProgress[currentlyResearching].progress += floatingScience;
        floatingScience = 0;
    }


}
