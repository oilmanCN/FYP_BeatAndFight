using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class P2Movement : MonoBehaviour
{
    public KeyCode keyboardButtonPositive;
    public KeyCode keyboardButtonNegative;
    public float speed = 20f;
    private Rigidbody rb;
    private float moveAmount = 0.028f; //0.005f when released
    private float distance = 10f;
    private float moveScale;
    private string currentSceneName;

    private bool canMove = true;
    private bool trainingCanMove = true;
    private Vector3 movement;

    public Animator Anime2P;

    public static P2Movement Instance { get; private set; }

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
        rb = GetComponent<Rigidbody>();
        Anime2P = GetComponentInChildren<Animator>();
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        distance = transform.position.x - P1Movement.Instance.transform.position.x;
        if (currentSceneName == "TrainingRoom")
        {
            if (transform.position.x >= 1291f)
            {
                trainingCanMove = false;
            }
            else
            {
                trainingCanMove = true;
            }
        }
    }

    public void AvatarMove(KeyCode button)
    {
        if (canMove)
        {
            if (button == keyboardButtonPositive)
            {
                //movement = new Vector3(0f, 1f, 0f).normalized;
                //transform.Translate(movement * speed * Time.deltaTime);
                Anime2P.SetTrigger("FORWARD");
                //movement = new Vector3(0f, 0f, 0.5f);
                //transform.Translate(movement);
                if (distance > 1.8f)
                {
                    StartCoroutine(MoveWithAnimation(Vector3.forward * moveAmount));
                }
                else if (distance > 1.2f && distance <= 1.8f)
                {
                    moveScale = (float)((distance - 1.2) / 2);
                    StartCoroutine(MoveWithAnimation(Vector3.forward * moveAmount * moveScale));
                }
                else
                {
                    canMove = false;
                }
            }
            if (trainingCanMove == true)
            {
                if (button == keyboardButtonNegative)
                {
                    //movement = new Vector3(0f, 0f, -1f).normalized;
                    //transform.Translate(movement * speed * Time.deltaTime);
                    Anime2P.SetTrigger("BACKWARD");
                    StartCoroutine(MoveWithAnimation(Vector3.back * moveAmount));
                }
            }

        }
        else if (canMove == false)
        {
            if (button == keyboardButtonNegative)
            {
                //movement = new Vector3(0f, -1f, 0f).normalized;
                //transform.Translate(movement * speed * Time.deltaTime);
                Anime2P.SetTrigger("BACKWARD");
                StartCoroutine(MoveWithAnimation(Vector3.back * moveAmount));
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("2P collision");
            canMove = false;
            movement = Vector3.zero;
        }

    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("2P collision exit");
            canMove = true;
        }

    }

    private IEnumerator MoveWithAnimation(Vector3 moveDirection)
    {
        float animationLength = 25f; // 获取动画长度

        while (animationLength > 0)
        {
            transform.Translate(moveDirection);
            yield return new WaitForSeconds(0.01f);
            animationLength--;
        }
    }

    public IEnumerator RepulsedWithAnimation(float repellAmount)
    {
        float animationLength = 25f; // 获取动画长度

        while (animationLength > 0)
        {
            transform.Translate(Vector3.back * repellAmount);
            yield return new WaitForSeconds(0.01f);
            animationLength--;
        }
    }
}


