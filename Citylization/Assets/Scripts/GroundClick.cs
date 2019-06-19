using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundClick : MonoBehaviour
{
    public GameObject prefabRoad;
    public GameObject prefabNode;
    GameObject nodeStart;
    Vector3 roadStart;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickLocation(out roadStart);
            nodeStart = Instantiate(prefabNode, roadStart, Quaternion.identity);
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(ClickLocation(out Vector3 roadEnd))
            {
                GameObject nodeEnd = Instantiate(prefabNode, roadEnd, Quaternion.identity);
                CreateRoad(nodeStart.transform.position, nodeEnd.transform.position);
            }
        }
    }

    bool ClickLocation(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitinfo))
        {
            point = hitinfo.point;
            return true;
        }
        point = Vector3.zero;
        return false;
    }


    void CreateRoad(Vector3 roadStart, Vector3 roadEnd)
    {
        float width = 1;
        float length = Vector3.Distance(roadStart, roadEnd);

        if(length<1)
            return;
        

        GameObject road = Instantiate(prefabRoad);
        road.transform.position = roadStart + new Vector3(0, 0.01f, 0);

        
        road.transform.rotation = Quaternion.FromToRotation(Vector3.right, roadEnd - roadStart);


        Vector3[] vertices =
        {
            new Vector3(0,0,-width/2),
            new Vector3(length,0,-width/2),
            new Vector3(length,0,width/2),
            new Vector3(0,0,width/2)
        };

        int[] triangles =
        {
            1,0,2,
            2,0,3
        };

        Vector2[] uv =
        {
            new Vector2(0,0),
            new Vector2(length,0),
            new Vector2(length,1),
            new Vector2(0,1)
        };

        Vector3[] normals =
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        //Create a mesh with the assigned vertices (points) triangles, UVs and normals
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uv,
            normals = normals
        };

        MeshFilter meshFilter = road.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;


        //Mesh m = meshFilter.mesh;
        //m.vertices[0] = new Vector3 
    }
}
