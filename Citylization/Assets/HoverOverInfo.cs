﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverOverInfo : MonoBehaviour
{
    public GameObject panel;
    public Image backdrop;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void Show(Unlockable unlockable) {
        panel.SetActive(true);
        nameText.text = unlockable.name;
        descriptionText.text = unlockable.description.description;
    }

    public void Hide() {
        panel.SetActive(false);
    }
}