using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechStatus { Unavailable, Available, Researching, Completed }

public class TechInfo
{
    public float progress;
    public TechStatus techStatus;

    public TechInfo(float _progress, TechStatus _techStatus)
    {
        progress = _progress;
        techStatus = _techStatus;
        
    }

}
