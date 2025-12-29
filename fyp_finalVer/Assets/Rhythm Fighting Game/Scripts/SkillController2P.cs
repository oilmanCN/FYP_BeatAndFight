using SonicBloom.Koreo.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController2P : MonoBehaviour
{

    public KeyCode keyboardButtonSkill1;
    public KeyCode keyboardButtonSkill2;
    public KeyCode keyboardButtonSkill3;

    public static SkillController2P Instance { get; private set; }
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
        if (Input.GetKeyDown(keyboardButtonSkill1))
        {
            SkillControllerServer.Instance.checkSkillAvailable(2, 1);
        }
        else if (Input.GetKeyDown(keyboardButtonSkill2))
        {
            SkillControllerServer.Instance.checkSkillAvailable(2, 2);
        }
        else if (Input.GetKeyDown(keyboardButtonSkill3))
        {
            SkillControllerServer.Instance.checkSkillAvailable(2, 3);
        }
    }
}
