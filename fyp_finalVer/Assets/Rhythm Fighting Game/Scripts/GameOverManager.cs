using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverManager : MonoBehaviour
{
    public GameObject KOText; // refer to the game over scene
    public GameObject TimeoutText; // refer to the game over scene
    public GameObject GameOverImage;
    public AudioSource music;
    public GameObject ExitText;
    public GameObject timer;
    public GameObject GameCanvas;
    public GameObject timeOutCanvas;
    public GameObject fallOutCanvas;
    public GameObject KOCanvas;
    public VideoPlayer timeOutVideoPlayer;
    public VideoPlayer fallOutVideoPlayer;
    public VideoPlayer KOVideoPlayer;
    public KeyCode keyboardEsc;
    private bool videoEnded = false;
    private float flickTimer = 1;

    public static GameOverManager Instance { get; private set; }


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
        // Subscribe to the loopPointReached event
        timeOutVideoPlayer.loopPointReached += OnTimeOutVideoEnd;
        fallOutVideoPlayer.loopPointReached += OnFallOutVideoEnd;
        KOVideoPlayer.loopPointReached += OnKOVideoEnd;
    }

    void Update()
    {
        if (P1Movement.Instance.transform.position.x < 1268.5f)
        {
            KOText.GetComponent<Text>().text = "1P fall off!\n2P wins!!";
            P1Movement.Instance.transform.Translate(Vector3.forward * 10f);
            FallOutGameOver();
        }
        if (P2Movement.Instance.transform.position.x > 1291.5f)
        {
            KOText.GetComponent<Text>().text = "2P fall off!\n1P wins!!";
            P2Movement.Instance.transform.Translate(Vector3.forward * 10f);
            FallOutGameOver();
        }

        if (HPController1P.Instance.CurrentHealth == 0)
        {
            KOText.GetComponent<Text>().text = "K.O.\n2P wins!!";
            HPController1P.Instance.CurrentHealth = 1;
            KOGameOver();
        }
        if (HPController2P.Instance.CurrentHealth == 0)
        {
            KOText.GetComponent<Text>().text = "K.O.\n1P wins!!";
            HPController2P.Instance.CurrentHealth = 1;
            KOGameOver();
        }

        if (videoEnded)
        {
            if (Input.GetKeyDown(keyboardEsc))
            {
                videoEnded = false;
                SceneManager.LoadScene("Start Menu");
            }
        }
    }

    public void TimeoutGameOver()
    {
        if (HPController1P.Instance.CurrentHealth > HPController2P.Instance.CurrentHealth)
        {
            TimeoutText.GetComponent<Text>().text = "Time Out\n1P Win!";
        }
        else if (HPController2P.Instance.CurrentHealth > HPController1P.Instance.CurrentHealth)
        {
            TimeoutText.GetComponent<Text>().text = "Time Out\n2P Win!";
        }
        else {
            TimeoutText.GetComponent<Text>().text = "Time Out\nDraw Game!";
        }
        //Time.timeScale = 0;
        music.Stop();
        GameCanvas.SetActive(false);
        timeOutCanvas.SetActive(true);
        Cursor.visible = true;
    }


    public void FallOutGameOver()
    {
        //Time.timeScale = 0;
        music.mute = true;
        timer.SetActive(false);
        GameCanvas.SetActive(false);
        fallOutCanvas.SetActive(true);
        Cursor.visible = true;
    }

    public void KOGameOver() 
    {
        music.mute = true;
        timer.SetActive(false);
        GameCanvas.SetActive(false);
        KOCanvas.SetActive(true);
        Cursor.visible = true;
    }

    void OnTimeOutVideoEnd(VideoPlayer vp)
    {
        timeOutCanvas.SetActive(false);
        GameOverImage.SetActive(true);
        TimeoutText.SetActive(true);      // Activate TimeoutText and ExitButton after the video ends
        StartCoroutine(flickText());

        videoEnded = true;
    }

    void OnFallOutVideoEnd(VideoPlayer vp)
    {
        fallOutCanvas.SetActive(false);
        GameOverImage.SetActive(true);
        KOText.SetActive(true);
        StartCoroutine(flickText());

        videoEnded = true;
    }

    void OnKOVideoEnd(VideoPlayer vp)
    {
        KOCanvas.SetActive(false);
        GameOverImage.SetActive(true);    
        KOText.SetActive(true);
        StartCoroutine(flickText());

        videoEnded = true;
    }

    private IEnumerator flickText()
    {
        ExitText.SetActive(true);
        while (flickTimer > 0)
        {
            yield return new WaitForSeconds(0.5f);
            flickTimer--;
        }
        flickTimer = 1;

        ExitText.SetActive(false);

        while (flickTimer > 0)
        {
            yield return new WaitForSeconds(0.5f);
            flickTimer--;
        }
        flickTimer = 1;

        yield return null;
        StartCoroutine(flickText());
    }
}
