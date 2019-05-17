using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    public UnlockableMenuCategory unlockableMenuCategory;
    public List<Unlockable> unlockables = new List<Unlockable>();
    public List<UnlockablePicker> unlockablePickers = new List<UnlockablePicker>();
    public Scrollbar scrollbar;
    public GameObject pickingPanel;
    public bool panelOpened;
    [Range(1f, 2f)]
    public float spacingToButtonInPanel = 1.5f;
    public RectTransform pickerPosTransform;
    public UnlockablePicker unlockablePickerPrefab;


    //Add unlockable from technology to the list
    public void AddUnlockable(Unlockable unlockable)
    {
        //Add to list
        unlockables.Add(unlockable);
        AddUnlockableToPanel(unlockable, unlockables.IndexOf(unlockable));
    }

    //Add unlockable from the list to the picker panel
    public void AddUnlockableToPanel(Unlockable unlockable, int index)
    {
        //Position
        UnlockablePicker newPicker = Instantiate(unlockablePickerPrefab, pickerPosTransform, false);
        unlockablePickers.Insert(index, newPicker);
        Vector2 offset = new Vector2(index * (newPicker.rectTransform.sizeDelta.x * spacingToButtonInPanel), 0f);
        newPicker.rectTransform.anchoredPosition = offset;

        //Unlockable on button
        newPicker.unlockable = unlockable;
        UpdateUnlockableStatus(unlockable);

        //Sprite
        if (unlockable.iconOnMenu != null)
            newPicker.icon.sprite = unlockable.iconOnMenu;
        else
        {
            newPicker.icon.sprite = unlockable.iconOnTechTree;
        }

        //(De)Activate button
        switch (UnlockManager.instance.unlocksInfo[unlockable].unlockStatus)
        {
            case UnlockStatus.Unlocked:
                newPicker.icon.color = UnlockManager.instance.unlockedColor;
                break;
            case UnlockStatus.Locked:
                newPicker.icon.color = UnlockManager.instance.lockedColor;
                break;
            case UnlockStatus.CantAfford:
                newPicker.icon.color = UnlockManager.instance.cantAffordColor;
                break;
            case UnlockStatus.AtMax:
                newPicker.icon.color = UnlockManager.instance.atMaxColor;
                break;
        }


        //Update scrollbar
        scrollbar.gameObject.SetActive(index<5);
    }

    //Update if you can purchase this button
    public void UpdateUnlockableStatus(Unlockable unlockable)
    {
        //look for unlockable in the pickers
        foreach (UnlockablePicker unlockablePicker in unlockablePickers)
        {
            if (unlockablePicker.unlockable == unlockable)
            {
                //Only be able to buy it if you have it unlocked
                unlockablePicker.Unlock(UnlockManager.instance.unlocksInfo[unlockable].unlockStatus);
                return;
            }
        }

    }

    public void ToggleMenu()
    {
        //Switch panel on and off
        panelOpened = !panelOpened;
        pickingPanel.SetActive(panelOpened);
    }
}
