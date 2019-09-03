using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MouseMode { Free, CarryingBuilding, PreselectingRoad, PuttingDownRoad }

public class MouseBehaviour : MonoBehaviour
{
    public UnlockableType unlockableType;
    public Building building;
    public float buildingRoadSnapRadius = 0.5f;
    public Road road;
    public Unlockable selectedUnlockable;
    public MouseMode mode = MouseMode.Free;
    public LayerMask terrainLayer;
    public RoadPlacer roadPlacer;
    public HoverOverInfo hoverOverInfo;
    public RaycastHit hit;
    public Ray ray;
    public Vector3 mousePosition;

    private Color baseColor;

    //Select tool based on unlockable
    public void SelectUnlockable(Unlockable unlockable)
    {
        selectedUnlockable = unlockable;
        unlockableType = selectedUnlockable.type;
        switch(unlockableType)
        {
            case UnlockableType.Building:
                StartCarryingBuilding();
                break;

            case UnlockableType.Road:
                StartPlacingRoad();
                break;
        }
    }

    public void StartCarryingBuilding()
    {
        //Pick a random model, that one will be put down
        building = Instantiate(selectedUnlockable.GetComponent<Building>());
        building.selectedModel = Instantiate(building.models[Random.Range(0, building.models.Count - 1)], building.transform);
        building.rend = building.selectedModel.GetComponent<Renderer>();
        baseColor = building.rend.material.color;
        mode = MouseMode.CarryingBuilding;
    }

    public void StartPlacingRoad()
    {
        road = selectedUnlockable.GetComponent<Road>();
        mode = MouseMode.PreselectingRoad;

    }


    private void LateUpdate()
    {
        if (GetMousePosition())
        {
            switch (mode)
            {
                //Carrying Building
                case MouseMode.CarryingBuilding:
                    MoveBuilding();
                    break;

                //Creating a road
                case MouseMode.PreselectingRoad:
                    
                    if(Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (roadPlacer.IsValid(mousePosition, out ClickType clickType))
                        {
                            roadPlacer.SetBeginPoint(mousePosition, road, clickType);
                            mode = MouseMode.PuttingDownRoad;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1)) {
                        mode = MouseMode.Free;
                    }
                    break;

                case MouseMode.PuttingDownRoad:
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (roadPlacer.IsValid(roadPlacer.endPoint, out roadPlacer.endType))
                        {
                            roadPlacer.CreateRoad();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1)) {
                        if (roadPlacer.creatingLine) {
                            roadPlacer.CancelPlacing();
                            mode = MouseMode.Free;
                        }
                    }
                    break;

            }

        }
    }


    public void MoveBuilding()
    {
        bool canBePutDown = BuildingCanBePutDown(out Road roadToSnap);
        if (canBePutDown) building.rend.material.SetColor("_Color", Color.green);
        else building.rend.material.SetColor("_Color", Color.red);

        if (roadToSnap != null) {
            building.transform.position = SnapToRoad(roadToSnap);
        }
        else
        building.transform.position = mousePosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canBePutDown) { 
                PutDownBuilding();
            }
            else {
                Debug.Log(building + " can't be put down!");
            }
                
        }

        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            CancelPuttingDown();
        }
    }



    public bool GetMousePosition()
    { 
        //Mouse cursor
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, terrainLayer))
        {
            mousePosition = hit.point;
            return true;
        }
        return false;
    }


    public bool BuildingCanBePutDown(out Road roadToSnap)
    {
        roadToSnap = null;
        bool allowed = false;
        if (!building.needsToBeRoadside) allowed = true;
        if(building.needsToBeRoadside) {
            roadToSnap = PlacementUtils.GetClosestRoad(mousePosition, buildingRoadSnapRadius);
            if(roadToSnap!=null) {
                allowed = true;
            }
        }
        return allowed;
    }

    public Vector3 SnapToRoad(Road roadToSnapTo) {
        Vector3 pos = mousePosition;
        //Face to road
        building.transform.LookAt(roadToSnapTo.transform);
        //Shoot ray forward to determine where to put it
        if(Physics.Raycast(building.transform.position, building.transform.TransformDirection(Vector3.forward), out RaycastHit hit, buildingRoadSnapRadius)) {
            Debug.DrawRay(building.transform.position, building.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Hit!" + hit.distance);
            pos = hit.point - new Vector3(0, 0, roadToSnapTo.width);
        }

        return pos;
    }


    public void PutDownBuilding()
    {
        building.rend.material.color = baseColor;
        building.PlaceDownBuilding();
        StopCarrying();
    }

    public void StopCarrying()
    {
        building = null;
        mode = MouseMode.Free;
    }

    public void CancelPuttingDown() {
        Destroy(building.gameObject);
        StopCarrying();
    }
}
