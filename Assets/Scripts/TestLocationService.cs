using System.Collections;
using UnityEngine;

public class TestLocationService : MonoBehaviour
{
    private float latitude;
    private float longitude;
    public float Latitude {
        get => latitude;
    }

    public float Longitude {
        get => longitude;
    }
     IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        Debug.Log(Input.location.status);
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
        }
}
}
