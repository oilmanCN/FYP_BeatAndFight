using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{

    private Light pointLight; 
    private AudioSource Audio;
    public float timeDelay;

    public static OptionController Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // destroy duplicate instance
        }
    }

    void Start()
    {
        pointLight = FindObjectOfType<Light>(); 

        Audio = FindObjectOfType<AudioSource>();

      
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f);
        pointLight.intensity = savedBrightness;

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        Audio.volume = savedVolume;

        timeDelay = PlayerPrefs.GetFloat("Delay", 0f);
    }
}

