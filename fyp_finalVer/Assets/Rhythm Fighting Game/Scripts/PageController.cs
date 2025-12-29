using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{

    public int currentPage = 1;
    public static PageController Instance { get; set; }

    public GameObject[] PageArray;
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;
    public GameObject Page4;
    public GameObject Page5;
    public GameObject Page6;

    public int CurrentPage            //current page
    {
        get
        {
            return currentPage;
        }
        set
        {
            if (value <= 1)           
                currentPage = 1;
            else if (value >= 6)          
                currentPage = 6;           
            else
                currentPage = value;
        }
    }

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

    // Start is called before the first frame update
    void Start()
    {
        PageArray = new GameObject[6];
        PageArray[0] = Page1;
        PageArray[1] = Page2;
        PageArray[2] = Page3;
        PageArray[3] = Page4;
        PageArray[4] = Page5;
        PageArray[5] = Page6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
