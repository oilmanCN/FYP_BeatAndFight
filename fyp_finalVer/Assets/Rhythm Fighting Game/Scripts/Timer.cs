using SonicBloom.Koreo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimeText;
    private int songIndex;
    Koreography playingKoreo;
    int timer;
    int timerDelay;

    // Start is called before the first frame update
    void Start()
    {
        songIndex = SelectedSongController.Instance.SongSelectedIndex;
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(songIndex);
        timer = (int)Math.Round(playingKoreo.SourceClip.length);
        timerDelay = timer + 1;
        StartCoroutine(ChangeTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ChangeTime()
    {
        while (timerDelay > 0)
        {
            yield return new WaitForSeconds(1);
            timerDelay--;
            timer--;
            if (timer < 0) {
                timer = 0;
            }
            TimeText.text = timer.ToString();
        }
        GameOverManager.Instance.TimeoutGameOver();
    }
}