using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YieldType { Science, Food, Culture, Money, Health, Happiness, GovernmentPoints, Produce, Faith, Electricity, Water }

[CreateAssetMenu(fileName = "Resource Yield", menuName = "Yield", order = 1)]
public class Resource : ScriptableObject
{
    public YieldType yieldType;
    public Sprite popupSprite;
    public Color color;
}
