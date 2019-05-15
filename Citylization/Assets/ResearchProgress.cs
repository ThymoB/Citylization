using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ResearchProgress : MonoBehaviour
{
    private UnityAction scienceListener;

    public Technology technology;
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

    }

    public void OnOpenPanel()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        TechManager.instance.technologyProgress.TryGetValue(technology, out TechInfo techInfo);
        blueBar.localScale = new Vector3(techInfo.progress / technology.costToResearch, 1f, 1f);
        numbers.text = techInfo.progress.ToString() + "/" + technology.costToResearch.ToString();
    }

}
