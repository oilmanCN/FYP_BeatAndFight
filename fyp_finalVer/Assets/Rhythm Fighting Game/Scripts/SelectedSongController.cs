using SonicBloom.Koreo.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectedSongController : MonoBehaviour
{
    // Start is called before the first frame update
    public int SongSelectedIndex;
    public string buttonPace;
    public string buttonAttack;

    public static SelectedSongController Instance { get; set; }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getSelectedSongIndex(int buttonIndex) {
        SongSelectedIndex = buttonIndex;
        switch (SongSelectedIndex) {
            case 0: buttonPace = "KatyushaPace"; buttonAttack = "katyusha"; SceneManager.LoadScene("RhythmGame"); break;
            case 1: buttonPace = "RadetzkyPace"; buttonAttack = "Radetzky"; SceneManager.LoadScene("RhythmGame"); break; 
            case 2: buttonPace = "RiverPace"; buttonAttack = "Rivervalley"; SceneManager.LoadScene("RhythmGame"); break;
            case 3: buttonPace = "MikuPace";buttonAttack = "miku"; SceneManager.LoadScene("RhythmGame"); break;
            case 4: buttonPace = "TrainingPace"; buttonAttack = "Training"; SongSelectedIndex = 0; SceneManager.LoadScene("TrainingRoom"); break;
        }
        
    }

    public void RandomSong()
    {
        SongSelectedIndex = Random.Range(0,4);
        //Debug.Log(SongSelectedIndex);
        getSelectedSongIndex(SongSelectedIndex);
    }
}
