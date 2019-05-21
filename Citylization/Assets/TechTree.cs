using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public TechnologyButton[] technologyButtons;

    private void Awake()
    {
        technologyButtons = GetComponentsInChildren<TechnologyButton>(true);
    }

    public void FindTechButton(Technology technology)
    {
        foreach (TechnologyButton technologyButton in technologyButtons)
        {
            if(technology==technologyButton.technology)
            {
                TechManager.instance.technologyProgress[technology].technologyButton = technologyButton;
            }

        }

    }
}
