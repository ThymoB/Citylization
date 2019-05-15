using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ResearchProgress : MonoBehaviour
{
    private UnityAction scienceListener;
    public float needed;
    public float current;

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

    public void UpdateProgress()
    {
        needed = technology.costToResearch;
        TechManager.instance.technologyProgress.TryGetValue(technology, out current);
        blueBar.localScale = new Vector3(current / needed, 1f, 1f);
        numbers.text = current.ToString() + "/" + needed.ToString();
    }

}
