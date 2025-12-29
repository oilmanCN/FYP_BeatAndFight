using UnityEngine;
using UnityEngine.UI;

public class PowerController1P : MonoBehaviour
{
    public Image Power;     //Power Image
    public GameObject FullPower;      //Full Power Image
    public Text Power_Text;        //Power Text
    int powerNumber = 0;          //power
    public int MaxPower = 10;       //max power

    [SerializeField]
    private int currentPower = 0;

    public int CurrentPower           //current power
    {
        get
        {
            return currentPower;
        }
        set
        {
            if (powerNumber < 3) {
                if (value == MaxPower)
                {
                    currentPower = 0;
                    powerNumber++;
                }
                else if (value > MaxPower)
                {
                    if (powerNumber < 2)
                    {
                        currentPower = value - MaxPower;
                    }
                    else {
                        currentPower = 0;
                    }
                    powerNumber++;
                }
                else
                {
                    currentPower = value;
                }
            }            
        }
    }

    public int PowerNumber 
    {
        get 
        {
            return powerNumber; 
        }
        set 
        {
            if (powerNumber > 0)
            {
                powerNumber = value;
            }
            else 
            {
                powerNumber = 0;
            }
        }
    }

    public static PowerController1P Instance { get; private set; }

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


    private void Start()
    {
        Power.fillAmount = 0;
        Power_Text.text = powerNumber.ToString();
    }

    private void Update()
    {
        if (powerNumber == 3)
        {
            FullPower.SetActive(true);
        }
        else { 
            FullPower.SetActive(false);
        }
        Power_Text.text = powerNumber.ToString();
    }

    //add power when attack
    public void GetPower(int power)
    {
        CurrentPower = CurrentPower + power;
        Power.fillAmount = (float)CurrentPower / MaxPower;        //add power
    }

    public void UsePowerNumber(int usePowerNumber)
    {
        powerNumber = powerNumber - usePowerNumber;
    }
}
