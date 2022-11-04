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
    public float timeScale;
    public int currentHungerPoints;
    public int maxHungerPoints;
    public int livesStage;

    [Header("Display Scores/Points/Lives")]
    public TextMeshProUGUI hungerPointsDisplay;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetHungerDisplay(); 
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
        --livesStage;
    }
}