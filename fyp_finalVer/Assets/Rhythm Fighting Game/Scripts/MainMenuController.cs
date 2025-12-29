using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject SettingsCanvas;
    public GameObject SongSelectCanvas;
    public GameObject MainTheme;

    private void Start()
    {
        MainTheme.GetComponent<AudioSource>().Play();
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("RhythmGame");
        MainMenuCanvas.SetActive(false);
    }

    public void GoToTrainingRoom()
    {
        SelectedSongController.Instance.getSelectedSongIndex(4);
    }

    public void GoToOption()
    {
        MainMenuCanvas.SetActive(false);
        SongSelectCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("quit");
        Application.Quit();  
    }
}