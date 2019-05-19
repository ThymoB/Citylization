using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public List<PlayerResource> resources;
    public List<Technology> learnedTechnologies;
    public GameObject techTree;



    public MouseBehaviour mouseBehaviour;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
    }


    public void DeselectUnlockable()
    {
        mouseBehaviour.StopCarrying();
    }

}
