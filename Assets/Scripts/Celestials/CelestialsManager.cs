using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialsManager : MonoBehaviour
{
    public static CelestialsManager instance;
    public List<Celestial> celestials = new List<Celestial>();
    public GameObject celestialPrefab;

    public Transform celestialsParent;

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

    void Start()
    {
        GetAllCelestials();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetAllCelestials();
        }
    }

    void GetAllCelestials()
    {
        Options options = new(true, false);
        Data data = new(options);

        Command command = new(data, ObjectType.celestial, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetCelestials);
    }

    public void HandleGetCelestials(string data, string request) {
        SetCelestials(data);
    }

    public void SetCelestials(string data)
    {
        string[] _celestials = data.Split("celestial");
        for (int i = 0; i < _celestials.Length; i++)
        {
            if (_celestials[i] == "") continue;
            try
            {
                CelestialClass celestial = JsonUtility.FromJson<CelestialClass>(_celestials[i]);
                if (SearchCelestial(celestial.uuid)) continue; //We continue because it already exists
                InstantiateCelestial(celestial);
            }
            catch (Exception)
            {
                Debug.LogWarning("Error parsing " + _celestials[i]);
            }

        }
    }

    private void InstantiateCelestial(CelestialClass celestial)
    {
        float x = (float)(celestial.distanceFromSun * Math.Cos(celestial.angle));
        float y = (float)(celestial.distanceFromSun * Math.Sin(celestial.angle));

        GameObject celestialObject = Instantiate(celestialPrefab, new Vector2(x, y), Quaternion.identity, celestialsParent);
        celestialObject.GetComponent<Orbit>().radius = celestial.distanceFromSun;
        celestialObject.transform.localScale = new Vector2(celestial.radius, celestial.radius);

        Celestial celestialScript = celestialObject.GetComponent<Celestial>();
        celestialScript.SetData(celestial);
        celestials.Add(celestialScript);
    }

    private bool SearchCelestial(string uuid)
    {
        for (int i = 0; i < celestials.Count; i++)
        {
            if (celestials[i].uuid == uuid)
            {
                return true;
            }
        }

        return false;
    }

    public Celestial GetCelestial(string uuid)
    {
        for (int i = 0; i < celestials.Count; i++)
        {
            if (celestials[i].uuid == uuid)
            {
                return celestials[i];
            }
        }

        return null;
    }
}
