using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*public class P1Movement : MonoBehaviour
{
    public float moveSpeed = 3f;

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(0f, 0f,moveInput);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}*/
public class P1Movement : MonoBehaviour
{
    public KeyCode keyboardButtonPositive;
    public KeyCode keyboardButtonNegative;
    public float speed = 20f;
    private Rigidbody rb;
    private float moveAmount = 0.028f; //0.005f when released
    private float distance = 10f;
    private float moveScale;
    private string currentSceneName;

    public bool canMove = true;
    private bool trainingCanMove = true;
    private Vector3 movement;

    //public GameOverManager over;

    public Animator Anime1P;

    public static P1Movement Instance { get; set; }

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
        Anime1P = GetComponentInChildren<Animator>();
        currentSceneName = SceneManager.GetActiveScene().name;
        //over = FindObjectOfType<GameOverManager>();
    }

    /*
public void AvatarMove(KeyCode button)
{
    if (button == keyboardButtonPositive)
    {
        Anime1P.SetTrigger("FORWARD");
        //movement = new Vector3(0f, 0f, 1f);

        movement = new Vector3(0, 0, 1f) * speed;
        rb.velocity = movement;

        //transform.Translate(movement * speed * Time.deltaTime);
        //MoveWithCollisionCheck(movement);
    }
    if (button == keyboardButtonNegative)
    {
        Anime1P.SetTrigger("BACKWARD");
        movement = new Vector3(0f, 0f, -1f);
        //MoveWithCollisionCheck(movement);
        if (transform.position.x > 11)
        {
            print("1P fall off!");
            GameOverManager.Instance.KOGameOver();
        }
    }
}

private void MoveWithCollisionCheck(Vector3 moveDirection)
{
    Vector3 newPosition = transform.position + moveDirection;

    // 检查将要移动到的位置是否有碰撞
    if (!HasCollision(newPosition))
    {
        transform.position = newPosition;
    }
    else
    {
        // 处理碰撞逻辑
        print("1P collision");
    }
}

private bool HasCollision(Vector3 position)
{
    // 在这里实现检查碰撞的逻辑，可以使用Physics.Raycast或其他方法
    // 如果有碰撞，返回true；否则，返回false
    RaycastHit hit;
    return Physics.Raycast(position, Vector3.right, out hit, 0.1f);
}
*/
    private void Update()
    {
        distance = P2Movement.Instance.transform.position.x - transform.position.x;
        if (currentSceneName == "TrainingRoom")
        {

            if (transform.position.x <= 1269f)
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
            if (button == keyboardButtonPositive){
                //movement = new Vector3(0f, 0f, 1f).normalized;
                //transform.Translate(movement * speed * Time.deltaTime);
                Anime1P.SetTrigger("FORWARD");
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
            if (trainingCanMove == true) {
                if (button == keyboardButtonNegative)
                {
                    //movement = new Vector3(0f, 0f, -1f).normalized;
                    //transform.Translate(movement * speed * Time.deltaTime);
                    Anime1P.SetTrigger("BACKWARD");
                    StartCoroutine(MoveWithAnimation(Vector3.back * moveAmount));
                }
            }
            //Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
            //rb.MovePosition(transform.position + movement);
            
        }
        else if (canMove == false) {
            if (button == keyboardButtonNegative)
            {
                //movement = new Vector3(0f, 0f, -1f).normalized;
                //transform.Translate(movement * speed * Time.deltaTime);
                Anime1P.SetTrigger("BACKWARD");
                StartCoroutine(MoveWithAnimation(Vector3.back * moveAmount));
            }        
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("1P collision");
            canMove = false;
            movement = Vector3.zero;
        }
        
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("1P collision exit");
            canMove = true;
        }

    }
    private IEnumerator MoveWithAnimation(Vector3 moveDirection)
    {
        /*
        float animationLength = 0.1f; // 获取动画长度
        float elapsedTime = 0f;

        while (elapsedTime < animationLength)
        {
            transform.Translate(moveDirection);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        */
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
        /*
        float animationLength = 0.1f; // 获取动画长度
        float elapsedTime = 0f;

        while (elapsedTime < animationLength)
        {
            transform.Translate(Vector3.back * repellAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        */
        float animationLength = 25f; // 获取动画长度

        while (animationLength > 0)
        {
            transform.Translate(Vector3.back * repellAmount);
            yield return new WaitForSeconds(0.01f);
            animationLength--;
        }
    }
}


