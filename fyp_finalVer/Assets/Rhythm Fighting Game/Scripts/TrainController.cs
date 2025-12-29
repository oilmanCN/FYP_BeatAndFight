using SonicBloom.Koreo;
using SonicBloom.Koreo.Demos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainController : MonoBehaviour
{
    private int songIndex;
    Koreography playingKoreo;
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        songIndex = 0;
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(songIndex);
        timer = (int)Math.Round(playingKoreo.SourceClip.length);
        //Debug.Log(playingKoreo + " & " + timer);
        StartCoroutine(ChangeTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ChangeTime()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            if (timer < 0)
            {
                timer = 0;
            }
        }
        RhythmGameController1P.Instance.Restart();
        RhythmGameController2P.Instance.Restart();
        PaceController1P.Instance.Restart();
        PaceController2P.Instance.Restart();
        timer = (int)Math.Round(playingKoreo.SourceClip.length);
        StartCoroutine(ChangeTime());

    }
}