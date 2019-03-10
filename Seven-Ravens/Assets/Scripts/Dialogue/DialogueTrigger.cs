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

        [SerializeField]
        private NpcController _triggerNpc; 

        // reference to dialogue
        [SerializeField]
        private Dialogue _dialogue;
        public Dialogue Dialogue { get { return _dialogue ; } }
                
        // reference to status of player pressing next
        private bool _nextPressed = false; 

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _nextPressed = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == _playerTag)
            {
                TriggerDialogue();
                _triggerNpc.CanMove = false;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag == _playerTag)
                _triggerNpc.CanMove = false;

            if (_nextPressed && collision.tag == _playerTag)
            {
                NextSentence();
                _nextPressed = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == _playerTag)
            {
                ExitDialogue();
                _triggerNpc.CanMove = true;
            }
        }

        // function riggering dialogue
        private void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }

        // function triggering next sentence
        private void NextSentence()
        {
            DialogueManager.Instance.DisplayNextSentence();
        }

        // function triggering exit dialogue
        private void ExitDialogue()
        {
            DialogueManager.Instance.EndDialogue();
        }

    }
}