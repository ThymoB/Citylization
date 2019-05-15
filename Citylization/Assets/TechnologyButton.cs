using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TechnologyButton : MonoBehaviour
{
    public Technology technology;
    public UIUnlockable[] unlockablePreview;
    public TextMeshProUGUI nameText;
    public Image icon;
    public ResearchProgress progress;

    [Header("Outline")]
    public Outline outline;
    public Color completedColor;
    public Color researchingColor;
    public Color unavailableColor;

    private void Awake()
    {
        for (int i = 0; i < unlockablePreview.Length; i++)
        {
            if (i >= technology.unlocks.Count)
                unlockablePreview[i].gameObject.SetActive(false);
            else
                unlockablePreview[i].icon.sprite = technology.unlocks[i].iconOnTechTree;
        }
        progress.technology = technology;
        nameText.text = technology.name;
        icon.sprite = technology.icon;
    }

    private void OnEnable()
    {
        


    }

    private void OnDisable()
    {
        
    }

    public void Researching()
    {

    }

    public void Completed()
    {

    }

    public void Unavailable()
    {

    }

    public void SelectTech()
    {

    }
}
