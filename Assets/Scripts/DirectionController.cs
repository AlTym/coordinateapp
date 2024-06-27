using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour
{
    [SerializeField] private TMP_InputField latitudeInput;
    [SerializeField] private TMP_InputField longitudeInput;
    [SerializeField] private Transform arrow;
    private TestLocationService locationService; 
    private Vector2 currentGPS;
    private Vector2 targetGPS;
    private float lat;
    private float lon;
    private float rotationSpeed = 3f;
    private Quaternion targetRotation;
    private Quaternion startRotation;

    private void Start()
    {
        locationService = FindObjectOfType<TestLocationService>();
        startRotation = arrow.rotation;
        targetRotation = arrow.rotation;
        latitudeInput.text = "48,670944";
        longitudeInput.text = "33,116822";
        Input.compass.enabled = true;
    }

    public void UpdateDirection()
    {
        currentGPS = new Vector2(lat, lon);
        float targetLat, targetLon;
        if (!float.TryParse(latitudeInput.text, out targetLat) || !float.TryParse(longitudeInput.text, out targetLon))
        {
            Debug.LogError("Invalid coordinates input.");
            return;
        }
        if (targetLat < -90 || targetLat > 90 || targetLon < -180 || targetLon > 180)
        {
            Debug.LogError("Invalid coordinates input.");
            return;
        }
        targetGPS = new Vector2(targetLat, targetLon);

        Vector2 direction = targetGPS - currentGPS;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float compassHeading = Input.compass.trueHeading;

        float finalAngle = angle - compassHeading;

        targetRotation = Quaternion.Euler(new Vector3(70, finalAngle, -90));
    }

    private void Update()
    {
        lat = locationService.Latitude;
        lon = locationService.Longitude;
        startRotation = arrow.rotation;
        arrow.rotation = Quaternion.Lerp(startRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    
}
