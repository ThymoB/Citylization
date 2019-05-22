using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class RoadPlacer : MonoBehaviour
{
    public Vector3 beginPoint;
    public Vector3 endPoint;
    public MouseBehaviour mouseBehaviour;
    public MeshRenderer previewPrefab;
    public Material validMat;
    public Material invalidMat;
    public Transform roadsParent;
    [Header("Grid")]
    public bool useGrid;
    public float gridSize = 1f;

    private bool creatingLine;
    private MeshRenderer preview;
    private Road road;

    public bool IsValid(Vector3 place)
    {
        return true;

    }

    public void SetBeginPoint(Vector3 _beginPoint, Road _road)
    {
        beginPoint = _beginPoint;
        road = _road;
        preview = Instantiate(previewPrefab, beginPoint, Quaternion.identity, transform);
        creatingLine = true;
        StartCoroutine(ShowRoad());
    }


    public void CreateRoad()
    {
        StopCoroutine(ShowRoad());
        creatingLine = false;

        //Straight road
        Road newRoad = Instantiate(road, beginPoint, Quaternion.identity, roadsParent);
        newRoad.transform.localScale = preview.transform.localScale;
        newRoad.transform.LookAt(endPoint);

        //Delete preview
        Destroy(preview.gameObject);
    }


    IEnumerator ShowRoad()
    {
        while(creatingLine)
        {
            endPoint = mouseBehaviour.mousePosition;

            if (useGrid)
                endPoint = PlacementUtils.SnapToGrid(beginPoint, endPoint, gridSize);

            preview.transform.localScale= new Vector3(1, 1, Vector3.Distance(beginPoint, endPoint));
            preview.transform.position = Vector3.Lerp(beginPoint, endPoint, 0.5f);
            preview.transform.LookAt(endPoint);
            if (IsValid(endPoint))
                preview.material = validMat;
            else
                preview.material = invalidMat;
            yield return null;
        }
        
    }


}
