/*
using SonicBloom.Koreo.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController1P : MonoBehaviour
{
    public Transform HealthBarTransform;
    public Transform HealthBarBGTransform;
    public int MaxHealth = 100;
    public float ScaleSpeed = 1f;

    private bool isScaling = false;

    private int currentHealth = 100;
    public static HPController1P Instance { get; private set; }

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

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
            UpdateScale();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (isScaling)
        {
            ScaleValue(HealthBarBGTransform, HealthBarTransform.localScale.x);
        }
    }

    private void UpdateScale()
    {
        float healthScale = (float)CurrentHealth / MaxHealth;
        HealthBarTransform.localScale = new Vector3(healthScale * 6f, 0.29f, 1f);
        isScaling = true;  // on scaling
    }

    private void ScaleValue(Transform transform, float endValue)
    {
        float currentScale = transform.localScale.x;
        float newScale = Mathf.MoveTowards(currentScale, endValue, ScaleSpeed * Time.deltaTime);

        transform.localScale = new Vector3(newScale, 0.29f, 1f);

        if (Mathf.Approximately(newScale, endValue))
        {
            isScaling = false;  // off scaling
        }
    }

    // calculate health when damaged
    public void TakeDamage(int Damage)
    {
        print(Damage);
        CurrentHealth -= Damage;
    }

}
*/

using TMPro;
using UnityEngine;
using SonicBloom.Koreo.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HPController1P : MonoBehaviour
{
    public Image HealthBar;     //血条Image
    public Image HealthBar_Damage;      //伤害条Image
    public Image HealthBar_Heal;       //恢复条Image
    public Text Health_Text;        //HEealth文本
    int Damage;          //伤害
    int Heal = 10;            //治疗
    public int MaxHealth = 60;       //最大生命
    float FadeTime = 0.5f;      //渐变时间
    private bool startDamage = false;
    private bool startHeal = false;
    private float temp;

    [SerializeField]
    private int currentHealth = 60;
    public Text kotext;
    public int CurrentHealth            //当前血量
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value < 0)
            {
                currentHealth = 0;

            }
            else if (value > MaxHealth)
            {
                currentHealth = MaxHealth;
            }
            else
            {
                currentHealth = value;
            }
            Health_Text.text = "HP " + CurrentHealth + "/" + MaxHealth;
        }
    }

    public static HPController1P Instance { get; private set; }

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
        HealthBar.fillAmount = 1;
        HealthBar_Damage.fillAmount = HealthBar.fillAmount;
        HealthBar_Heal.fillAmount = HealthBar.fillAmount;
        Health_Text.text = "HP " + CurrentHealth + "/" + MaxHealth;
    }

    private void Update()
    {
        FadeValue(HealthBar.fillAmount, FadeTime);
        FadeHeal(HealthBar_Heal.fillAmount, FadeTime);
    }

    //伤害条缓变
    public void FadeValue(float endValue, float duration)
    {
        if (startDamage)
        {
            HealthBar_Damage.fillAmount -= (temp / duration) * Time.deltaTime;    //temp/duration使用固定渐变的时间。
            if (HealthBar_Damage.fillAmount <= endValue)        //到达设定值，关闭渐变。
            {
                startDamage = false;
            }
        }
    }
    //血条条缓变
    public void FadeHeal(float endValue, float duration)
    {
        if (startHeal)
        {
            HealthBar.fillAmount += (temp / duration) * Time.deltaTime;    //temp/duration使用固定渐变的时间。
            if (HealthBar.fillAmount >= endValue)        //到达设定值，关闭渐变。
            {
                startHeal = false;
            }
        }
    }
    //受到治疗，先立刻加实际恢复条和伤害条，再缓加血条
    public void TakeHeal()
    {
        CurrentHealth = CurrentHealth + Heal;
        HealthBar_Heal.fillAmount = (float)CurrentHealth / MaxHealth;        //立刻加恢复条
        HealthBar_Damage.fillAmount = HealthBar_Heal.fillAmount;            //立刻加伤害条
        temp = HealthBar_Heal.fillAmount - HealthBar.fillAmount;
        startHeal = true;          //开启治疗渐变
    }

    //受到伤害，先立刻减实际血条和恢复条，再缓减伤害条
    public void TakeDamage(int Damage)
    {
        CurrentHealth = CurrentHealth - Damage;
        HealthBar.fillAmount = (float)CurrentHealth / MaxHealth;        //减少血条
        HealthBar_Heal.fillAmount = HealthBar.fillAmount;            //减少恢复条
        temp = HealthBar_Damage.fillAmount - HealthBar.fillAmount;
        startDamage = true;       //开启伤害渐变
    }
}
