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

        #region Choices Data Variables

        private ResponseType[] _currentReponses;
        // reference to current choice options 
        private Choices _currentChoices;

        private int _choiceIndex = 0;

        private int _currentIndex = 0;

        private bool _choiceMade = false;


        #endregion 

        #region Controller References

        private ComController _currentController = null;

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
            
            _sentences = new Queue<string>();
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
            ChoiceUIController.Instance.onChoiceMadeCallback += EvaluateChoice;
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

        // function performing generic steps in beginning communication
        public void BeginCommunication(ComType type, Dialogue dialogue)
        {
            if(_active)
            {
                Debug.Log("ComManager BeginCommunication Error: Communications already active, cannot begin new communication session!");
            }
            else 
            {
                
                if(InventoryUIController.Instance != null)
                    InventoryUIController.Instance.HideInventory();
                if(QuestInventoryUIController.Instance != null)
                    QuestInventoryUIController.Instance.HideQuestLog();
                
                TargetUIController.Instance.TargetChange(null); // remove target UI's target

                _currentDialogue = dialogue;    // set current dialogue reference
                PlayerController.Instance.FreezePlayer(); // freeze player movement
                PlayerController.Instance.RemoveTarget();
                _active = true;         // set active
                _currentComType = type; // update current communication type
                StartCommDialogue(dialogue); // start the dailogue
            }
        }

        // regular dialogue communication initializer 
        public void BeginCommunication(ComType type, Dialogue dialogue, ComController currentController)
        {
            if(_active)
            {
                Debug.Log("ComManager BeginCommunication Error: Communications already active, cannot begin new communication session!");
            }
            else 
            {
                _currentController = currentController; // reference to current com controller
                BeginCommunication(type, dialogue);
            }
        }
        
        // choice dialouge communication initialzer 
        public void BeginCommunication(ComType type, Dialogue dialogue, Choices choices, int choiceIndex, ResponseType[] responses, ComController currentController)
        {
            if(_active)
            {
                Debug.Log("ComManager BeginCommunication Error: Communications already active, cannot begin new communication session!");
            }
            else 
            {
                _currentReponses = responses; // assing current responses
                _choiceIndex = choiceIndex;
                _currentChoices = choices;
                _currentController = currentController; // reference to current com controller

                BeginCommunication(type, dialogue);
            }
        }

        private void EndComHelper()
        {
            _currentReponses = null;
            _currentIndex = 0;  // reset current index
            _choiceIndex = 0;   // reset choices index
            _currentDialogue = null; // update active dialogue reference
            _active = false;         // set inactive
            _currentComType = ComType.None;  // update current communication type
            HideDialogueUI();
            _sentences.Clear();
        }
        // reset variables for ending communication
        public void EndCommunication()
        {
            
            if(_currentController != null && _currentController.EventTrigger)
            {
                EndComHelper();
                _currentController.TriggerComplete();
                Debug.Log("REACHED SIMULATION POINT");
            }
            else if(_currentController != null)
            {
                EndComHelper();
                PlayerController.Instance.UnfreezePlayer(); // unfreeze player movement
                _currentController.Instance.TriggerComplete();
                _currentController = null; // reset reference to com controller
                Debug.Log("REACHED SIMULATION POINT 2");
            } else
            {
                EndComHelper();
                PlayerController.Instance.UnfreezePlayer(); // unfreeze player movement
                Debug.Log("REACHED SIMULATION POINT 3");
            }
            
           
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
            // TODO: Delete this call?
        }

        #endregion

        #region Quest
        
        private void ProcessQuest()
        {
            
        }

        #endregion

        #region Choice
        
        // process choice communication
        private void ProcessChoice()
        {
            if(_currentIndex < _choiceIndex)
            {
                ProcessDialogue();
            }
            else if(_currentIndex == _choiceIndex)
            {
                ChoiceUIController.Instance.DispalyChoices(_currentChoices);
                _currentIndex++;
            }else if(_currentIndex > _choiceIndex && _choiceMade)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    EndCommunication();
                }
                
            }
            // TODO : add update options for after choice complete?
        }

        // evaluate the choice made by the user
        public void EvaluateChoice(int choiceMadeIndex)
        {  
            ActionBarUIController.Instance.HideAllFlashes();
            Debug.Log("Evaluating Choice");
            ResponseType responseChosen = _currentReponses[choiceMadeIndex-1];
            switch(responseChosen)
            {
                case ResponseType.Cutscene:
                    if(CutsceneManager.Instance != null)
                    {
                        Debug.Log("ComManager EvaluateChoice: Choice type response: " + responseChosen);
                        EndCommunication();
                        CutsceneManager.Instance.BeginCutscene();
                    }
                    break;
                case ResponseType.Kill:

                    PlayerInformationController.Instance.AddChoice(ChoicesMadeType.Neutral); // set neutral decision made

                    ActionBarUIController.Instance.ActivateFireboltFlash();
                    _choiceMade = true;
                    SkipSentences(choiceMadeIndex-1);
                    DisplayNextSentence();
                    GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach(GameObject obj in objects)
                    {
                        EnemyController tempController = obj.GetComponentInChildren<EnemyController>();
                        if(tempController != null)
                            tempController.CanCastFirebolt = false;
                    }
                    //     obj.SetActive(false);
                    
                     _currentController.Npc.SetActive(true);
                    Debug.Log("Process Kill Choice");
                    break;
                case ResponseType.Trade:
                    PlayerInformationController.Instance.AddChoice(ChoicesMadeType.Good); // set good decision made

                    _choiceMade = true;
                    SkipSentences(choiceMadeIndex-1);
                    DisplayNextSentence();
                    PlayerController.Instance.DeactivateAbility(_currentController.AbilityName);
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
                     foreach(GameObject obj in objs)
                     {
                        EnemyController tempController = obj.GetComponentInChildren<EnemyController>();
                        if(tempController != null)
                            tempController.CanCastFirebolt = false;
                     }
                    //     obj.SetActive(false);
                     _currentController.Npc.SetActive(true);
                    _currentController.Npc.layer = 1;
                    _currentController.Npc.SetActive(false);
                    _currentController.Npc.GetComponent<ItemDropper>().DropGoodItem();
                    Debug.Log("Process Trade Choice");
                    break;
                case ResponseType.Nothing:
                    _choiceMade = true;
                    SkipSentences(choiceMadeIndex-1);
                    DisplayNextSentence();
                    _currentController.Npc.layer = 1;
                    _currentController.Npc.SetActive(false);
                    Debug.Log("Process Nothing Choice");
                    EndCommunication();
                    break;
                default:
                    Debug.Log("ComManager EvaluateChoice: Choice type response: " + responseChosen);
                    break;
            }
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
            _currentIndex++;
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
                if(dialogue.displayContinue)
                    _dialogueContinueText.enabled = true;
                 else 
                    _dialogueContinueText.enabled = false;
                
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