using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YieldCountRange { UseRange, Self, Global }

public abstract class YieldCounter : MonoBehaviour
{
    public YieldCountRange yieldCountRange;
    public Range range;
    public float multiplier = 1f;

    public abstract float YieldAmount();
}
