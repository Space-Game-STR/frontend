using UnityEngine;

[System.Serializable]
public class CelestialClass
{
    public string _id;
    public string uuid;
    public float radius;
    public float angle;
    public float distanceFromSun;

    public CelestialClass(string uuid, float radius, float angle, float distanceFromSun)
    {
        this.uuid = uuid;
        this.radius = radius;
        this.angle = angle;
        this.distanceFromSun = distanceFromSun;
    }

    public string toString()
    {
        return JsonUtility.ToJson(this);
    }
}