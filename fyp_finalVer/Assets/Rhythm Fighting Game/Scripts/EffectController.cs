using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
,
    IPointerEnterHandler, IPointerExitHandler
{

    public AudioSource audioSource;

    void Start()
    {

    }

    public void playAudio()
    {
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        playAudio();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
