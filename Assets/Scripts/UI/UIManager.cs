using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum States
    {
        Celestial,
        Map
    }
    public static UIManager instance;
    public States currentState = States.Map;

    public GameObject celestialPanel;

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
        celestialPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == States.Celestial)
            {
                CelestialUnselect();
            }
        }
    }

    public void AskForSpaceships()
    {
        if (currentState != States.Celestial) return;

        celestialPanel.GetComponent<CelestialPanel>().RegenerateSpaceShips();
    }

    public void CelestialSelected(Celestial celestial)
    {
        if (currentState != States.Celestial)
        {
            currentState = States.Celestial;
            celestialPanel.SetActive(true);
        }

        celestialPanel.GetComponent<CelestialPanel>().SetCelestial(celestial);
    }

    public void CelestialUnselect()
    {
        currentState = States.Map;
        celestialPanel.SetActive(false);
        SpaceshipsManager.instance.GetSpaceShips();
    }

    public void SetSpaceShipsForCelestialPanel(SpaceShipClass[] spaceShips)
    {
        celestialPanel.GetComponent<CelestialPanel>().SetSpaceShips(spaceShips);
    }
}
