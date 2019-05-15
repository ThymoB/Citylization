using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    public static ResourceSystem instance;
    public Player player;
    public Popup popup;
    [Range(0f,1f)]
    public float popupSpeed = 0.5f;
    [Range(0f,0.99f)]
    public float intervalVariance = 0.2f;
    public LayerMask householdMask;
    public float rangeHeight = 2f;
    public GameObject showRangeObject;

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

    public void AddToPlayer(Resource resource, float amount, Transform building, float speed)
    {
        if (amount>0)
        {
            foreach (PlayerResource playerResource in player.resources)
            {
                if (resource == playerResource.resource)
                {
                    playerResource.Amount += amount;

                    //Popup
                    Popup newPopup = Instantiate(popup, building.position, Quaternion.identity, building);
                    newPopup.text.text = "+" + amount;
                    newPopup.text.color = resource.color;
                    newPopup.spriteRenderer.sprite = resource.popupSprite;
                    newPopup.animator.Play("Popup");
                    newPopup.animator.speed = Mathf.Pow(speed, popupSpeed);
                    Destroy(newPopup.gameObject, 1 / Mathf.Pow(speed, popupSpeed));

                    break;
                }
            }
        }
    }

    public void YieldPerDay(Resource resource, float change)
    {
        foreach (PlayerResource playerResource in player.resources)
        {
            if (resource == playerResource.resource)
            {
                playerResource.AmountPerDay += change;
                break;
            }
        }
    }

}
