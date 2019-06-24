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
    public float snapSize = 0.5f;
    public Vector3 beginPoint;
    public Vector3 endPoint; 
    public MouseBehaviour mouseBehaviour;
    public GameObject previewPrefab;
    public Material validMat;
    public Material invalidMat;
    public Transform roadsParent;
    public bool creatingLine;
    [Header("Grid")]
    public bool useGrid;
    public float gridSize = 1f;

    private GameObject preview;
    private MeshRenderer previewMR;
    private Road road;
    public LayerMask roadLayer;
    public LayerMask roadAndNodeLayer;

    public bool IsValid(Vector3 place, out ClickType _clickType) {
        if (GetNodeAtPoint(place, out selectedNode)) {
            _clickType = ClickType.Node;
        }
        else if (GetRoadAtPoint(place, out Road foundRoad)) {
            _clickType = ClickType.Road;
        }
        else _clickType = ClickType.Free;


        
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
        previewMR = preview.GetComponent<MeshRenderer>();
        creatingLine = true;
        endPoint = beginPoint;
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
                if (useGrid) endPoint = PlacementUtils.SnapToGrid(beginPoint, _endPoint, gridSize);
                else endPoint = _endPoint;
                break;
        }
    }


    public void CreateRoad()
    {
        StopCoroutine(ShowRoad());
        creatingLine = false;

        //Create nodes
        Node beginNode = CreateNode(beginType, beginPoint);
        selectedNode = CreateNode(endType, endPoint);

        //Create Roads between nodes
        Road newRoad = Instantiate(road, beginPoint, Quaternion.identity, roadsParent);
        newRoad.transform.localScale = preview.transform.localScale;
        newRoad.transform.LookAt(endPoint);
        ConnectRoadEndsAroundNode(beginNode);

        //Delete preview
        Destroy(preview);



        //Begin new road
        SetBeginPoint(beginPoint, road, ClickType.Node);
    }

    Node CreateNode(ClickType clickType, Vector3 point) {
        switch (clickType) {
            case ClickType.Free:
                return Instantiate(nodePrefab, point, Quaternion.identity, transform);
            case ClickType.Node:
                GetNodeAtPoint(point, out Node node);
                return node;
            case ClickType.Road:
                return Instantiate(nodePrefab, SnapToExistingRoad(point), Quaternion.identity, transform);
        }
        return null;
    }


    IEnumerator ShowRoad()
    {
        while (creatingLine) {
            //Curve line
            if(Input.GetKeyDown(KeyCommands.instance.curveRoad)) {
                //CurveRoad();
                Debug.Log("Curving Road!");
            }

            //Straight line
            IsValid(mouseBehaviour.mousePosition, out endType);
            SetEndPoint(mouseBehaviour.mousePosition, endType);
            preview.transform.localScale = new Vector3(1, 1, Vector3.Distance(beginPoint, endPoint));
            preview.transform.position = Vector3.Lerp(beginPoint, endPoint, 0.5f);
            preview.transform.LookAt(endPoint);
            previewMR.material = validMat;
            yield return null;
        }
    }

    Vector3 SnapToExistingRoad(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Road roadToSnapTo;
        if (Physics.Raycast(ray, out RaycastHit hit, 100, roadLayer)) {
            roadToSnapTo = hit.collider.GetComponentInParent<Road>();
        }
        return point;

    }

    public void CancelPlacing() {
        Destroy(preview);
        StopCoroutine(ShowRoad());
        creatingLine = false;
    }

    bool GetNodeAtPoint(Vector3 point, out Node node) {
        node = null;
        Collider[] colliders = Physics.OverlapSphere(point, snapSize);
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders) {
            if (collider.tag == "Node") {
                Node curNode = collider.GetComponent<Node>();
                float curDistance = Vector3.Distance(curNode.transform.position, point);
                if (curDistance < minDistance) node = curNode;
            }
        }
        if (node != null) return true;
        else return false;
    }

    bool GetRoadAtPoint(Vector3 point, out Road road) {
        road = null;
        Collider[] colliders = Physics.OverlapSphere(point, snapSize);
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders) {
            if (collider.tag == "Road") {
                Road curRoad = collider.GetComponent<Road>();
                float curDistance = Vector3.Distance(curRoad.transform.position, point);
                if (curDistance < minDistance) road = curRoad;
            }
        }
        if (road != null) return true;
        else return false;
    }

    void ConnectRoadEndsAroundNode(Node node) {
        //Look up corner vertices of all roads around a node and make their transform the same
    }

    public void CurveRoad() {
        if (creatingLine) {
            SetEndPoint(mouseBehaviour.mousePosition, ClickType.Free);


        }
    }

}
