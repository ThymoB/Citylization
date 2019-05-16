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
    [Header("Button")]
    public Image button;
    public Color buttonAvailableColor;
    public Color buttonCompletedColor;
    public Color buttonResearchingColor;
    public Color buttonUnavailableColor;

    [Header("Outline")]
    public Outline outline;
    public Color availableColor;
    public Color completedColor;
    public Color researchingColor;
    public Color unavailableColor;

    private void Start()
    {
        for (int i = 0; i < unlockablePreview.Length; i++)
        {
            if (i >= technology.unlocks.Count)
                unlockablePreview[i].gameObject.SetActive(false);
            else
                unlockablePreview[i].icon.sprite = technology.unlocks[i].iconOnTechTree;
        }
        nameText.text = technology.name;
        icon.sprite = technology.icon;
        UpdateColors();
    }

    private void OnEnable()
    {
        


    }

    private void OnDisable()
    {
        
    }

    //When clicking on a button to research
    public void SelectTech()
    {
        TechManager.instance.SwitchTechs(technology);
        progress.UpdateProgress();
        UpdateColors();
        
    }

    public void UpdateColors()
    {
        switch (TechManager.instance.technologyProgress[technology].techStatus)
        {
            case TechStatus.Unavailable:
                outline.effectColor = unavailableColor;
                button.color = buttonUnavailableColor;
                break;

            case TechStatus.Available:
                outline.effectColor = availableColor;
                button.color = buttonAvailableColor;
                break;

            case TechStatus.Researching:
                outline.effectColor = researchingColor;
                button.color = buttonResearchingColor;
                break;

            case TechStatus.Completed:
                outline.effectColor = completedColor;
                button.color = buttonCompletedColor;
                break;
        }

    }


}
