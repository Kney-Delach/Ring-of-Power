﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [RequireComponent(typeof(Collider2D))]
    public class QuestDialogueTrigger : MonoBehaviour
    {        
        // reference to player tag 
        private static string PLAYER_TAG = "Player";

        // reference to npc controller
        [SerializeField]
        private NpcController _triggerNpc; 

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

        // reference to status of player pressing next
        private bool _nextPressed = false; 


        [Header("Dialogue Context")]
        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                


        private void Start()
        {
            if(_triggerNpc == null)
                Debug.LogError("QuestDialogueTrigger Start ERROR: NPC trigger not attached to this object!");

            ChoiceUIController.Instance.onChoiceMadeCallback += EvaluateChoice;
        }

        void Update()
        {
            ProcessDialogue();
        }

        // performs the logic for processing the quest dialogue 
        private void ProcessDialogue()
        {
            if(_isColliding)
            {
                if(!_choiceReached)
                {
                    if(Input.GetKeyDown(KeyCode.C))
                    {
                        _isActive = true;
                        TriggerDialogue();
                    } 

                    if(_isActive)
                    {
                        if(Input.GetKeyDown(KeyCode.Space))
                       {
                            if(_choiceComplete)
                            {
                                ExitDialogue();

                                transform.gameObject.GetComponent<QuestSource>().enabled = false; 
                                transform.gameObject.GetComponent<QuestDialogueTrigger>().enabled = false;
                                transform.gameObject.GetComponent<DialogueTrigger>().enabled = true;
                            }
                            else
                            {
                                NextSentence();
                            }

                        }
                        else if(_choiceIndex != -1 && _choiceIndex == _currentIndex)
                        {
                            ChoiceUIController.Instance.DispalyChoices(_choices);
                            _choiceReached = true;
                        }
                         
                    }
                }            
            }
        }
      

        private void EvaluateChoice(int choiceMadeIndex)
        {               
            if(_isActive)
            {   
                // for some reason I index the buttons 1,2,3.... so this is necessary :shrug:
                _choiceMadeIndex = choiceMadeIndex-1;
                bool questExists = _questSource.OnActivated(_choiceMadeIndex);
                if(!questExists)
                {   
                    // no quest for selected choice
                    QuestDialogueManager.Instance.SkipNextSentence(_choiceMadeIndex);
                    _choiceReached = false;
                    NextSentence();
                    
                    // TODO: Do something with this
                    // no quest for choice, continue with dialogue - 
                }
            }
        }

        public void QuestAcceptanceStatusResult(bool accepted)
        {
            if(_isActive)
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
                _isColliding = true;
                _triggerNpc.CanMove = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == PLAYER_TAG)
            {               
                _triggerNpc.CanMove = true;
                _isColliding = false;
                ExitDialogue();                

            }
        }

        // function riggering dialogue
        private void TriggerDialogue()
        {
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
            }
        }

        // function triggering exit dialogue
        private void ExitDialogue()
        {
            QuestDialogueManager.Instance.EndDialogue();
            
            if(_choiceReached)
            {
                ChoiceUIController.Instance.DisableCanavs();
                _choiceReached = false; 

            }
            _questSource.OnDeactivated();
            _choiceMadeIndex = 0;
            _isActive = false;
            _currentIndex = 0; 
            _choiceComplete = false;
        }

    }
}