using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Rokemon {

    public enum ComType
    {
        Dialogue, 
        Quest,
        Choice, 
        Cutscene,
        None
    }

    // manages all communication events
    public class ComManager : MonoBehaviour
    {
        #region Dialogue UI Components

        [Header("Dialogue UI Components")]

        // reference to dialogue name text UI object
        [SerializeField]
        private Text _dialogueNameText;

        // reference to dialogue context text UI object
        [SerializeField]
        private Text _dialogueContextText;

        // reference to dialogue name text UI rect transform
        [SerializeField]
        private RectTransform _dialogueNameTransform;

        // reference to dialogue typing animator 
        [SerializeField]
        private CanvasGroup _dialogueGroup;

        // reference to dialogue press space to continue text
        [SerializeField]
        private Text _dialogueContinueText;

        #endregion

        #region Dialogue Data Variables

        // reference to active status of communications
        private bool _active = false; 
        public bool Active { get { return _active ; }}

        // reference to the current communication type
        private ComType _currentComType; 
        public ComType CurrentComType { get {return _currentComType ; } }

        // reference to queue of sentences for current dialogue 
        private Queue<string> _sentences;

        // reference to status of dialouge 
        private bool _dialogueActive = false; 
        public bool DialogueActive {get { return _dialogueActive ; } }

        private Dialogue _currentDialogue = null;

        #endregion

        #region Singleton Initialization

        // reference to communication instance
        private static ComManager _instance; 
        public static ComManager Instance { get { return _instance ; } }

        // initialize instance
        private void Awake()
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

        #endregion


        // initialise sentences queue
        private void Start()
        {
            _sentences = new Queue<string>();
        }

        // process communication if active
        private void Update()
        {
            if(_active && _currentDialogue.displayDialogue)
            {
                switch (_currentComType)
                {
                    case ComType.Dialogue:
                        ProcessDialogue();
                        break;
                    case ComType.Quest:
                        ProcessQuest();
                        break;
                    case ComType.Choice:
                        ProcessChoice();
                        break;
                    case ComType.Cutscene:
                        ProcessCutscene();
                        break;
                    case ComType.None:
                        Debug.LogError("ComManager Update Error: ComType Non whilst active, Fail Safe Case!");
                        break;
                    default:
                        Debug.Log("ComManagerBegin Communication Error, type not valid");
                        break;
                }
            }
        }
        #region Communications

        public void BeginCommunication(ComType type, Dialogue dialogue)
        {
            if(_active)
            {
                Debug.Log("ComManager BeginCommunication Error: Communications already active, cannot begin new communication session!");
            }
            else 
            {
                _currentDialogue = dialogue;    // set current dialogue reference

                PlayerController.Instance.FreezePlayer(); // freeze player movement
                _active = true;         // set active
                _currentComType = type; // update current communication type
                StartCommDialogue(dialogue); // start the dailogue
            }
        }

        // reset variables for ending communication
        public void EndCommunication()
        {
            _currentDialogue = null; // update active dialogue reference
            _active = false;         // set inactive
            _currentComType = ComType.None;  // update current communication type
            HideDialogueUI();
            _sentences.Clear();
            PlayerController.Instance.UnfreezePlayer(); // unfreeze player movement
        }

        #endregion

        #region Dialogue

        // process dialogue communication type
        private void ProcessDialogue()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(_currentDialogue.displayContinue)
                {
                    DisplayNextSentence();
                }
            }
        }

        #endregion

        #region Cutscene

        // process dialogue communication type
        private void ProcessCutscene()
        {
            CutsceneManager.Instance.BeginCutscene();
        }

        #endregion

        #region Quest
        
        private void ProcessQuest()
        {
            
        }

        #endregion

        #region Choice
        
        private void ProcessChoice()
        {
            
        }

        #endregion


        #region Process 

        // skip a number of sentences in the dialogue
        private void SkipSentences(int skipAmount)
        {
            if(skipAmount != 0)
            {
                StopAllCoroutines();
                
                for(int i = 0; i < skipAmount; i++)
                {
                    string sentence = _sentences.Dequeue();
                }
            }
        }

        // displays the next sentence in the dialogue
        private void DisplayNextSentence()
        {
            // if no more sentences, end communication
            if (_sentences.Count == 0)
            {
                EndCommunication();
                return;
            }

            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        // coroutine to type out the dialogue sentence
        IEnumerator TypeSentence(string sentence)
        {
            _dialogueContextText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _dialogueContextText.text += letter;
                yield return null;
            }
        }

        #endregion

        #region Initialise Process

        // starts a new communication dialogue 
        public void StartCommDialogue(Dialogue dialogue)
        {
            if(dialogue.displayDialogue)
            {
                DisplayDialogueUI();
                InitialiseSentences(dialogue);
                DisplayNextSentence();      // display initial sentence
            }
            else 
                HideDialogueUI();

        }

        // intiialises sentences for the dialogue 
        private void InitialiseSentences(Dialogue dialogue)
        {
            if (!dialogue.displayName)
                _dialogueNameText.gameObject.SetActive(false);
            else
                _dialogueNameText.text = dialogue.speakerName;

            _sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                _sentences.Enqueue(sentence);
            }
        }

        // activate dialogue ui canvas group
        private void DisplayDialogueUI()
        {
            _dialogueGroup.alpha = 1;
            _dialogueGroup.interactable = true;
        }

        // hide dialogue ui canvas group
        private void HideDialogueUI()
        {
            _dialogueGroup.alpha = 0;
            _dialogueGroup.interactable = false;
        }

        #endregion

    }

}