using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LocationMenu : MonoBehaviour
{
    public GameObject locationPrefab;
    public Transform locationList;
    public Button addLocationButton;
    public TMP_InputField inputLatitude;
    public TMP_InputField inputLongitude;
    private const int MaxLocations = 4;
    private List<SavedLocations> savedLocations = new List<SavedLocations>();

    private void Start()
    {
        LoadLocations();
        UpdateAddButtonState();
    }

    public void AddLocation(SavedLocations location, GameObject locationUI)
    {
        
        TMP_InputField[] inputFields = locationUI.GetComponentsInChildren<TMP_InputField>();
        location.locationName = inputFields[0].text;
        location.latitude = float.Parse(inputFields[1].text);
        location.longitude = float.Parse(inputFields[2].text);

        savedLocations.Add(location);
        SaveLocations();
        UpdateAddButtonState();
    }

    public void SelectLocation(SavedLocations location)
    {
        inputLatitude.text = location.latitude.ToString();
        inputLongitude.text = location.longitude.ToString();
    }

    public void DeleteLocation(SavedLocations location, GameObject locationUI)
    {
        savedLocations.Remove(location);
        Destroy(locationUI);
        SaveLocations();
        UpdateAddButtonState();
    }

    private void LoadLocations()
    {
        savedLocations.Clear();
        for (int i = 0; i < MaxLocations; i++)
        {
            string locationKey = $"Location_{i}";
            if (PlayerPrefs.HasKey(locationKey))
            {
                string json = PlayerPrefs.GetString(locationKey);
                SavedLocations loadedLocation = JsonUtility.FromJson<SavedLocations>(json);
                savedLocations.Add(loadedLocation);
                CreateLocationUI(loadedLocation);
            }
        }
    }

    private void SaveLocations()
    {
        for (int i = 0; i < MaxLocations; i++)
        {
            PlayerPrefs.DeleteKey($"Location_{i}");
        }

        for (int i = 0; i < savedLocations.Count; i++)
        {
            string json = JsonUtility.ToJson(savedLocations[i]);
            PlayerPrefs.SetString($"Location_{i}", json);
        }
        PlayerPrefs.Save();
    }

    public void CreateLocationUI()
    {
        if (locationList.childCount >= MaxLocations)
        {
            Debug.Log("Максимальна кількість локацій досягнута");
            return;
        }
        GameObject locationUI = Instantiate(locationPrefab, locationList);
        SavedLocations newLocation = new SavedLocations();
        Button[] buttons = locationUI.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => SelectLocation(newLocation));
        buttons[1].onClick.AddListener(() => DeleteLocation(newLocation, locationUI));
        buttons[2].onClick.AddListener(() => AddLocation(newLocation, locationUI));
    }

     private void CreateLocationUI(SavedLocations location)
    {
        if (locationList.childCount >= MaxLocations)
        {
            Debug.Log("Максимальна кількість локацій досягнута");
            return;
        }

        GameObject locationUI = Instantiate(locationPrefab, locationList);

        TMP_InputField[] inputFields = locationUI.GetComponentsInChildren<TMP_InputField>();
        inputFields[0].text = location.locationName;
        inputFields[1].text = location.latitude.ToString();
        inputFields[2].text = location.longitude.ToString();

        Button[] buttons = locationUI.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => SelectLocation(location));
        buttons[1].onClick.AddListener(() => DeleteLocation(location, locationUI));
        buttons[2].onClick.AddListener(() => AddLocation(location, locationUI));

        location.locationUI = locationUI;
    }

    private void UpdateAddButtonState()
    {
        addLocationButton.interactable = locationList.childCount < MaxLocations;
    }
}
