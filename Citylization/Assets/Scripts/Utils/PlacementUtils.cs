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

    public static List<Vector3> VerticesToSnapTo(MeshFilter meshFilter)
    {
        Matrix4x4 localToWorld = meshFilter.transform.localToWorldMatrix;
        List<Vector3> vertexPoints = new List<Vector3>();

        for (int i = 0; i < meshFilter.mesh.vertices.Length; ++i)
        {
            Vector3 world_v = localToWorld.MultiplyPoint3x4(meshFilter.mesh.vertices[i]);
            vertexPoints.Add(world_v);
        }

        return vertexPoints;
    }

}
