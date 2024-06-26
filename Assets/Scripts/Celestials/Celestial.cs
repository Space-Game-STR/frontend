using System.Collections.Generic;
using UnityEngine;

public class Celestial : MonoBehaviour
{
    public string uuid;
    public float radius;
    public float angle;
    public float distanceFromSun;

    private CelestialClass celestialClass;

    public void SetData(CelestialClass celestial)
    {
        uuid = celestial.uuid;
        radius = celestial.radius;
        angle = celestial.angle;
        distanceFromSun = celestial.distanceFromSun;
        celestialClass = celestial;
    }

    public void OnMouseDown()
    {
        UIManager.instance.CelestialSelected(this);
        GetSpaceShips();
    }

    public void GetSpaceShips()
    {
        Options options = new(false, true);
        Data data = new(celestialClass.toString(), options);

        Command command = new(data, ObjectType.spaceship, TCPCommand.get);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleGetSpaceShips);
    }

    public void HandleGetSpaceShips(string data, string request)
    {
        List<SpaceShipClass> spaceShips = SpaceShipClass.ResponseToList(data);
        UIManager.instance.SetSpaceShipsForCelestialPanel(spaceShips);
    }

    public void CreateSpaceship(string name)
    {
        Options options = new();
        SpaceShipClass spaceShip = new SpaceShipClass("", name, true, uuid, "", 1);
        Data data = new(spaceShip.toString(), options);

        Command command = new(data, ObjectType.spaceship, TCPCommand.create);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleCreateSpaceShip);
    }

    public void HandleCreateSpaceShip(string data, string request)
    {
        UIManager.instance.AskForSpaceships();
    }
}
