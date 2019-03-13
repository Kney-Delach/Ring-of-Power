using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [RequireComponent(typeof(Collider2D))]
    public class DialogueTrigger : MonoBehaviour
    {        
        // reference to player tag 
        private static string _playerTag = "Player";

        // reference to npc controller
        [SerializeField]
        private NpcController _triggerNpc; 

        [SerializeField]
        private bool _isItem = false;

        private bool _isActive = false; 

        private bool _itemProcessed = false;

        private bool _cPressed = false;

        private bool _isColliding = false;

        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                
        // reference to status of player pressing next
        private bool _nextPressed = false; 

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C) && _isColliding)
            {
                _cPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && _isActive)
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
            if(_isItem && collision.tag == _playerTag && !_itemProcessed)
            {
                // hide inventory
                InventoryUIController.Instance.HideInventory();
                _isActive = true;
                TriggerDialogue(); 
                _itemProcessed = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(!_isItem)
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
            }

            if (_nextPressed && collision.tag == _playerTag && _isActive)
            {
                NextSentence();
                _nextPressed = false;
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
            DialogueManager.Instance.StartDialogue(_dialogue);
            if(_triggerNpc != null)
                _triggerNpc.CanMove = false;
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