using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    [SerializeField] private float elapsedTime;
    [SerializeField] private float elapsedHours;
    public enum DayPart { night, dawn, day, dusk};
    [SerializeField] private DayPart daypart;
    private DayPart informedDayPart;
    private float rot;

    private List<ILightable> informants;

    public float timescale = 100;
    void Start()
    {
        elapsedHours = 11.0f;
        elapsedTime = (elapsedHours * 60 * 60 / timescale);

        rot = Mathf.Lerp(-90, 270, elapsedHours / 24);
        transform.rotation = Quaternion.Euler(rot, -30, 0);
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        elapsedHours = elapsedTime / (60 * 60 / timescale);

        if(elapsedHours < 5.0f) daypart = DayPart.night;
        else if(elapsedHours < 7.0f) daypart = DayPart.dawn;
        else if(elapsedHours < 17.0f) daypart= DayPart.day;
        else if(elapsedHours < 19.0f) daypart= DayPart.dusk;
        else daypart= DayPart.night;

        if (informedDayPart != daypart) InformOnDayPartChange(daypart);

        if(elapsedHours > 24.0f)
        {
            elapsedHours = 0;
            elapsedTime -= (elapsedHours * 60 * 60 / timescale);
        }

        rot = Mathf.Lerp(-90, 270, elapsedHours / 24);

        transform.rotation = Quaternion.Euler(rot, -30, 0);
    }

    private void InformOnDayPartChange(DayPart part)
    {
        if (part == DayPart.day) foreach (ILightable lihgt in informants) lihgt.TurnLights(false);
        if(part == DayPart.dusk) foreach(ILightable light in informants) light.TurnLights(true);
        informedDayPart = part;
    }

    public void SetInformant(ILightable inf)
    {
        if(informants == null) informants = new List<ILightable> ();
        if(!informants.Contains (inf)) informants.Add (inf);
    }
}
