using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
   
    public static GameManager instance;

    [Header("Handle Reimu Powers")]
    public GameObject ReimuManager;
    public float timeScale;
    public int currentHungerPoints;
    public int maxHungerPoints;
    public int livesStage;

    [Header("Display Scores/Points/Lives")]
    public TextMeshProUGUI hungerPointsDisplay;

    private bool killedReimu = false;
    private void Awake()
    {
        if(GameManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
        }

        hungerPointsDisplay.text = currentHungerPoints.ToString("D2") + "/" + maxHungerPoints.ToString("D2");
        ReimuManager = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetHungerDisplay(); 

        if(livesStage <= 0 && killedReimu == false)
        {
            killedReimu = true;
            KillReimu();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ++currentHungerPoints;
        }
        if(currentHungerPoints >= maxHungerPoints)
        {
            // Reimu is temp powered up //
            // set trigger Reimu Animation //
            print("now power up");
            IncreaseMaxHungerPoints();
        }


        // Dialogue Code

        if(Input.GetKeyDown(KeyCode.I))
        {
            Dialogue_Controller d_control = GameObject.FindGameObjectWithTag("DialogueCanvas").GetComponent<Dialogue_Controller>();
            if(d_control != null) // avoid situation where it can't find Dialogue Canvas
            {
                d_control.startConvo();
            }

            if(d_control == null)
            {
                Debug.Log("Can't find Dialogue Canvas");
            }
            
        }
    }

    /* hunger points system */
    private void IncreaseMaxHungerPoints()
    {
        maxHungerPoints += 50;
    }
    private void SetHungerDisplay()
    {
        hungerPointsDisplay.text = currentHungerPoints.ToString() + "/" + maxHungerPoints.ToString();
    }

    /* lives and power system*/
    public void PowerUpReimu()
    {

    }

    public void DecreaseLife()
    {
        print("Lost a life");
        --livesStage;
    }

    public void KillReimu()
    {
        print("no lives left: killed Reimu");
        ReimuManager.SetActive(false);
    }
}
