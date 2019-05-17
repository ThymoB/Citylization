using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockablePicker : MonoBehaviour
{
    public Unlockable unlockable;
    public Image icon;
    public RectTransform rectTransform;
    public UnlockStatus status;

    public void Unlock(UnlockStatus unlockStatus)
    {
        status = unlockStatus;

    }

    public void SelectThisUnlockable()
    {
        if (status==UnlockStatus.Unlocked)
        {
            Player.instance.SelectUnlockable(unlockable);
        }
        else
        {
            Debug.LogWarning("UNLOCKABLE PICKER: " + unlockable + " not unlocked yet, or not enough money/atMax!");

        }

    }
}
