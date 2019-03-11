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

        private bool _qPressed = false;

        private bool _isColliding = false;

        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                
        // reference to status of player pressing next
        private bool _nextPressed = false; 

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q) && _isColliding)
            {
                _qPressed = true;
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

            if(collision.tag == _playerTag && _qPressed && _triggerNpc != null)
            {
                _isActive = true;
                TriggerDialogue();
                _qPressed = false;
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
        }

    }
}