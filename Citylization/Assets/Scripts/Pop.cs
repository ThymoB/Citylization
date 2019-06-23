using System.Collections.Generic;
using UnityEngine;

public enum PopAge { Junior, Adult, Senior}
public enum PopEducation { None, Primary, Secondary, High }


public class Pop : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float lifeSpan;
    public Household home;
    public int age;
    public PopAge ageGroup;
    public PopEducation education;

    [Header("Current Action")]
    public Building currentAction;

    [Header("Misc")]
    public bool isTourst;

    public void OnSpawn() {
        currentAction = home;
    }

    public void DecideAction() {
        List<Building> possibleActions = GatherActions();
        //possibleActions = SortActionsByPriority(possibleActions);
        if (PickAction(possibleActions, out Building action)) {
            MoveToBuilding(action);
        }
        else {
            GoHome();
        }
    }


    public void MoveToBuilding(Building building) {


    }

    List<Building> GatherActions() {
        List<Building> actions = new List<Building>();
        foreach (Building building in home.buildingsInRange) {
            actions.Add(building);
        }
        return actions;
    }

    List<Building> SortActionsByPriority(List<Building> actions) {
        List<Building> sortedActions = new List<Building>();
        return sortedActions;
    }

    bool PickAction(List<Building> actions, out Building action) {
        if (actions != null) {
            for (int i = 0; i < actions.Count; i++) {
                if (Random.value < PopManager.instance.priorityQuotient) {
                    if (!actions[i].usePopCapacity || (actions[i].usePopCapacity && actions[i].currentPops < actions[i].popCapacity)) {
                        action = actions[i];
                        return true;
                    }
                }
            }
        }
        action = null;
        return false;
    }


    void GoHome() {
    }



}
