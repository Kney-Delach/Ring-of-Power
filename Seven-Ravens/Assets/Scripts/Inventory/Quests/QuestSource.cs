using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [RequireComponent(typeof(Collider2D))]
    public class QuestSource : MonoBehaviour
    {
        private static string PLAYER_TAG = "Player";
        [SerializeField]
        private Quest[] _quests;

        private Quest _currentQuest; 

        private int _currentQuestIndex = 0;

        private bool _active = false;

        private bool _qPressed = false;
        private bool _colliding = false; 
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q) && _colliding)
            {
                _qPressed = true;
            }            
        }

        private void OnActivated()
        {
            //_active = true; 
            if(_quests != null && _currentQuestIndex < _quests.Length)
                _currentQuest = _quests[_currentQuestIndex];

            InventoryUIController.Instance.DisplayInventory();
            InventoryUIController.Instance.ActivateQuests();
            Debug.Log(_currentQuest.questTitle + "Requested");
            
        } 

        private void OnDeactivated()
        {
            InventoryUIController.Instance.HideInventory();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _colliding = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag == PLAYER_TAG && _qPressed)
            {
                OnActivated();
                _qPressed = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _colliding = false;
            _qPressed = false;
            OnDeactivated();
        }

    }
}