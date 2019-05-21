using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechStatus { Unavailable, Available, Researching, Completed }

public class TechInfo 
{
    public Technology technology;
    public float progress;
    public float scienceNeeded;
    public TechStatus techStatus;
    public TechnologyButton technologyButton; 

    public TechInfo(float _progress, float _scienceNeeded, TechStatus _techStatus)
    {
        progress = _progress;
        scienceNeeded = _scienceNeeded;
        techStatus = _techStatus;
    }

}
