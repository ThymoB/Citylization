using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Era { Ancient, Classical, Medieval, Renaissance, Enlightenment, Industrial, Modern, Atomic, Information, Future }


public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;


    [Header("Time Info")]
    public int hoursInDay = 24;
    public int daysInMonth = 30;
    public int monthsInYear = 4;
    public int startYear = -4000;
    public int startHour = 8;

    public int curHour;
    public int curDay = 1;
    public int curMonth = 1;
    public int curYear;

    public int endOfDay = 18;

    public string[] monthNames;

    [Header("Speed")]
    public float gameSpeed = 2f;
    [Range (0.25f, 4f)]
    public float timeScale = 1f;
    public bool gameIsPaused = false;
    public float slowSpeed = 0.5f;
    public float medSpeed = 1f;
    public float fastSpeed = 2f;

    //How much you leap ahead in years for every year threshold
    [System.Serializable]
    public class YearGaps
    {
        public int yearThreshold;
        public int yearJumps;
    }

    [Header("Year Gaps")]
    [SerializeField]
    public YearGaps[] yearGaps;
    public int curYearGaps;

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


    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    private void Init()
    {
        curHour = startHour;
        curDay = 1;
        curMonth = 1;
        curYear = startYear;
        curYearGaps = 0;
        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1 / gameSpeed);
            UpdateHour();
            if(curHour>=hoursInDay)
                UpdateDay();

            if(curDay>daysInMonth)
                UpdateMonth();

            if(curMonth>monthsInYear)
                UpdateYear();

            //Hourly update event;
            UpdateTime();
        }
    }

    public void UpdateHour()
    {
        curHour++;
    }

    public void UpdateDay()
    {
        curHour = 0;
        curDay++;
        EventManager.TriggerEvent("day");
    }




    public void UpdateMonth()
    {
        curDay = 1;
        curMonth++;
    }


    public void UpdateYear()
    {
        curMonth = 1;
        //Look up which year gap to use
        for (int i = 0; i < yearGaps.Length; i++)
        {
            if(curYear>yearGaps[i].yearThreshold)
            {
                curYearGaps = i;
                break;
            }
        }
        //Increase the year by the yearGaps
        curYear = curYear + yearGaps[curYearGaps].yearJumps;
    }

    public void UpdateTime()
    {
        EventManager.TriggerEvent("time");
    }

    public string CurYear()
    {
        string year;
        int yearText = Mathf.Abs(curYear);
        if (curYear < 0)
            year = yearText + " BC";
        else
            year = yearText + " AD";
        return year;
    }

}
