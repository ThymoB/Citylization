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

    public Dictionary<Technology, float> technologyProgress = new Dictionary<Technology, float>();

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
            technologyProgress.Add(tech, 0f);
        }
    }


    public void UpdateCurrentTechByAmount(float amount)
    {
        if (currentlyResearching != null)
        {
            technologyProgress[currentlyResearching] += amount;
            if (technologyProgress[currentlyResearching] >= currentlyResearching.costToResearch)
                AwardCurrentTechToPlayer();
        }
        else floatingScience += amount;
    }

    public void AwardCurrentTechToPlayer()
    {
        Player.instance.learnedTechnologies.Add(currentlyResearching);
        lastCompleted = currentlyResearching;
        currentlyResearching = null;
    }

}
