using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [RequireComponent(typeof(Collider2D))]
    public class QuestDialogueTrigger : MonoBehaviour
    {        
        // reference to player tag 
        private static string _playerTag = "Player";

        // reference to npc controller
        [SerializeField]
        private NpcController _triggerNpc; 

        [SerializeField]
        private bool _isItem = false;

        private bool _isActive = false; 

        private bool _cPressed = false;

        private bool _isColliding = false;

        private bool _choiceComplete = false;

        [Header("Dialogue Choices")]
        [SerializeField]
        private Choices _choices; 

        [SerializeField]
        private int _choiceIndex;

        private int _choiceMadeIndex = 0;

        [Header("Quest Source")]
        [SerializeField]
        private QuestSource _questSource; 

        // reference to current index
        private int _currentIndex = 0; 

        private bool _choiceReached = false;

        [Header("Dialogue Context")]
        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                
        // reference to status of player pressing next
        private bool _nextPressed = false; 

        private void Awake()
        {
            ChoiceUIController.Instance.onChoiceMadeCallback += EvaluateChoice;
        }

        private void EvaluateChoice(int choiceMadeIndex)
        {               
            if(_isActive)
            {
                _choiceMadeIndex = choiceMadeIndex-1;
                Debug.Log("Choice made A: " + _choiceMadeIndex);
                bool questExists = _questSource.OnActivated(_choiceMadeIndex);
                if(!questExists)
                {   
                    // no quest for selected choice
                    QuestDialogueManager.Instance.SkipNextSentence(_choiceMadeIndex);
                    _choiceReached = false;
                    NextSentence();
                    Debug.Log("No Quest for selected choice, exiting");
                }

                // buttons indexed 1,2,3 - so -1 necessary
                //DialogueManager.Instance.SkipNextSentence(choiceIndex-1);
                //_choiceReached = false;
                //NextSentence();
            }
        }

        public void QuestAcceptanceStatusResult(bool accepted)
        {
            if(_isActive)
            {
                if(accepted)
                {
                    Debug.Log("Choice made: " + _choiceMadeIndex);
                    QuestDialogueManager.Instance.SkipNextSentence(_choiceMadeIndex);
                    _choiceReached = false;
                    NextSentence();
                    _choiceComplete = true;
                }
                else 
                {
                    Debug.Log("Quest Rejected, Exiting dialogue");
                    ExitDialogue();
                }
            }

        }
        private void Update()
        {

            if(_choiceIndex != -1 && _choiceIndex == _currentIndex && _isActive && !_nextPressed && !_choiceReached)
            {
                ChoiceUIController.Instance.DispalyChoices(_choices);
                _choiceReached = true;
                _nextPressed = false;
            }
                // TRIGGER CHOICES DISPALY AND WAIT FOR RESPONSE
            if(Input.GetKeyDown(KeyCode.C) && _isColliding && !_choiceReached)
            {
                _cPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && _isActive && !_choiceReached)
            {
                _nextPressed = true;
                if(_isItem)
                {
                    ExitDialogue();
                    _nextPressed = false;
                    _isActive = false;
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == _playerTag)
                _isColliding = true;

            // if item, set active
            if(_triggerNpc == null && collision.tag == _playerTag)
            {
                _isActive = true;
                TriggerDialogue(); 
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag == _playerTag && _triggerNpc != null)
                _triggerNpc.CanMove = false;

            if(collision.tag == _playerTag && _cPressed && _triggerNpc != null)
            {
                InventoryUIController.Instance.HideInventory();
                _isActive = true;
                TriggerDialogue();
                _cPressed = false;
            }

            if (_nextPressed && collision.tag == _playerTag && _isActive)
            {
                if(_choiceComplete)
                {
                    ExitDialogue();
                    _isActive = false; 
                    _triggerNpc.CanMove = true;
                    _isColliding = false;
                    transform.gameObject.GetComponent<QuestSource>().enabled = false; 
                    transform.gameObject.GetComponent<QuestDialogueTrigger>().enabled = false;
                    transform.gameObject.GetComponent<DialogueTrigger>().enabled = true;
                    //transform.gameObject.SetActive(false);
                    // TODO: Add dictionary code to not re-accept this quest
                }

                else
                {
                    NextSentence();
                   _nextPressed = false;
                }

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == _playerTag && _triggerNpc != null)
            {
                _isActive = false;
                ExitDialogue();                
                _triggerNpc.CanMove = true;
                _isColliding = false;

            }
        }

        // function riggering dialogue
        private void TriggerDialogue()
        {
            _questSource.OnDeactivated();
            QuestDialogueManager.Instance.StartDialogue(_dialogue);
            if(_triggerNpc != null)
                _triggerNpc.CanMove = false;
        }

        // function triggering next sentence
        private void NextSentence()
        {
            QuestDialogueManager.Instance.DisplayNextSentence();
            _currentIndex ++;
            if(QuestDialogueManager.Instance.DialogueExited)
            {
                ExitDialogue();
                InventoryUIController.Instance.HideInventory();
            }
        }

        // function triggering exit dialogue
        private void ExitDialogue()
        {
            QuestDialogueManager.Instance.EndDialogue();
            
            if(_choiceReached)
                ChoiceUIController.Instance.DisableCanavs();
            _questSource.OnDeactivated();

            _choiceMadeIndex = 0;
            _isActive = false;
            _currentIndex = 0; 
            _choiceMadeIndex = 0;
            _choiceReached = false; 
            _nextPressed = false;
            _choiceComplete = false;

        }

    }
}