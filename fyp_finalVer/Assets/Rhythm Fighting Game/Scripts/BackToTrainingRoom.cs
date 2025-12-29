using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTrainingRoom : MonoBehaviour
{
    public GameObject TutorialCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToTrainingRoom() 
    {
        PageController.Instance.CurrentPage = 1;
        TutorialCanvas.SetActive(false);
    }
}
