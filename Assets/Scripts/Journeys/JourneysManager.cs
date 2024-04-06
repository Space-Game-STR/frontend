using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneysManager : MonoBehaviour
{
    public static JourneysManager instance;
    public List<JourneyClass> journeys = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetJourneys(string data)
    {
        string[] _journeys = data.Split("journey");
        journeys.Clear();
        for (int i = 0; i < _journeys.Length; i++)
        {
            if (_journeys[i] == "") continue;
            try
            {
                JourneyClass journey = JsonUtility.FromJson<JourneyClass>(_journeys[i]);
                journeys.Add(journey);
            }
            catch (Exception)
            {
                Debug.LogWarning("Error parsing " + _journeys[i]);
            }
        }
    }

    public JourneyClass SearchJourneyByShip(string uuid)
    {
        for (int i = 0; i < journeys.Count; i++)
        {
            if (journeys[i].spaceship == uuid)
            {
                return journeys[i];
            }
        }

        return null;
    }
}
