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

    // CELESTIALS NETWORKING

    /// <summary>
    /// Method to get all celestials
    /// </summary>
    void GetAllCelestials()
    {
        Options options = new(true, false);
        Data data = new(options);

        Command command = new(data, ObjectType.celestials, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetCelestials);
    }

    /// <summary>
    /// Method to handle the response from the server with the celestials
    /// </summary>
    /// <param name="data"></param>
    /// <param name="request"></param>
    public void HandleGetCelestials(string data, string request)
    {
        try
        {
            SetCelestials(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing " + data);
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Method to get all the spaceships in an specific celestial
    /// </summary>
    /// <param name="uuid"></param>
    public void GetSpaceShipsInCelestial(string uuid)
    {
        Options options = new(false, true, uuid);
        Data data = new(options);

        Command command = new(data, ObjectType.spaceships, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetSpaceShipsInCelestial);
    }

    /// <summary>
    /// Method to handle all the spaceships in an specific celestial
    /// </summary>
    /// <param name="data"></param>
    /// <param name="request"></param>
    void HandleGetSpaceShipsInCelestial(string data, string request)
    {
        try
        {
            Response<SpaceShipClass[]> response = JsonUtility.FromJson<Response<SpaceShipClass[]>>(data);
            UIManager.instance.SetSpaceShipsForCelestialPanel(response.data);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    // CELESTIALS MANAGER METHODS

    /// <summary>
    /// Method to set all the celestials
    /// </summary>
    /// <param name="data"></param>
    public void SetCelestials(string data)
    {
        Response<CelestialClass[]> response = JsonUtility.FromJson<Response<CelestialClass[]>>(data);
        CelestialClass[] celestialsData = response.data;

        for (int i = 0; i < celestialsData.Length; i++)
        {
            CelestialClass celestial = celestialsData[i];
            if (SearchCelestial(celestial.uuid)) continue; //We continue because it already exists
            InstantiateCelestial(celestial);
        }
    }

    /// <summary>
    /// Method to instantiate a celestial object based in its class
    /// </summary>
    /// <param name="celestial"></param>
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

    /// <summary>
    /// Method to see if a celestial exists by uuid
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Method to search a Celestial and return it by uuid
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
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
