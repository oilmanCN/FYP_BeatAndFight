using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousPage : MonoBehaviour
{
    private int currentPage;
    int currentPageIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void previousPage()
    {
        currentPage = PageController.Instance.CurrentPage;
        currentPageIndex = currentPage - 1;
        //Debug.Log(currentPage + " <- current  current-1 -> "+ currentPageIndex);
        PageController.Instance.PageArray[currentPageIndex].SetActive(false); // current   
        currentPageIndex -= 1;
        if (currentPageIndex <= 0)
            currentPageIndex = 0;
        PageController.Instance.PageArray[currentPageIndex].SetActive(true);
        currentPage -= 1;
        PageController.Instance.CurrentPage = currentPage;
        

    }

}
