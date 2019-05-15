using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeGetter : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = TimeSystem.instance.curHour + ":00 - " + TimeSystem.instance.curDay + " - " + TimeSystem.instance.monthNames[TimeSystem.instance.curMonth - 1] + " - " + TimeSystem.instance.CurYear();
    }
}
