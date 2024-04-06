using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private void Start()
    {
        TCPConnect.tcpClient.OnDataReceived += HandleReceivedData;
    }

    private void OnDestroy()
    {
        TCPConnect.tcpClient.OnDataReceived -= HandleReceivedData;
    }

    private void HandleReceivedData(string data, string request)
    {
        string[] splitted = data.Split(' ');
        string content = "";
        if (splitted.Length == 1)
        {
            //HandleEndRequest(request, data);
        }
        else if (splitted.Length == 2)
        {
            content = splitted[1];
        }
        else
        {
            content = string.Join(" ", splitted[1..splitted.Length]);
        }
        //if (splitted[0] == "spaceship") SpaceshipsManager.instance.SetSpaceShips(content);
        //else if (splitted[0] == "journey") JourneysManager.instance.SetJourneys(content);
    }

    private void HandleEndRequest(string request, string data)
    {
        string[] splitted = request.Split(' ');
        string command = splitted[0];
        string objectType = splitted[1];
        string content = string.Join(" ", splitted[1..splitted.Length]);

        if (command == "create")
        {
            if (objectType == "spaceship")
            {
                UIManager.instance.AskForSpaceships();
            }
        }
        Debug.Log(data);
    }
}
