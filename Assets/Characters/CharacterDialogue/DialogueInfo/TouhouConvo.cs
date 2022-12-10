using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTouhouConvo", menuName = "Data/New Touhou Convo")]
public class TouhouConvo : ScriptableObject
{

    [Header("scene's convo")]
    public List<Conversation> conversations;
    

    [System.Serializable]
    public struct Conversation
    {
        [TextArea(2, 10)]
        public string convoText;
        public Sprite currentSpriteEmotion;
        public TouhouSpeaker character;
        public Action currentAction;

        [SerializeField]
        public enum Action
        {
            IDLE, SHOW, DISAPPEAR
        }
    }
}
