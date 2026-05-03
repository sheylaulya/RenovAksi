using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Rendering;

public class DayTimeCycle : MonoBehaviour
{
    public WaterSystem waterSystem;

    [Header("UI")]
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI dayDisplay;
    public TextMeshProUGUI speedDisplay;

    [Header("Post Processing")]
    public Volume ppv;

    [Header("Time Settings")]
    public float tick = 1f;
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    [Header("Initial Time")]
    public int startHour = 6;
    public int startMinute = 0;

    [Header("Lights")]
    public bool activateLights;
    public GameObject[] lights;
    [Header("Sky Group")]
    public Transform daySky;
    public Transform daySky2;
    public Transform nightSky;
    public Transform nightSky2;

    void Start()
    {
        ppv = GetComponent<Volume>();

        // Set initial time
        hours = startHour;
        mins = startMinute;
        seconds = 0;

        // Apply initial state
        ControlPPV();
        DisplayTime();
        UpdateSpeedUI();
    }

    void FixedUpdate()
    {
        CalcTime();
        DisplayTime();
    }

    // =========================
    // TIME CALCULATION
    // =========================
    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60)
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;

            if (waterSystem != null)
                waterSystem.UpdateWaterDaily(this);
        }

        ControlPPV();
    }

    // =========================
    // DAY / NIGHT SYSTEM
    // =========================
    public void ControlPPV()
    {

        float time = hours + mins / 60f;
        float dayFactor = 0f;

        // 🌅 Morning (06:00 → 08:00)
        if (time >= 6f && time < 8f)
        {
            float t = Mathf.InverseLerp(6f, 8f, time);
            dayFactor = Mathf.SmoothStep(0f, 1f, t);

            if (t > 0.5f && activateLights)
            {
                ToggleLights(false);
            }
        }
        // ☀️ Day (08:00 → 16:00)
        else if (time >= 8f && time < 16f)
        {
            dayFactor = 1f;

            if (activateLights)
            {
                ToggleLights(false);
            }
        }
        // 🌇 Evening (16:00 → 19:00)
        else if (time >= 16f && time < 19f)
        {
            float t = Mathf.InverseLerp(16f, 19f, time);
            dayFactor = 1f - Mathf.SmoothStep(0f, 1f, t);

            if (t > 0.5f && !activateLights)
            {
                ToggleLights(true);
            }
        }
        // 🌙 Night
        else
        {
            dayFactor = 0f;

            if (!activateLights)
            {
                ToggleLights(true);
            }
        }

        // Apply brightness
        ppv.weight = 1 - dayFactor;

        UpdateSkyGroup(dayFactor);
    }

    void UpdateSkyGroup(float dayFactor)
    {
        // Day Sky Fade In
        foreach (Transform child in daySky)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = dayFactor;
                sr.color = c;
            }
        }

        foreach (Transform child in daySky2)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = dayFactor;
                sr.color = c;
            }
        }

        // Night Sky Fade Out
        foreach (Transform child in nightSky)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - dayFactor;
                sr.color = c;
            }
        }
        foreach (Transform child in nightSky2)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - dayFactor;
                sr.color = c;
            }
        }
    }

    void ToggleLights(bool state)
    {
        foreach (var light in lights)
        {
            light.SetActive(state);
        }

        activateLights = state;
    }

    // =========================
    // UI DISPLAY
    // =========================
    public void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        dayDisplay.text = "Day: " + days;
    }

    void UpdateSpeedUI()
    {
        if (speedDisplay != null)
        {
            speedDisplay.text = tick == 0 ? "Paused" : tick + "x";
        }
    }

    // =========================
    // TIME CONTROL (UI BUTTONS)
    // =========================
    public void SetTimeSpeed(float newTick)
    {
        tick = newTick;
        UpdateSpeedUI();
    }

    public void PauseTime()
    {
        SetTimeSpeed(0f);
    }

    public void NormalSpeed()
    {
        SetTimeSpeed(100f);
    }

    public void DoubleSpeed()
    {
        SetTimeSpeed(200f);
    }

    public void QuadrupleSpeed()
    {
        SetTimeSpeed(400f);
    }
}