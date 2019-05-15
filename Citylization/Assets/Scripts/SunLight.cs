using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SunLight : MonoBehaviour
{
    public GameObject sun;
    public GameObject moon;

    public float speed = 100f;

    [Range(0.01f, 0.99f)]
    public float dayNightBalance = 0.5f;


    private void FixedUpdate()
    {


        sun.transform.localRotation = Quaternion.Lerp(sun.transform.localRotation, Quaternion.Euler((DayNightBalance()) - 90, 170, 0), TimeSystem.instance.gameSpeed/speed);
        moon.transform.localRotation = Quaternion.Lerp(moon.transform.localRotation, Quaternion.Euler((DayNightBalance()) - 270, 170, 0), TimeSystem.instance.gameSpeed/speed);
    }

    float DayNightBalance()
    {

        float multiplier = TimeSystem.instance.curHour * 15;
        //float multiplier = Mathf.Pow(TimeSystem.instance.curHour, dayNightBalance) * 


        return multiplier;
    }
}
