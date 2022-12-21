using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue_Controller : MonoBehaviour
{
    public float typingSpeed;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI namePanel;
    
    public SakuyaStage sakuya_stage;
    public YoumuStage youmu_stage;

    [Header("Conversation Scene Info")]

    public TouhouConvo[] TouhouConversations;
    public int TouhouConversationIndex;
    public TouhouConvo currentConvo;
    private int convoIndex = 0;
    private State state = State.COMPLETED;
    public int pathSceneNumber = 0;
    // to keep track of what path number I took


    public Image baseAliceSprite;
    public Animator aliceAnimator;

    [Header("People and Textbox")]
    public GameObject dialoguePanel;
    public GameObject character1;
    public GameObject character2;
    public bool convoStarted;
    

    //[Header("Controls Characters")]
   // public CharacterController characterController;
    private enum State
    {
        TALKING, COMPLETED
    }

    // Start is called before the first frame update
    void Start()
    {
        // for now start at the beginning, change to function called at start
        // startConvo(); // now has both index = 0 and the start coroutein, can be called from GameManagerScript
        // do thhe ++convoindex thing, make a sceneController script, add onto Dialogue Canvas
        TouhouConversationIndex = 0;// Touhouconversationindex and convoindex are different, convoindex is used to represent place in the conversations list
                                    // inside a Touhou Dialogue

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Z) && convoStarted == true)
        {
            if (state == State.COMPLETED) // if the line is completed
            {
                NextLine();
            }

            else // in the midst of typing
            {
                StopAllCoroutines();
                textPanel.text = currentConvo.conversations[convoIndex].convoText;
                state = State.COMPLETED;

            }
        }
    }


    public void startConvo()
    {
        convoStarted = true;
        convoIndex = 0;

        // enable ui 
        dialoguePanel.SetActive(true); // polish later on by adding text box animations for beginning/end
        character1.SetActive(true);
        character2.SetActive(true);

        StartCoroutine(typeText(currentConvo.conversations[convoIndex].convoText));
    }

    private IEnumerator typeText(string text)
    {
        namePanel.text = currentConvo.conversations[convoIndex].character.name;
        namePanel.color = currentConvo.conversations[convoIndex].character.nameColor;
        textPanel.text = "";

        state = State.TALKING;

        int label = currentConvo.conversations[convoIndex].character.characterLabel;
        Sprite emotionParam = currentConvo.conversations[convoIndex].currentSpriteEmotion;
        showEmotion(emotionParam, label);

        int charIndex = 0;

        while (state != State.COMPLETED)
        {
            // add audio clip of text here
            textPanel.text += text[charIndex];
            yield return new WaitForSeconds(typingSpeed);

            if (++charIndex == text.Length)
            {
               // print("finished");
                state = State.COMPLETED;
                break;
            }
        }

        yield return null;
    }


    public void NextLine() // controls the value of convoIndex, which is the marker for which speaker is speaking
    {

        // depends on line 79
        if (convoIndex < currentConvo.conversations.Count - 1)
        {
            convoIndex++;
            textPanel.text = string.Empty;
            StartCoroutine(typeText(currentConvo.conversations[convoIndex].convoText));
        }

        else
        {
            print("end of dialogue for now");
            // temporary disappearance of dialogue box and characters
            if(dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(false);
                character1.SetActive(false);
                character2.SetActive(false);
            }
          

            convoStarted = false;

            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                StartSakuyaPhase(TouhouConversationIndex);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                YoumuPhaseSelector(TouhouConversationIndex);
            }
            
            //sakuya_stage.StartPhase1(); // make switch based on build index so that stages dialogue ends, should be called in GameManager
            
        }
    }


    // in future use in separate script
    public void showEmotion(Sprite spriteEmotion, int speakerNumber) // 1 or 2
    {
  
            if (spriteEmotion == null)
            {
                print("no emotion animation will be played");
                return;
            }

        if(speakerNumber == 1)
        {
            character1.GetComponent<Image>().sprite = spriteEmotion;
        }
        else if(speakerNumber == 2)
        {
            character2.GetComponent<Image>().sprite = spriteEmotion;
        }


        else
        {
            print("no emotion sprite will be displayed");
        }

    

       // }
    }

    public void ShowUI()
    {

    }

    public void HideUI()
    {

    }
    // Move to GameManager later, give sakuyastage variable
    public void StartSakuyaPhase(int stageIndex)
    {
        switch (stageIndex)
        {
            case 0:
                sakuya_stage.StartPhase1();
                break;
            case 1:
                print("started");
               StartCoroutine( sakuya_stage.StartPhase2Coroutine() );
                break;
            case 2:
                // start conversation after phase 3 ended
                // move onto next scene

                GameManager.instance.GetComponent<AudioSource>().Pause();
                SceneManager.LoadScene(2);
                break;
   
        }
        return;
    }

    public void YoumuPhaseSelector(int phaseIndex)
    {
        switch (phaseIndex)
        {
            case 0: // start phase 1
                youmu_stage.StartYoumuPhase1();
                break;
            case 1:
                print("phase 2 should start");
                youmu_stage.StartYoumuPhase2();
                break;
            case 2:
                // victory screen
                print("phase 3 should start");
                break;

        }
        return;
    }
}
