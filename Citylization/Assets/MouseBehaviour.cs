using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseMode { Free, CarryingBuilding, PreselectingRoad, PuttingDownRoad }

public class MouseBehaviour : MonoBehaviour
{
    public UnlockableType unlockableType;
    public Building building;
    public Road road;
    public Unlockable selectedUnlockable;
    public MouseMode mode = MouseMode.Free;
    public LayerMask terrainLayer;
    public RoadPlacer roadPlacer;

    public RaycastHit hit;
    public Ray ray;
    public Vector3 mousePosition;

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
                            roadPlacer.SetBeginPoint(mousePosition, road, roadPlacer.beginType);
                            mode = MouseMode.PuttingDownRoad;
                        }
                    }
                    break;

                case MouseMode.PuttingDownRoad:
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (roadPlacer.IsValid(roadPlacer.endPoint, out roadPlacer.endType))
                        {
                            roadPlacer.CreateRoad();
                            mode = MouseMode.Free;
                        }
                    }
                    break;

            }

        }
    }


    public void MoveBuilding()
    {
        building.transform.position = mousePosition;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (BuildingCanBePutDown())
            {
                PutDownBuilding();
            }
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


    public bool BuildingCanBePutDown()
    {

        return true;
    }

    public void PutDownBuilding()
    {
        building.PlaceDownBuilding();
        StopCarrying();
    }

    public void StopCarrying()
    {
        building = null;
        mode = MouseMode.Free;
    }
}
