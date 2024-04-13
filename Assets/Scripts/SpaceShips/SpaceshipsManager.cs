using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsManager : MonoBehaviour
{
    public static SpaceshipsManager instance;
    public List<SpaceShipClass> spaceShips = new();
    public GameObject spaceShipPrefab;
    public Transform spaceShipsParent;

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
        GetSpaceShips();
    }

    void Update()
    {

    }

    public void GetSpaceShips()
    {
        Options options = new(true, false);
        Data data = new(options);

        Command command = new(data, ObjectType.spaceships, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetSpaceShips);
    }

    public void HandleGetSpaceShips(string data, string request)
    {
        SetSpaceShips(data);
    }

    public void SetSpaceShips(string data)
    {
        List<SpaceShipClass> spaceShipClasses = SpaceShipClass.ResponseToList(data);
        //if (spaceShipClasses.Count == 0) return;
        List<string> alreadyExists = new();
        bool continueOuterLoop = true;
        //We first remove all the extra ships and save the ones that exist
        for (int i = 0; i < spaceShips.Count; i++)
        {
            string spaceShipUuid = spaceShips[i].uuid;
            for (int j = 0; j < spaceShipClasses.Count; j++)
            {
                if (spaceShipClasses[j].uuid == spaceShipUuid)
                {
                    //We add them to alreadyExists and we break to continue the outer loop
                    alreadyExists.Add(spaceShipUuid);
                    continueOuterLoop = false;
                    break;
                }
            }
            if (!continueOuterLoop)
            {
                continueOuterLoop = true;
                continue;
            }
            //If it doesnt exist, we eliminate it
            DestroySpaceShip(spaceShipUuid);
        }
        continueOuterLoop = true;
        //And then we see whichs ships that are received are already in space, so we can instantiate those that are not
        for (int i = 0; i < spaceShipClasses.Count; i++)
        {
            SpaceShipClass spaceShip = spaceShipClasses[i];
            for (int j = 0; j < spaceShips.Count; j++)
            {
                if (spaceShip.uuid == spaceShips[j].uuid)
                {
                    continueOuterLoop = false;
                    break;
                }
            }
            if (!continueOuterLoop)
            {
                continueOuterLoop = true;
                continue;
            }
            spaceShips.Add(spaceShip);
            InstantiateSpaceShip(spaceShip);
        }
        //We then get all the journies
        GetSpaceShipJourney();
    }

    private void InstantiateSpaceShip(SpaceShipClass spaceShipClass)
    {
        GameObject gameObject = Instantiate(spaceShipPrefab, Vector2.zero, Quaternion.identity);
        gameObject.GetComponent<SpaceShip>().SetClass(spaceShipClass);
        gameObject.transform.SetParent(spaceShipsParent);
    }

    private void GetSpaceShipJourney()
    {
        Options options = new(true, false);
        GetSpaceShipJourney getSpaceShipJourney = new("");
        Data data = new(JsonUtility.ToJson(getSpaceShipJourney), options);

        Command command = new(data, ObjectType.journeys, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetSpaceShipJoureny);
    }

    private void HandleGetSpaceShipJoureny(string data, string request)
    {
        JourneysManager.instance.SetJourneys(data);
    }

    private bool SearchSpaceship(string uuid)
    {
        for (int i = 0; i < spaceShips.Count; i++)
        {
            if (spaceShips[i].uuid == uuid)
            {
                return true;
            }
        }

        return false;
    }



    private void DestroySpaceShips()
    {
        foreach (Transform child in spaceShipsParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void DestroySpaceShip(string uuid)
    {
        Debug.LogError("Destroying: " + uuid);
        //Destroy the specific spaceship
        foreach (Transform child in spaceShipsParent)
        {
            SpaceShip spaceShip = child.GetComponent<SpaceShip>();
            if (spaceShip.spaceShip.uuid == uuid) Destroy(child.gameObject);
        }

        for (int i = 0; i < spaceShips.Count; i++)
        {
            if (spaceShips[i].uuid == uuid) spaceShips.RemoveAt(i);
        }
    }
}

class GetSpaceShipJourney
{
    public string uuid;

    public GetSpaceShipJourney(string uuid)
    {
        this.uuid = uuid;
    }
}