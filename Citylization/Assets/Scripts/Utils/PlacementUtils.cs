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

    public static Road GetClosestRoad(Vector3 point, float snapRadius) {
        Road road = null;
        Collider[] colliders = Physics.OverlapSphere(point, snapRadius);
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders) {
            if (collider.tag == "Road") {
                Road curRoad = collider.GetComponent<Road>();
                float curDistance = Vector3.Distance(curRoad.transform.position, point);
                if (curDistance < minDistance) road = curRoad;
            }
        }
        return road;

    }
    public static Node GetClosestNode(Vector3 point, float snapRadius) {
        Node node = null;
        Collider[] colliders = Physics.OverlapSphere(point, snapRadius);
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders) {
            if (collider.tag == "Node") {
                Node curNode = collider.GetComponent<Node>();
                float curDistance = Vector3.Distance(curNode.transform.position, point);
                if (curDistance < minDistance) node = curNode;
            }
        }
        return node;
    }
}
