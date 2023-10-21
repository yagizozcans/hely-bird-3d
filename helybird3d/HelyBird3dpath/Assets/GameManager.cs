using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI nextLevel;

    public GameObject cylinder;
    public GameObject cylinderMesh;
    public GameObject[] meshes;

    public int howManyCircles;
    public int[] howManyCirclesInLevels;

    public GameObject fullCircle;

    [SerializeField] float darkeningAmount;
    GameObject player;

    public static bool isGameStarted = false;
    public static bool isPlayerAlive = true;

    Color randomColor;
    Color randomColor2;

    public GameObject[] panels;

    Camera camera;

    public Slider levelSlider;

    string gameId = "4073053";
    bool testMode = true;

    public string myPlacementID = "video";

    public CircleScriptableScript[] circles;

    public int[] levelRatios;
    public int[] circlesAtLevel;

    public bool isMuted = false;

    public Sprite muted;
    public Sprite unmuted;
    public Button muteButton;

    public GameObject upCircle;

    public GameObject adsButton;

    void Start()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        Advertisement.Initialize(gameId, testMode);
        if(PlayerPrefs.GetInt("RemoveAds",0) == 1 &&  adsButton != null)
        {
            Destroy(adsButton);
        }
        camera = FindObjectOfType<Camera>();
        isGameStarted = false;
        OpenThatPanel(0);
        level.text = PlayerPrefs.GetInt("level", 1).ToString();
        nextLevel.text = (PlayerPrefs.GetInt("level", 1) + 1).ToString();
        player = GameObject.FindGameObjectWithTag("Player");

        randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        randomColor2 = new Color(randomColor.r - darkeningAmount, randomColor.g - darkeningAmount, randomColor.b - darkeningAmount);

        Color randomColor3 = new Color(randomColor.r + (darkeningAmount * 4), randomColor.g + (darkeningAmount * 4), randomColor.b + (darkeningAmount * 4));
        camera.backgroundColor = randomColor3;
        player.GetComponent<MeshRenderer>().material.color = randomColor3;
        upCircle.GetComponent<MeshRenderer>().material.color = randomColor;
        /*for (int i = 0; i < howManyCircles; i++)
        {
            GameObject circle = Instantiate(meshes[Random.Range(0, meshes.Length - 1)], cylinder.transform.position, Quaternion.identity, cylinder.transform);
            circle.transform.localScale = new Vector3(350, 350, 550);
            var euler = transform.eulerAngles;
            euler.z = Random.Range(0.0f, 360.0f);
            circle.transform.eulerAngles = new Vector3(-90, 0, euler.z);
            circle.layer = 8;
            circle.tag = "Circle";
            if(circle.GetComponent<MeshRenderer>() != null)
            {
                circle.AddComponent<MeshCollider>();
                circle.GetComponent<MeshRenderer>().material.color = randomColor;
            }
        }*/

        for (int y = 0; y < levelRatios.Length; y++)
        {
            if (PlayerPrefs.GetInt("level", 1) > levelRatios[levelRatios.Length - 1])
            {
                howManyCircles = howManyCirclesInLevels[howManyCirclesInLevels.Length - 1];
                for (int i = 0; i < howManyCircles; i++)
                {
                    int randomCircle = Random.Range(0, circlesAtLevel[circlesAtLevel.Length - 1]);
                    GameObject parent = new GameObject();
                    parent.transform.position = cylinder.transform.position;
                    parent.transform.parent = cylinder.transform;
                    parent.tag = "Circle";
                    for (int z = 0; z < circles[randomCircle].meshes.Length; z++)
                    {
                        GameObject circle = Instantiate(circles[randomCircle].meshes[z], cylinder.transform.position, Quaternion.identity, parent.transform);
                        circle.transform.localScale = new Vector3(350, 350, 550);
                        circle.transform.localEulerAngles = new Vector3(-90, 0, circles[randomCircle].rotationZ[z]);
                        circle.layer = 8;
                        circle.tag = "Circle";
                        if (circle.GetComponent<MeshRenderer>() != null)
                        {
                            circle.AddComponent<MeshCollider>();
                            circle.GetComponent<MeshRenderer>().material.color = randomColor;
                        }
                        var euler2 = transform.eulerAngles;
                        euler2.y = Random.Range(0, 360f);
                        parent.transform.eulerAngles = euler2;
                    }
                }
                break;
            }
            else
            {
                if(y+1 == levelRatios.Length)
                {
                    howManyCircles = howManyCirclesInLevels[y - 1];
                    Debug.Log(howManyCircles);
                    for (int i = 0; i < howManyCircles; i++)
                    {
                        int randomCircle = Random.Range(0, circlesAtLevel[y - 1]);
                        GameObject parent = new GameObject();
                        parent.transform.position = cylinder.transform.position;
                        parent.transform.parent = cylinder.transform;
                        parent.tag = "Circle";
                        for (int z = 0; z < circles[randomCircle].meshes.Length; z++)
                        {
                            GameObject circle = Instantiate(circles[randomCircle].meshes[z], cylinder.transform.position, Quaternion.identity, parent.transform);
                            circle.transform.localScale = new Vector3(350, 350, 550);
                            circle.transform.localEulerAngles = new Vector3(-90, 0, circles[randomCircle].rotationZ[z]);
                            circle.layer = 8;
                            circle.tag = "Circle";
                            if (circle.GetComponent<MeshRenderer>() != null)
                            {
                                circle.AddComponent<MeshCollider>();
                                circle.GetComponent<MeshRenderer>().material.color = randomColor;
                                circle.GetComponent<MeshRenderer>().receiveShadows = false;
                            }
                            var euler2 = transform.eulerAngles;
                            euler2.y = Random.Range(0, 360f);
                            parent.transform.eulerAngles = euler2;
                        }
                    }
                    break;
                }
                else if ((levelRatios[y] >= PlayerPrefs.GetInt("level", 1) && PlayerPrefs.GetInt("level", 1) < levelRatios[y + 1]))
                {
                    howManyCircles = howManyCirclesInLevels[y-1];
                    Debug.Log(howManyCircles);
                    for (int i = 0; i < howManyCircles; i++)
                    {
                        int randomCircle = Random.Range(0, circlesAtLevel[y-1]);
                        GameObject parent = new GameObject();
                        parent.transform.position = cylinder.transform.position;
                        parent.transform.parent = cylinder.transform;
                        parent.tag = "Circle";
                        for (int z = 0; z < circles[randomCircle].meshes.Length; z++)
                        {
                            GameObject circle = Instantiate(circles[randomCircle].meshes[z], cylinder.transform.position, Quaternion.identity, parent.transform);
                            circle.transform.localScale = new Vector3(350, 350, 550);
                            circle.transform.localEulerAngles = new Vector3(-90, 0, circles[randomCircle].rotationZ[z]);
                            circle.layer = 8;
                            circle.tag = "Circle";
                            if (circle.GetComponent<MeshRenderer>() != null)
                            {
                                circle.AddComponent<MeshCollider>();
                                circle.GetComponent<MeshRenderer>().material.color = randomColor;
                                circle.GetComponent<MeshRenderer>().receiveShadows = false;
                            }
                            var euler2 = transform.eulerAngles;
                            euler2.y = Random.Range(0, 360f);
                            parent.transform.eulerAngles = euler2;
                        }
                    }
                    break;
                }
            }
        }
        InstantieFullCircle();
        cylinder.GetComponent<Cylinder>().PlaceAllCirclesCorrectly();
    }

    private void Update()
    {
        levelSlider.value = Mathf.Lerp(levelSlider.value, (float)player.GetComponent<BirdScript>().skippedCounter / howManyCircles, 0.5f);
    }

    public void Mute()
    {
        if(isMuted)
        {
            muteButton.image.sprite = unmuted;
            isMuted = !isMuted;
            AudioListener.volume = 1f;
        }
        else
        {
            muteButton.image.sprite = muted;
            isMuted = !isMuted;
            AudioListener.volume = 0f;
        }
    }
    
    public void StartButton()
    {
        isGameStarted = true;
        isPlayerAlive = true;
        OpenThatPanel(-1);
    }

    public void adsShow()
    {
        // Check if UnityAds ready before calling Show method:
        if (PlayerPrefs.GetInt("deadCount") % 7 == 0)
        {

            Advertisement.Show(myPlacementID);

        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
        if(PlayerPrefs.GetInt("RemoveAds",0) != 1)
        {
            adsShow();
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
        SceneManager.LoadScene(0);
    }

    public void OpenThatPanel(int whichPanel)
    {
        for(int i = 0; i < panels.Length-1; i++)
        {
            panels[i].SetActive(false);
        }

        if(whichPanel != -1)
        {
            panels[whichPanel].SetActive(true);
        }
    }

    /*private void OnValidate()
    {
        for (int i = 0; i < howManyCircles; i++)
        {
            int randomCircle = Random.Range(0, circles.Length);
            GameObject parent = new GameObject();
            parent.transform.position = cylinder.transform.position;
            parent.transform.parent = cylinder.transform;
            parent.tag = "Circle";
            for (int z = 0; z < circles[randomCircle].meshes.Length; z++)
            {
                GameObject circle = Instantiate(circles[randomCircle].meshes[z], cylinder.transform.position, Quaternion.identity, parent.transform);
                circle.transform.localScale = new Vector3(350, 350, 550);
                circle.transform.localEulerAngles = new Vector3(-90, 0, circles[randomCircle].rotationZ[z]);
                circle.layer = 8;
                circle.tag = "Circle";
                var euler2 = transform.eulerAngles;
                euler2.y = Random.Range(0, 360f);
                parent.transform.eulerAngles = euler2;
            }
        }
    }*/

    void InstantieFullCircle()
    {
        GameObject circle = Instantiate(fullCircle, cylinder.transform.position, Quaternion.identity, cylinder.transform);
        circle.transform.localScale = new Vector3(350, 350, 550);
        circle.transform.eulerAngles = new Vector3(-90, 0, 0);
        circle.layer = 8;
        circle.tag = "LastCircle";
        circle.AddComponent<MeshCollider>();
        circle.GetComponent<MeshRenderer>().material.color = randomColor2;
        cylinderMesh.GetComponent<MeshRenderer>().material.color = randomColor2;
        cylinder.GetComponent<Cylinder>().PlaceAllCirclesCorrectly();
    }
}
