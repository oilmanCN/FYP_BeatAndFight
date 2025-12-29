using SonicBloom.Koreo.Demos;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider brightnessSlider;
    public Slider volumeSlider;
    public GameObject MainMenuCanvas;
    public GameObject SettingsCanvas;
    public GameObject SongSelectCanvas;

    private void Start()
    {
        // load settings from saved prefabs
        //brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
        //volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        brightnessSlider.value = 2f;
        volumeSlider.value = 1f;
    }

    public void AdjustBrightness()
    {
        float brightnessValue = brightnessSlider.value;

        RenderSettings.ambientLight = new Color(brightnessValue, brightnessValue, brightnessValue);

        PlayerPrefs.SetFloat("Brightness", brightnessValue);
    }

    public void AdjustVolume()
    {
        float volumeValue = volumeSlider.value;

        AudioListener.volume = volumeValue;

        PlayerPrefs.SetFloat("Volume", volumeValue);
    }

    public void BackToMainMenu()
    {
        MainMenuCanvas.SetActive(true);
        SongSelectCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    }
}
