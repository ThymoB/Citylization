using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClickType { Road, Node, Free }

public  class RoadPlacer : MonoBehaviour
{
    public ClickType beginType;
    public ClickType endType;
    public Node nodePrefab;
    public Node selectedNode;
    public float snapSize = 1;
    public Vector3 beginPoint;
    public Vector3 endPoint; 
    public MouseBehaviour mouseBehaviour;
    public GameObject previewPrefab;
    public Material validMat;
    public Material invalidMat;
    public Transform roadsParent;
    [Header("Grid")]
    public bool useGrid;
    public float gridSize = 1f;

    private bool creatingLine;
    private GameObject preview;
    private MeshRenderer previewMR;
    private Road road;
    public LayerMask roadLayer;
    public LayerMask roadAndNodeLayer;

    public bool IsValid(Vector3 place, out ClickType _clickType)
    {
        Collider[] colliders = Physics.OverlapSphere(place, snapSize, roadAndNodeLayer);

        _clickType = ClickType.Free;
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders) {
            if (collider.gameObject.layer == roadLayer) {
                _clickType = ClickType.Road;
            }
            else {
                _clickType = ClickType.Node;
                Node curNode = collider.GetComponent<Node>();
                float curDistance = Vector3.Distance(curNode.transform.position, place);
                if (curDistance < minDistance) selectedNode = curNode;
            }
        }
        if(creatingLine) {

        }
        return true;

    }

    public void SetBeginPoint(Vector3 _beginPoint, Road _road, ClickType _clickType) {

        switch (_clickType) {
            case ClickType.Node:
                beginPoint = selectedNode.transform.position;
                break;
            case ClickType.Road:
                beginPoint = SnapToExistingRoad(_beginPoint);
                break;
            case ClickType.Free:
                beginPoint = _beginPoint;
                break;
        }
        road = _road;
        preview = Instantiate(previewPrefab, beginPoint, Quaternion.identity, transform);
        creatingLine = true;
        StartCoroutine(ShowRoad());
    }

    public void SetEndPoint(Vector3 _endPoint, ClickType _clickType) {

        switch (_clickType) {
            case ClickType.Node:
                endPoint = selectedNode.transform.position;
                break;
            case ClickType.Road:
                endPoint = SnapToExistingRoad(_endPoint);
                break;
            case ClickType.Free:
                if (useGrid) endPoint = PlacementUtils.SnapToGrid(beginPoint, endPoint, gridSize);
                else endPoint = _endPoint;
                break;
        }
    }


    public void CreateRoad()
    {
        StopCoroutine(ShowRoad());
        creatingLine = false;

        //Straight road
        Road newRoad = Instantiate(road, beginPoint, Quaternion.identity, roadsParent);
        newRoad.transform.localScale = preview.transform.localScale;
        newRoad.transform.LookAt(endPoint);

        //Create nodes
        CreateNode(beginType, beginPoint);
        CreateNode(endType, endPoint);
        

        //Delete preview
        Destroy(preview.gameObject);
    }

    void CreateNode(ClickType clickType, Vector3 point) {
        switch (clickType) {
            case ClickType.Free:
                Instantiate(nodePrefab, point, Quaternion.identity, transform);
                break;
            case ClickType.Node:
                break;
            case ClickType.Road:
                Instantiate(nodePrefab, SnapToExistingRoad(point), Quaternion.identity, transform);
                break;
        }
    }


    IEnumerator ShowRoad()
    {
        while(creatingLine)
        {
            if (IsValid(endPoint, out endType)) {
                SetEndPoint(mouseBehaviour.mousePosition, endType);
                preview.transform.localScale = new Vector3(1, 1, Vector3.Distance(beginPoint, endPoint));
                preview.transform.position = Vector3.Lerp(beginPoint, endPoint, 0.5f);
                preview.transform.LookAt(endPoint);
                previewMR.material = validMat;
            }
            else
                previewMR.material = invalidMat;
            yield return null;
        }
    }

    Vector3 SnapToExistingRoad(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Road roadToSnapTo;
        if (Physics.Raycast(ray, out RaycastHit hit, 100, roadLayer)) {

            roadToSnapTo = hit.collider.GetComponentInParent<Road>();
            Debug.Log(roadToSnapTo);
        }
        return Vector3.zero;

    }


}
