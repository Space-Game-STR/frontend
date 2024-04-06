using System;
using UnityEngine;

[System.Serializable]
public class JourneyClass
{
    public string uuid;
    public string spaceship;
    public long start;
    public long end;
    public string celestialStart;
    public string celestialEnd;

    public JourneyClass(string uuid, string spaceship, long start, long end, string celestialStart, string celestialEnd)
    {
        this.uuid = uuid;
        this.spaceship = spaceship;
        this.start = start;
        this.end = end;
        this.celestialStart = celestialStart;
        this.celestialEnd = celestialEnd;
    }

    public string toString()
    {
        return JsonUtility.ToJson(this);
    }
}
