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
    public TextMeshProUGUI livesDisplay;

    private bool killedReimu = false;

    [Header("Handle Dialogue Flags")]
    public Dialogue_Controller d_control;
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
        /* code below to uncomment when 1st stage is ready */
        //Dialogue_Controller d_control = GameObject.FindGameObjectWithTag("DialogueCanvas").GetComponent<Dialogue_Controller>();
        //if (d_control != null) // avoid situation where it can't find Dialogue Canvas
        //{
        //    d_control.startConvo();
        //    d_control.dialoguePanel.SetActive(true);
        //    d_control.character1.SetActive(true);
        //    d_control.character2.SetActive(true);

        //}


        //if (d_control == null)
        //{
        //    Debug.Log("Can't find Dialogue Canvas");
        //}

        
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
        livesDisplay.text = "Lives: " + livesStage.ToString();



        // Dialogue Code

        if(Input.GetKeyDown(KeyCode.I))
        {
            d_control = GameObject.FindGameObjectWithTag("DialogueCanvas").GetComponent<Dialogue_Controller>();
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

    /// BOMB & DESTROY ALL BULLETS IN SCENE
    /// 
    /// ////////////////////////////////////
    public void DisableAllBullets()
    {
        print("disabled all bullets");
        regularCustomBehavior[] allregularBullets = GameObject.FindObjectsOfType<regularCustomBehavior>();
        foreach(regularCustomBehavior bullet in allregularBullets)
        {
            bullet.gameObject.SetActive(false);
        }
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

    ////////////////////////// HANDLE DIALOGUE //

    public void StartSpecificConvo(int TconvoIndex)
    {
        print("started convo of choice");
        d_control.TouhouConversationIndex = TconvoIndex;
        d_control.currentConvo = d_control.TouhouConversations[TconvoIndex];
        d_control.startConvo();
    }
}
