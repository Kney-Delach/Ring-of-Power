using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [RequireComponent(typeof(Collider2D))]
    public class DialogueTrigger : MonoBehaviour
    {        
        // reference to player tag 
        private static string PLAYER_TAG = "Player";

        // reference to npc controller
        [SerializeField]
        private NpcController _triggerNpc; 

        [SerializeField]
        private bool _isItem = false;

        private bool _isActive = false; 

        private bool _itemProcessed = false;

        private bool _isColliding = false;

        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }                

        private void Update()
        {   
            ProcessDialogue();
        }

        private void ProcessDialogue()
        {
            if(_isColliding)
            {
                if(!_isItem)
                { 
                    if(_triggerNpc != null)
                    {
                        _triggerNpc.FreezeMovement = false;
                        if(Input.GetKeyDown(KeyCode.C))
                        {
                            _isActive = true;
                            TriggerDialogue();
                        }
                    }                      
                }
                else if(!_itemProcessed)
                {
                    _isActive = true;
                    TriggerDialogue(); 
                    _itemProcessed = true;
                }

                if(_isActive)
                {
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        if(_isItem)
                        {
                            ExitDialogue();
                            _isActive = false;
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            NextSentence();
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == PLAYER_TAG)
            {
                _isColliding = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == PLAYER_TAG && _triggerNpc != null)
            {
                _isActive = false;
                ExitDialogue();                
                _triggerNpc.FreezeMovement = true;
                _isColliding = false;
            }
        }

        // function riggering dialogue
        private void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(_dialogue);
            if(_triggerNpc != null)
                _triggerNpc.FreezeMovement = false;
        }

        // function triggering next sentence
        private void NextSentence()
        {
            DialogueManager.Instance.DisplayNextSentence();
            if(DialogueManager.Instance.DialogueExited)
            {
                ExitDialogue();
                InventoryUIController.Instance.HideInventory();
            }
        }

        // function triggering exit dialogue
        private void ExitDialogue()
        {
            DialogueManager.Instance.EndDialogue();
            _isActive = false;
            if(_isItem)
            {
                gameObject.SetActive(false);
            }
        }

    }
}