using SonicBloom.Koreo.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControllerServer : MonoBehaviour
{
    int powerNumber1;
    int powerNumber2;
    int fastorSlow1;
    int fastorSlow2;
    int P1Skill1Time = 5;
    int P2Skill1Time = 5;
    int P1Skill3Time = 3;
    int P2Skill3Time = 3;
    public GameObject P1PaceJudgement;
    public GameObject P2PaceJudgement;
    public GameObject P1AttackJudgement;
    public GameObject P2AttackJudgement;
    public GameObject P1Skill;
    public GameObject P2Skill;

    public static SkillControllerServer Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        powerNumber1 = PowerController1P.Instance.PowerNumber;
        powerNumber2 = PowerController2P.Instance.PowerNumber;
    }

    public void checkSkillAvailable(int playerID, int skillLevel) {
        switch (playerID) {
            case 1:
                if (skillLevel > powerNumber1)
                {
                    Debug.Log("failed to use skill");
                }
                else 
                {
                    switch (skillLevel) {
                        case 1: Debug.Log("use skill1"); PowerController1P.Instance.UsePowerNumber(1); fastorSlow1 = Random.Range(1, 3); break;
                        case 2: Debug.Log("use skill2"); PowerController1P.Instance.UsePowerNumber(2); StartCoroutine(HealAndBan1()); break;
                        case 3: Debug.Log("use skill3"); PowerController1P.Instance.UsePowerNumber(3); repulseOpo1(); break;
                    }
                    if (fastorSlow1 == 1)
                    {
                        StartCoroutine(CountTime1(1)); //let pace slow down, attack speed up
                    }
                    else if (fastorSlow1 == 2)
                    {
                        StartCoroutine(CountTime1(2)); //let pace speed up, attack slow down
                    }
                }
                break;
            case 2:
                if (skillLevel > powerNumber2)
                {
                    Debug.Log("failed to use skill");
                }
                else
                {
                    switch (skillLevel)
                    {
                        case 1: Debug.Log("use skill1"); PowerController2P.Instance.UsePowerNumber(1); fastorSlow2 = Random.Range(1, 3); break;
                        case 2: Debug.Log("use skill2"); PowerController2P.Instance.UsePowerNumber(2); StartCoroutine(HealAndBan2()); break;
                        case 3: Debug.Log("use skill3"); PowerController2P.Instance.UsePowerNumber(3); repulseOpo2(); break;
                    }
                    if (fastorSlow2 == 1)
                    {
                        StartCoroutine(CountTime2(1)); //let pace slow down, attack speed up
                    }
                    else if (fastorSlow2 == 2)
                    {
                        StartCoroutine(CountTime2(2)); //let pace speed up, attack slow down
                    }
                }
                break;
        }
    }

    private IEnumerator CountTime1(int situation)
    {
        switch (situation)
        {
            case 1:
                RhythmGameController2P.Instance.noteSpeed = 8f;
                PaceController2P.Instance.noteSpeed = 4f;
                while (P1Skill1Time > 0)
                {
                    yield return new WaitForSeconds(1);
                    P1Skill1Time--;
                }
                RhythmGameController2P.Instance.noteSpeed = 6f;
                PaceController2P.Instance.noteSpeed = 6f;
                P1Skill1Time = 5;
                fastorSlow1 = 0;
                break;
            case 2:
                RhythmGameController2P.Instance.noteSpeed = 4f;
                PaceController2P.Instance.noteSpeed = 8f;
                while (P1Skill1Time > 0)
                {
                    yield return new WaitForSeconds(1);
                    P1Skill1Time--;
                }
                RhythmGameController2P.Instance.noteSpeed = 6f;
                PaceController2P.Instance.noteSpeed = 6f;
                P1Skill1Time = 5;
                fastorSlow1 = 0;
                break;
        }
    }

    private IEnumerator CountTime2(int situation)
    {
        switch (situation)
        {
            case 1:
                RhythmGameController1P.Instance.noteSpeed = 8f;
                PaceController1P.Instance.noteSpeed = 4f;
                while (P2Skill1Time > 0)
                {
                    yield return new WaitForSeconds(1);
                    P2Skill1Time--;
                }
                RhythmGameController1P.Instance.noteSpeed = 6f;
                PaceController1P.Instance.noteSpeed = 6f;
                P2Skill1Time = 5;
                fastorSlow2 = 0;
                break;
            case 2:
                RhythmGameController1P.Instance.noteSpeed = 4f;
                PaceController1P.Instance.noteSpeed = 8f;
                while (P2Skill1Time > 0)
                {
                    yield return new WaitForSeconds(1);
                    P2Skill1Time--;
                }
                RhythmGameController1P.Instance.noteSpeed = 6f;
                PaceController1P.Instance.noteSpeed = 6f;
                P2Skill1Time = 5;
                fastorSlow2 = 0;
                break;
        }
    }

    private IEnumerator HealAndBan1()
    {
        HPController1P.Instance.TakeHeal();
        HPController2P.Instance.TakeDamage(2);

        P2PaceJudgement.SetActive(false);
        P2AttackJudgement.SetActive(false);
        P2Skill.SetActive(false);
        while (P1Skill3Time > 0)
        {
            yield return new WaitForSeconds(1);
            P1Skill3Time--;
        }
        P2PaceJudgement.SetActive(true);
        P2AttackJudgement.SetActive(true);
        P2Skill.SetActive(true);
        P1Skill3Time = 3;
    }

    private IEnumerator HealAndBan2()
    {
        HPController2P.Instance.TakeHeal();
        HPController1P.Instance.TakeDamage(2);
        P1PaceJudgement.SetActive(false);
        P1AttackJudgement.SetActive(false);
        P1Skill.SetActive(false);
        while (P2Skill3Time > 0)
        {
            yield return new WaitForSeconds(1);
            P2Skill3Time--;
        }
        P1PaceJudgement.SetActive(true);
        P1AttackJudgement.SetActive(true);
        P1Skill.SetActive(true);
        P2Skill3Time = 3;
    }

    private void repulseOpo1() 
    {
        P2Movement.Instance.Anime2P.SetTrigger("LARGERBLOCK");
        StartCoroutine(P2Movement.Instance.RepulsedWithAnimation(0.15f)); // 0.03f when released
}

    private void repulseOpo2()
    {
        P1Movement.Instance.Anime1P.SetTrigger("LARGERBLOCK");
        StartCoroutine(P1Movement.Instance.RepulsedWithAnimation(0.15f));
    }

}
