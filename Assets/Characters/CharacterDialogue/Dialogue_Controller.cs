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



    [Header("Choice Properties")]
  //  public choiceMakerInfo currentChoice;
    public GameObject currentChoiceObject;
    public GameObject choiceDisplay;
    public bool choiceMade;

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
        startConvo();
     //   StartCoroutine(typeText(currentConvo.conversations[convoIndex].convoText)); // do thhe ++convoindex thing, make a sceneController script, add onto Dialogue Canvas

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space)) && choiceMade == true) // cant input when a choice is being made
        {
            if (state == State.COMPLETED) // if the line is completed
            {
                NextLine();
            }

            else // in the midst of typing
            {
                StopAllCoroutines();
       //         textPanel.text = currentConvo.conversations[convoIndex].convoText;
                state = State.COMPLETED;

            }
        }
    }


    public void startConvo()
    {
        convoIndex = 0;
    }

    private IEnumerator typeText(string text)
    {
        if (convoIndex == 3)
        {
 //           characterController.aliceAppear();
        }
        textPanel.text = "";
        state = State.TALKING;

        // probably move this into another function
  //      namePanel.text = currentConvo.conversations[convoIndex].speaker.speakerName;
    //    namePanel.color = currentConvo.conversations[convoIndex].speaker.nameColor;

//        currentChoice = currentConvo.conversations[convoIndex].makeChoice;
        // could be better coded
  //      if (currentChoice != null)
        {
            print("pog make a choice");
            StartCoroutine(makeAChoice());
        }
   //     Sprite emotionParam = currentConvo.conversations[convoIndex].currentSprite;
   //     showEmotion(emotionParam);

        int charIndex = 0;

        while (state != State.COMPLETED)
        {
            // add audio clip of text here
            textPanel.text += text[charIndex];
            yield return new WaitForSeconds(typingSpeed);

            if (++charIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }




        yield return null;


    }


    public void NextLine() // controls the value of convoIndex, which is the marker for which speaker is speaking
    {

        // depends on line 79
        //if (convoIndex < currentConvo.conversations.Count - 1)
        //{
        //    convoIndex++;
        //    textPanel.text = string.Empty;
        //    StartCoroutine(typeText(currentConvo.conversations[convoIndex].convoText));
        //}

        //else
        //{
        //    print("end of dialogue for now");
        //}
    }


    // in future use in separate script
    public void showEmotion(Sprite spriteEmotion)
    {
  //      if (currentConvo.conversations[convoIndex].speaker.speakerName == "Alice")
        {
            if (spriteEmotion == null)
            {
                print("no emotion animation will be played");
                return;
            }

            // since I know the names of the sprites for each emotion
            switch (spriteEmotion.name)
            {
                case "pointAlice1":
                    baseAliceSprite.sprite = spriteEmotion;
                    aliceAnimator.SetTrigger("smartass");
                    break;

                case "tipAlice1":
                    baseAliceSprite.sprite = spriteEmotion;
                    break;

                case "aliceTransform1":
                    baseAliceSprite.sprite = spriteEmotion;
                    aliceAnimator.SetTrigger("transformD");
                    break;

            }

        }
    }


    public IEnumerator makeAChoice()
    {
        choiceMade = false;
        currentChoiceObject = null;


        // handle spawning the choices
        // do a list of gameobjects
        // temp shitty choice system, make a struct within the choiceMakerInfo later for the customization of choices
        float number = 75f;
 //       if (currentChoice.numberOfChoices == 1)
        {
            number = 0;
        }

  //      for (int i = 0; i < currentChoice.numberOfChoices; ++i)
        {
   //         GameObject prefabC = Instantiate(currentChoice.choiceResource, choiceDisplay.transform);
  //          prefabC.transform.GetComponentInChildren<TextMeshProUGUI>().text = currentChoice.choiceTextOptions[i];
   //         prefabC.transform.localPosition = new Vector3(0, number);
   //         prefabC.GetComponent<choiceInfo>().pathNumberChoice = i; // change later
            number -= 150;
        }
        //GameObject prefabC = Instantiate(currentChoice.choiceResource, choiceDisplay.transform);


        while (currentChoiceObject == null)
        {

            yield return null;
            if (currentChoiceObject != null)
            {
                Transform cDisplay = GameObject.Find("Choice Display").transform;
                foreach (Transform choice in cDisplay)
                {
                    choice.gameObject.SetActive(false);
                }
                // then move onto next conversation or change the currentConvo;
                yield return new WaitForSeconds(.5f);
                convoIndex++;
                textPanel.text = string.Empty;
 //               StartCoroutine(typeText(currentConvo.conversations[convoIndex].convoText));
                break;
            }
        }

        print("you finished choosing");
        choiceMade = true;



        yield return null;

    }
}
