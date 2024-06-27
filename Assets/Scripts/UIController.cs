using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    public void ShowMenu()
    {
        mainCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void HideMenu()
    {
        mainCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }
    public void ShowSettings()
    {
        mainCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void HideSettings()
    {
        mainCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

}
