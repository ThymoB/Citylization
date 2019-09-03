using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIResources : MonoBehaviour
{
    public PlayerResource playerResource;
    public Image image;
    public TextMeshProUGUI text;
    public TextMeshProUGUI amount;

    public bool usePerDay;
    public bool useCustomDescription;
    public string customDescription;

    private void Start()
    {
        image.sprite = playerResource.resource.popupSprite;
        if (!useCustomDescription)
            text.text = playerResource.resource.name;
        else
            text.text = customDescription;
    }

    private void Update()
    {
        if (usePerDay)
            amount.text = "+"+playerResource.AmountPerDay.ToString(); 
        else
            amount.text = playerResource.Amount.ToString();

    }
}
