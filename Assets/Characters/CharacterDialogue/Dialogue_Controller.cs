using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Controller : MonoBehaviour
{
    public float typingSpeed;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI namePanel;
    
    public SakuyaStage sakuya_stage;

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
        
        textPanel.text = "";
        state = State.TALKING;

        Sprite emotionParam = currentConvo.conversations[convoIndex].currentSpriteEmotion;
        showEmotion(emotionParam);

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
            dialoguePanel.SetActive(false);
            character1.SetActive(false);
            character2.SetActive(false);

            convoStarted = false;
            StartSakuyaPhase(TouhouConversationIndex);
            //sakuya_stage.StartPhase1(); // make switch based on build index so that stages dialogue ends, should be called in GameManager
            
        }
    }


    // in future use in separate script
    public void showEmotion(Sprite spriteEmotion)
    {
  //      if (currentConvo.conversations[convoIndex].speaker.speakerName == "Alice")
       // {
            if (spriteEmotion == null)
            {
                print("no emotion animation will be played");
                return;
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
               StartCoroutine( sakuya_stage.StartPhase2Coroutine() );
                break;
            case 2:
                // start conversation after phase 3 ended
                break;
   
        }
        return;
    }
}
