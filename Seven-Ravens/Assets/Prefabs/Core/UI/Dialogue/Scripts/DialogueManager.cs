using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class DialogueManager : MonoBehaviour
    {
        // reference to dialogue name text UI object
        [SerializeField]
        private Text _nameText;

        // reference to dialogue context text UI object
        [SerializeField]
        private Text _dialogueText;

        // reference to dialogue name text UI rect transform
        [SerializeField]
        private RectTransform _nameTransform; 

        // reference to dialogue typing animator 
        [SerializeField]
        private CanvasGroup _dialogueGroup;

        // reference to queue of sentences for current dialogue 
        private Queue<string> _sentences;

        // reference to status of dialouge 
        private bool _dialogueExited = true; 
        public bool DialogueExited {get { return _dialogueExited ; } }
        // reference to object instance
        private static DialogueManager _instance;
        public static DialogueManager Instance {  get { return _instance ; } }
        
        // initialize instance
        void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        // Use this for initialization
        void Start()
        {
            _sentences = new Queue<string>();
        }

        // starts a new dialogue 
        public void StartDialogue(Dialogue dialogue)
        {
            // if(!QuestDialogueManager.Instance.DialogueExited)
            //     QuestDialogueManager.Instance.EndDialogue();
                
            _dialogueExited = false;
            _dialogueGroup.alpha = 1;
            _dialogueGroup.interactable = true;

            if (dialogue.speakerName == "Sign")
                _nameTransform.gameObject.SetActive(false);
            else
                _nameText.text = dialogue.speakerName;

            _sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        // displays the next sentence in the dialogue
        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            _dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _dialogueText.text += letter;
                yield return null;
            }
        }

        public void EndDialogue()
        {
            _dialogueGroup.alpha = 0;
            _dialogueGroup.interactable = false;
            _sentences.Clear();
            _dialogueExited = true;
        }

    }
}