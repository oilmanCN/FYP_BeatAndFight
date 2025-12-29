using UnityEngine;
using UnityEngine.UI;

public class DelayController : MonoBehaviour
{
    public InputField delayInputField;
    public Button increaseButton;
    public Button decreaseButton;

    public float delayValue = 0.0f;
    private float step = 0.1f;

    public static DelayController Instance { get; set; }

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
        delayValue = PlayerPrefs.GetFloat("Delay", 0f);
        delayInputField.text = delayValue.ToString();
        increaseButton.onClick.AddListener(IncreaseDelay);
        decreaseButton.onClick.AddListener(DecreaseDelay);
    }

    void Update()
    {
        
    }

    void IncreaseDelay()
    {
        delayValue += step;
        UpdateInputField();
    }

    void DecreaseDelay()
    {
        delayValue -= step;
        UpdateInputField();
    }

    void UpdateInputField()
    {

        delayValue = Mathf.Round(delayValue * 10.0f) / 10.0f;
        delayInputField.text = delayValue.ToString();
        PlayerPrefs.SetFloat("Delay", delayValue);
    }

    public float GetDelayValue()
    {
        return delayValue;
    }
}
