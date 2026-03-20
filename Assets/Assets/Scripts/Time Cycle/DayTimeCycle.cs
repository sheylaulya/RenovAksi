using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // using text mesh for the clock display

using UnityEngine.Rendering; // used to access the volume component

public class DayTimeCycle : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay; // Display Time
    public TextMeshProUGUI dayDisplay; // Display Day
    public Volume ppv; // this is the post processing volume

    public float tick; // Increasing the tick, increases second rate
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public bool activateLights; // checks if lights are on
    public GameObject[] lights; // all the lights we want on when its dark

    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
    }

    // Update is called once per frame
    void FixedUpdate() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
        DisplayTime();

    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }

    public void ControlPPV()
    {
        float time = hours + mins / 60f;
        float dayFactor = 0f;

        // 🌅 06:00 → 08:30 (transisi pagi)
        if (time >= 6f && time < 8f)
        {
            float t = Mathf.InverseLerp(6f, 8f, time);
            dayFactor = Mathf.SmoothStep(0f, 1f, t);

            // 💡 Lampu mati di tengah transisi pagi
            if (t > 0.5f && activateLights)
            {
                foreach (var light in lights)
                    light.SetActive(false);

                activateLights = false;
            }
        }
        // ☀️ 08:30 → 16:00 (siang full terang)
        else if (time >= 8f && time < 16f)
        {
            dayFactor = 1f;

            // pastikan lampu mati
            if (activateLights)
            {
                foreach (var light in lights)
                    light.SetActive(false);

                activateLights = false;
            }
        }
        // 🌇 16:00 → 19:00 (transisi sore)
        else if (time >= 16f && time < 19f)
        {
            float t = Mathf.InverseLerp(16f, 19f, time);
            dayFactor = 1f - Mathf.SmoothStep(0f, 1f, t);

            // 💡 Lampu nyala di tengah transisi sore
            if (t > 0.5f && !activateLights)
            {
                foreach (var light in lights)
                    light.SetActive(true);

                activateLights = true;
            }
        }
        // 🌙 Malam
        else
        {
            dayFactor = 0f;

            // pastikan lampu nyala
            if (!activateLights)
            {
                foreach (var light in lights)
                    light.SetActive(true);

                activateLights = true;
            }
        }

        // 🌗 Apply brightness
        ppv.weight = 1 - dayFactor;


    }
    public void DisplayTime() // Shows time and day in ui
    {

        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        dayDisplay.text = "Day: " + days; // display day counter
    }
}