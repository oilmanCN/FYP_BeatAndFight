using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateController : MonoBehaviour
{

    public GameObject beginText;
    public GameObject outRangeText;
    public GameObject inRangeText;
    private int hideTimer = 1;
    private int flickTimer = 1;
    private string currentSceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "TrainingRoom")
        {
            beginText.SetActive(false);
        }
        StartCoroutine(hideBeginText());
        StartCoroutine(flickText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator hideBeginText()
    {
        while (hideTimer > 0)
        {
            yield return new WaitForSeconds(1);
            hideTimer--;
        }
        beginText.SetActive(false);
    }

    private IEnumerator flickText()
    {
        if (P2Movement.Instance.transform.position.x - P1Movement.Instance.transform.position.x <= 1.5f)
        {
            inRangeText.SetActive(true);
        }
        else
        {
            outRangeText.SetActive(true);
        }

        while (flickTimer > 0)
        {
            yield return new WaitForSeconds(0.5f);
            flickTimer--;
        }
        flickTimer = 1;

        inRangeText.SetActive(false);
        outRangeText.SetActive(false);

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

