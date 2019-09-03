using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ResearchProgress : MonoBehaviour
{
    private UnityAction scienceListener;

    public TechnologyButton technologyButton;
    public RectTransform blueBar;
    public TextMeshProUGUI numbers;

    private void OnEnable()
    {
        EventManager.StartListening("Science", scienceListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Science", scienceListener);
    }

    private void Awake()
    {
        scienceListener = new UnityAction(UpdateProgress);
    }


    private void Start()
    {
        OnOpenPanel();
    }


    //When opening the tech tree
    public void OnOpenPanel()
    {
        UpdateProgress();
    }

    //Every time science comes in or a tech is completed, update the progress
    public void UpdateProgress()
    {
        if (TechManager.instance.techDictionary.TryGetValue(technologyButton.technology, out TechInfo techInfo))
        {
            blueBar.localScale = new Vector3(techInfo.progress / techInfo.scienceNeeded, 1f, 1f);
            numbers.text = techInfo.progress.ToString() + "/" + techInfo.scienceNeeded.ToString();
            
        }
        technologyButton.UpdateColors();
    }

}
