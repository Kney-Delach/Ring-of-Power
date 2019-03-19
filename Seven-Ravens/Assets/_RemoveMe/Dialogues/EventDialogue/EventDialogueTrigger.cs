using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [RequireComponent(typeof(Collider2D))]
    public class EventDialogueTrigger : MonoBehaviour
    {        
        // reference to player tag 
        private static string PLAYER_TAG = "Player";

        // reference to trigger activity status
        private bool _active = false;         
        
        [Header("Event Specific Checks")]
        [SerializeField]
        private bool _isPhoenix = false; 

        [SerializeField]
        private bool _isTraveller = false; 

        [SerializeField]
        private bool _isTurtle = false; 

        [SerializeField]
        private bool _isElves = false; 

        [SerializeField]
        private bool _isDirewolf = false; 

        [SerializeField]
        private bool _isOgreBattle = false; 

        [SerializeField]
        private bool _isWizard = false; 


        // reference to choice completion status
        private bool _choiceComplete = false;

        [Header("Dialogue Choices")]
        // reference to availble choices at choice index
        [SerializeField]
        private Choices _choices; 

        // reference to choice index in dialogue
        [SerializeField]
        private int _choiceLocationIndex;

        // reference to choice made by player
        private int _choiceMadeIndex;

        // reference to status if choice has been reached or not
        private bool _choiceReached = false;

        // reference to current index
        private int _currentIndex = 0; 

        // reference to status of player pressing next
        private bool _nextPressed = false; 

        [Header("Dialogue Context")]

        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                
        private void Start()
        {
            ChoiceUIController.Instance.onChoiceMadeCallback += EvaluateChoice;
        }

        void Update()
        {
            ProcessDialogue();
        }

        // performs the logic for processing the quest dialogue 
        private void ProcessDialogue()
        {
            if(_active)
            {              
                if(!_choiceReached)
                {
                        if(Input.GetKeyDown(KeyCode.Space))
                       {
                            if(_choiceComplete)
                            {
                                ExitDialogue(); // continue with the world state
                            }
                            else
                            {
                                NextSentence();
                            }

                        }
                        else if(_choiceLocationIndex != -1 && _choiceLocationIndex == _currentIndex)
                        {
                            ChoiceUIController.Instance.DispalyChoices(_choices);
                            _choiceReached = true;
                        }
                }            
            }
        }
      

        private void EvaluateChoice(int choiceMadeIndex)
        {               
            if(_active)
            {   
                _choiceMadeIndex = choiceMadeIndex-1;

                Debug.Log("Do something with the choice made: " + _choiceMadeIndex);
                // for some reason I index the buttons 1,2,3.... so this is necessary :shrug:
//                bool questExists = _questSource.OnActivated(_choiceMadeIndex);
                // if(!questExists)
                // {   
                //     // no quest for selected choice
                //     QuestDialogueManager.Instance.SkipNextSentence(_choiceMadeIndex);
                //     _choiceReached = false;
                //     NextSentence();
                    
                //     // TODO: Do something with this
                //     // no quest for choice, continue with dialogue - 
                // }
            }
        }

        public void QuestAcceptanceStatusResult(bool accepted)
        {
            if(_active)
            {
                _choiceReached = false; // should be true?
                if(accepted)
                {
                    QuestDialogueManager.Instance.SkipNextSentence(_choiceMadeIndex);
                    NextSentence();
                    _choiceComplete = true;
                }
                else 
                {
                    ExitDialogue();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == PLAYER_TAG)
            {
                TriggerDialogue();  // trigger dialogue on collision
                _active = true;     // activate on collision

                // TODO: Freeze player movement until dialogue is complete
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == PLAYER_TAG)
            {               
                _active = false;
                ExitDialogue();                
            }
        }

        // function riggering dialogue
        private void TriggerDialogue()
        {
            EventDialogueManager.Instance.StartDialogue(_dialogue);
        }

        // function triggering next sentence
        private void NextSentence()
        {
            EventDialogueManager.Instance.DisplayNextSentence();
            _currentIndex ++;
            if(!EventDialogueManager.Instance.Active)
            {
                ExitDialogue();
            }
        }

        // function triggering exit dialogue
        private void ExitDialogue()
        {            
            if(_choiceReached)
            {
                ChoiceUIController.Instance.DisableCanavs();
                _choiceReached = false; 
            }
            _choiceMadeIndex = 0;
            _active = false;
            _currentIndex = 0; 
            _choiceComplete = false;
        }

    }
}