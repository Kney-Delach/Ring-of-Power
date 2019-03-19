using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class EventDialogueManager : MonoBehaviour
    {
        // reference to dialogue name text UI object
        [SerializeField]
        private Text _nameText;

        // reference to dialogue context text UI object
        [SerializeField]
        private Text _dialogueText;

        // reference to dialogue typing animator 
        [SerializeField]
        private CanvasGroup _dialogueGroup;

        // reference to queue of sentences for current dialogue 
        private Queue<string> _sentences;

        // reference to activity status of dialouge 
        private bool _active = false; 
        public bool Active {get { return _active ; } }
        // reference to object instance
        private static EventDialogueManager _instance;
        public static EventDialogueManager Instance {  get { return _instance ; } }
        
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
            _active = true; // dialogue active

            _dialogueGroup.alpha = 1;
            _dialogueGroup.interactable = true;

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

        // skip the next "number of sentences" sentences 
        public void SkipNextSentence(int numberOfSentences)
        {
            if(numberOfSentences == 0)
            {
                char a; // TODO: Fix this bug
            }
            else
            {
                StopAllCoroutines();
                
                for(int i = 0; i < numberOfSentences; i++)
                {
                    string sentence = _sentences.Dequeue();
                }

            }
        }
        
        // coroutine to type the sentence letter by letter
        IEnumerator TypeSentence(string sentence)
        {
            _dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _dialogueText.text += letter;
                yield return null;
            }
        }

        // end the dialogue
        public void EndDialogue()
        {
            _dialogueGroup.alpha = 0;
            _dialogueGroup.interactable = false;
            _sentences.Clear();
            _active = false;
        }

    }
}