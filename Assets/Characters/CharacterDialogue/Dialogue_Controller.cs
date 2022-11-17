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

    [Header("Conversation Scene Info")]

   
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

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Z))
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
        convoIndex = 0;
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
                print("finished");
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

            // since I know the names of the sprites for each emotion
            //switch (spriteEmotion.name)
            //{
            //    case "pointAlice1":
            //        baseAliceSprite.sprite = spriteEmotion;
            //        aliceAnimator.SetTrigger("smartass");
            //        break;

            //    case "tipAlice1":
            //        baseAliceSprite.sprite = spriteEmotion;
            //        break;

            //    case "aliceTransform1":
            //        baseAliceSprite.sprite = spriteEmotion;
            //        aliceAnimator.SetTrigger("transformD");
            //        break;

            //}

       // }
    }

}
