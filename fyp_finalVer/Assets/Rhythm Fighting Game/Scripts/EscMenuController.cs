using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuController : MonoBehaviour
{
    public KeyCode keyboardEsc;
    public GameObject EscMenu;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if(Input.GetKeyDown(keyboardEsc))
        {
            if (EscMenu.active == false)
            {
                EscMenu.SetActive(true);
                music.mute = true;
                Cursor.visible = true;
            }
            else
            { 
                EscMenu.SetActive(false);
                music.mute = false;
                Cursor.visible = false;
            }  
        }
    }
}
