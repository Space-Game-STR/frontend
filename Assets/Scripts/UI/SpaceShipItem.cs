using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipItem : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public Button createJourneyButton;

    public void SetSpaceShipData(SpaceShipClass spaceShip)
    {
        nameText.text = spaceShip.name;

        createJourneyButton.onClick.AddListener(() =>
        {
            CreateJourney(spaceShip.uuid, "95e58df9-0453-4946-b5db-f1fc88065285"); //Planet uuid set for testing
        });
    }

    public void CreateJourney(string ship_uuid, string celestial_objective_uuid)
    {
        Options options = new(true, false);
        CreateJourneyContent createJourneyContent = new(ship_uuid, celestial_objective_uuid);
        Data data = new(JsonUtility.ToJson(createJourneyContent), options);

        Command command = new(data, ObjectType.journeys, TCPCommand.create);

        TCPConnect.tcpClient.addRequestToQueue(command.getString(), HandleCreateJourney);
    }

    private void HandleCreateJourney(string data, string request)
    {
        UIManager.instance.AskForSpaceships();
        SpaceshipsManager.instance.GetSpaceShips();
    }
}


class CreateJourneyContent
{
    public string uuid;
    public string objectiveCelestialUuid;

    public CreateJourneyContent(string uuid, string objectiveCelestialUuid)
    {
        this.uuid = uuid;
        this.objectiveCelestialUuid = objectiveCelestialUuid;
    }
}