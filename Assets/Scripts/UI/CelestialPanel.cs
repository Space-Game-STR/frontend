using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelestialPanel : MonoBehaviour
{
    private Celestial celestial;
    public Transform spaceShipsContentTransform;
    public GameObject spaceShipItemPrefab;
    public TextMeshProUGUI spaceShipNameInput;
    public Button createSpaceShipButton;

    public void SetCelestial(Celestial _celestial)
    {
        ClearCelestial();
        celestial = _celestial;
        createSpaceShipButton.onClick.AddListener(() =>
        {
            string name = spaceShipNameInput.text.Trim((char)8203);

            if (name == "")
            {
                Debug.LogWarning("Inputfield for spaceship name is empty...");
                return;
            }
            celestial.CreateSpaceship(name);
            createSpaceShipButton.interactable = false;
        });
        DestroySpaceShipsList();
    }

    public void ClearCelestial()
    {
        createSpaceShipButton.onClick.RemoveAllListeners();
    }

    public void RegenerateSpaceShips()
    {
        DestroySpaceShipsList();
        celestial.GetSpaceShips();
        createSpaceShipButton.interactable = true;
    }

    private void DestroySpaceShipsList()
    {
        foreach (Transform child in spaceShipsContentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetSpaceShips(SpaceShipClass[] spaceShips)
    {
        SetPanelContent(spaceShips);
    }

    private void SetPanelContent(SpaceShipClass[] spaceShips)
    {
        float itemHeight = spaceShipItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        spaceShipsContentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, spaceShips.Length * itemHeight); //We set the content size to whatever the height of the item is :3
        float y = -25;

        for (int i = 0; i < spaceShips.Length; i++)
        {
            Vector3 position = new Vector3(0, y);
            GameObject item = Instantiate(spaceShipItemPrefab, spaceShipsContentTransform);
            item.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.offsetMin = new Vector2(0, rt.offsetMin.y);
            rt.offsetMax = new Vector2(0, rt.offsetMax.y);

            item.GetComponent<SpaceShipItem>().SetSpaceShipData(spaceShips[i]);

            y -= itemHeight;
        }
    }
}