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
    public Color loseResourceColor;

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

    public void AddToPlayer(Resource resource, float amount, Transform source, float speed)
    {
        if (amount>0)
        {
            foreach (PlayerResource playerResource in player.resources)
            {
                if (resource == playerResource.resource)
                {
                    //Add it to player amount
                    playerResource.Amount += amount;

                    if(resource.yieldType==YieldType.Science)
                        TechManager.instance.UpdateCurrentTechByAmount(amount);

                    //Trigger event when changing amount
                    EventManager.TriggerEvent(resource.name.ToString());

                    //Popup
                    Popup newPopup = Instantiate(popup, source.position, Quaternion.identity, source);
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

    public void RemoveFromPlayer(Resource resource, float amount, Transform source, float speed)
    {
        if (amount > 0)
        {
            foreach (PlayerResource playerResource in player.resources)
            {
                if (resource == playerResource.resource)
                {
                    //Add it to player amount
                    playerResource.Amount -= amount;

                    if (resource.yieldType == YieldType.Science)
                        TechManager.instance.UpdateCurrentTechByAmount(amount);

                    //Trigger event when changing amount
                    EventManager.TriggerEvent(resource.name.ToString());

                    //Popup
                    Popup newPopup = Instantiate(popup, source.position, Quaternion.identity, source);
                    newPopup.text.text = "-" + amount;
                    newPopup.text.color = loseResourceColor;
                    newPopup.spriteRenderer.sprite = resource.popupSprite;
                    newPopup.animator.Play("PopupUse");
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

    public PlayerResource FindPlayerResource(Resource resource)
    {
        foreach (PlayerResource playerResource in player.resources)
        {
            if (resource == playerResource.resource)
            {
                return playerResource;
            }
        }
        return null;
    }
}
