using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceShipClass
{
    public string _id;
    public string uuid;
    public string name;
    public bool orbitingCelestial;
    public string celestialOrbiting;
    public string currentJourney;
    public float velocity;

    public SpaceShipClass(string uuid, string name, bool orbitingCelestial, string celestialOrbiting, string currenJourneyId, float velocity)
    {
        this.uuid = uuid;
        this.name = name;
        this.orbitingCelestial = orbitingCelestial;
        this.celestialOrbiting = celestialOrbiting;
        this.currentJourney = currenJourneyId;
        this.velocity = velocity;
    }

    public string toString()
    {
        return JsonUtility.ToJson(this);
    }

    public static List<SpaceShipClass> ResponseToList(string data)
    {
        string[] spaceshipsData = data.Split("spaceship");
        List<SpaceShipClass> spaceShips = new();

        for (int i = 0; i < spaceshipsData.Length; i++)
        {
            if (spaceshipsData[i] == "") continue;
            try
            {
                SpaceShipClass spaceShip = JsonUtility.FromJson<SpaceShipClass>(spaceshipsData[i]);
                spaceShips.Add(spaceShip);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error parsing " + spaceshipsData[i]);
                Debug.LogWarning(e);
            }
        }

        return spaceShips;
    }
}
