using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{

    public float velocity = 1;
    private Rigidbody rigidbody;

    private AudioSource audio;

    private float velocityF = 0;

    float smoothedRotation = 0;

    public Vector3 gravity;

    public float rotationZ;
    public float rotationZLerpTime;

    public GameObject circleParticle;

    public GameManager gameManager;

    public float scaleFactor;

    float vel;

    Cylinder cylinder;
    float nextBallPosToJump;
    public int skippedCounter = 0;

    float beginningY;

    void Start()
    {
        beginningY = transform.position.y;
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        Physics.gravity = gravity;


        cylinder = FindObjectOfType<Cylinder>();

        nextBallPosToJump = cylinder.firstCirclePos-cylinder.circlesHeight;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameStarted)
        {
            if (GameManager.isPlayerAlive == true)
            {
                if(transform.position.y < nextBallPosToJump)
                {
                    nextBallPosToJump -= cylinder.distanceBtwCircles + cylinder.circlesHeight;
                    skippedCounter++;
                    Debug.Log(skippedCounter);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    //rb2D.AddForce(Vector2.up * velocity * Time.deltaTime);

                    Jump();
                }
            }
            else
            {
                if (transform.localScale.x >= 0)
                {
                    transform.localScale -= Vector3.one * scaleFactor * Time.deltaTime;
                }
            }
        }
        else
        {
            if(transform.position.y <= beginningY)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        rigidbody.velocity = Vector2.up * velocity;

        PlayerPrefs.GetInt("jumpCount", PlayerPrefs.GetInt("jumpCount") + 1);

        if (audio != null)
        {
            audio.Play();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.isGameStarted)
        {
            if (GameManager.isPlayerAlive == true)
            {
                if (rigidbody.velocity.y > 0)
                {
                    smoothedRotation = Mathf.Lerp(smoothedRotation, rotationZ, rotationZLerpTime);
                    this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(smoothedRotation, -30, 30));
                }
                else
                {
                    smoothedRotation = Mathf.Lerp(smoothedRotation, -rotationZ, rotationZLerpTime / 2);
                    this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(smoothedRotation, -30, 30));
                }
            }
        }
        if (rigidbody.velocity.y > 0)
        {
            smoothedRotation = Mathf.Lerp(smoothedRotation, rotationZ, rotationZLerpTime);
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(smoothedRotation, -30, 30));
        }
        else
        {
            smoothedRotation = Mathf.Lerp(smoothedRotation, -rotationZ, rotationZLerpTime / 2);
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(smoothedRotation, -30, 30));
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Score")
        {
            //GameManager.score += 1;
            collision.gameObject.transform.parent.GetChild(0).GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Circle")
        {
            /*if (GameManager.score > PlayerPrefs.GetInt("highscore", 0))
            {
                PlayerPrefs.SetInt("highscore", GameManager.score);
            }*/
            //StartCoroutine(gameManager.EndGame());

            if(transform.position.y < collision.transform.position.y && rigidbody.velocity.y >= 0)
            {
                Debug.Log(transform.position.y);
                Debug.Log(collision.transform.position.y);
                Debug.Log("Semi-Death");
                GameManager.isPlayerAlive = false;
                rigidbody.velocity = Vector3.zero;
                PlayerPrefs.SetInt("deadCount", PlayerPrefs.GetInt("deadCount") + 1);
            }
            else
            {
                Debug.Log("Death");
                GameManager.isPlayerAlive = false;

                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;

                transform.parent = gameManager.GetComponent<GameManager>().cylinder.transform;
                GameObject.FindObjectOfType<Camera>().GetComponent<CameraScript>().enabled = false;
                transform.position = new Vector3(transform.position.x, collision.transform.position.y + 1, transform.position.z);

                gameManager.GetComponent<GameManager>().OpenThatPanel(2);
            }

            /*if (transform.position.y > collision.transform.position.y)
            {
                Debug.Log("Death");
                GameManager.isPlayerAlive = false;

                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;

                transform.parent = gameManager.GetComponent<GameManager>().cylinder.transform;
                GameObject.FindObjectOfType<Camera>().GetComponent<CameraScript>().enabled = false;
                transform.position = new Vector3(transform.position.x, collision.transform.position.y + 1, transform.position.z);

                gameManager.GetComponent<GameManager>().OpenThatPanel(2);
            }
            else
            {
                Debug.Log(transform.position.y);
                Debug.Log(collision.transform.position.y);
                Debug.Log("Semi-Death");
                GameManager.isPlayerAlive = false;
                rigidbody.velocity = Vector3.zero;
                PlayerPrefs.SetInt("deadCount", PlayerPrefs.GetInt("deadCount") + 1);
            }*/
        }

        if (collision.tag == "LastCircle")
        {
            GameManager.isPlayerAlive = false;

            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector3.zero;

            transform.parent = gameManager.GetComponent<GameManager>().cylinder.transform;
            GameObject.FindObjectOfType<Camera>().GetComponent<CameraScript>().enabled = false;
            transform.position = new Vector3(transform.position.x, collision.transform.position.y + 1, transform.position.z);

            gameManager.GetComponent<GameManager>().OpenThatPanel(1);
        }
    }
}
