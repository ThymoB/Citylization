using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementUtils : MonoBehaviour
{


    public static Vector3 SnapToGrid(Vector3 beginPoint, Vector3 curPoint, float gridSize)
    {
        return new Vector3(Divide(beginPoint.x, curPoint.x, gridSize), Divide(beginPoint.y, curPoint.y, gridSize), Divide(beginPoint.z, curPoint.z, gridSize));
    }
    static float Divide(float begin, float cur, float size)
    {
        return begin + (Mathf.Round((cur - begin) / size) * size);
    }

}
