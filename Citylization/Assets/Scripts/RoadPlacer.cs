using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class RoadPlacer : MonoBehaviour
{
    public Vector3 beginPoint;
    public Vector3 endPoint;
    public LineRenderer lineRenderer;
    public MouseBehaviour mouseBehaviour;
    public GameObject preview;
    public Color validColor;
    public Color invalidColor;
    public bool creatingLine;
    public Road road;
    public Transform roadsParent;

    public bool IsValid(Vector3 place)
    {
        return true;

    }

    public void SetBeginPoint(Vector3 _beginPoint, Road _road)
    {
        beginPoint = _beginPoint;
        road = _road;
        lineRenderer.SetPosition(0, beginPoint);
        lineRenderer.SetPosition(1, beginPoint);
        creatingLine = true;
        StartCoroutine(ShowRoad());
    }

    public void SetEndPoint(Vector3 _endPoint)
    {
        endPoint = _endPoint;
        StopCoroutine(ShowRoad());
        creatingLine = false;
        lineRenderer.SetPosition(1, endPoint);
        CreateRoad();
    }

    public void CreateRoad()
    {
        Road newRoad = Instantiate(road, beginPoint, Quaternion.FromToRotation(beginPoint,endPoint), roadsParent);
        newRoad.transform.localScale = new Vector3(1, 1, Vector3.Distance(beginPoint, endPoint));

    }


    IEnumerator ShowRoad()
    {
        while(creatingLine)
        {
            lineRenderer.SetPosition(1, mouseBehaviour.mousePosition);
            yield return null;
        }
        
    }


}
