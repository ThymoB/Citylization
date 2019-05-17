using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    public UnlockableType unlockableType;
    public Building building;
    public bool isCarrying;

    public RaycastHit hit;
    public Ray ray;

    public void StartCarrying()
    {
        unlockableType = Player.instance.selectedUnlockable.type;
        if (unlockableType == UnlockableType.Building)
        {
            //Pick a random model, that one will be put down
            building = Instantiate(Player.instance.selectedUnlockable.GetComponent<Building>());
            building.selectedModel = Instantiate(building.models[Random.Range(0, building.models.Count - 1)], building.transform);

            isCarrying = true;
        }
        
    }

    private void LateUpdate()
    {
        if (isCarrying)
        {
            MoveBuilding();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (BuildingCanBePutDown())
                {
                    PutDownBuilding();
                }
            }
        }
    }

    public void MoveBuilding()
    {
        //Mouse cursor
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            building.transform.position = hit.point;


        }
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
        isCarrying = false;
    }
}
