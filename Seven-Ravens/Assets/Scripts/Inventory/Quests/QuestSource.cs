using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [RequireComponent(typeof(QuestDialogueTrigger))]
    public class QuestSource : MonoBehaviour
    {
        private static string PLAYER_TAG = "Player";
        [SerializeField]
        private Quest[] _quests;

        private Quest _currentQuest; 

        // Todo: Utilise this to continue the world? 
        private int _currentQuestIndex = 0;

        private bool _active = false;

        private bool _qPressed = false;
        private bool _colliding = false; 

        [SerializeField]
        private QuestDialogueTrigger _dialogueTrigger; 


        private void Awake()
        {
            QuestRequestUIController.Instance.onRequestChoiceMadeCallback += QuestAcceptanceStatus;
        }
        private void Update()
        {
            // if(Input.GetKeyDown(KeyCode.Q) && _colliding)
            // {
            //     _qPressed = true;
            // }            
        }

        public bool OnActivated(int questIndex)
        {
            // TODO: quest reference to quest inventory
            if(_quests == null)
            {
                Debug.LogError("QuestSource OnActivated: Quests == null error, add a quest to this source");
                return false;
            }

            if(questIndex < _quests.Length)
            {
                _active = true;
                _currentQuest = _quests[questIndex];
                _currentQuestIndex = questIndex;

                InventoryUIController.Instance.DisplayInventory();
                bool val = true; 
                InventoryUIController.Instance.ActivateQuests(val);
                
                Debug.Log(_currentQuest.questTitle + "Requested");

                return true;
            }
            else if(questIndex >= _quests.Length)
            {
                Debug.Log("Quest Doesn't exist");
                return false;
            }

            return false;
            
        } 

        public void OnDeactivated()
        {
            InventoryUIController.Instance.HideInventory();
            _active = false;
        }

        private void QuestAcceptanceStatus(bool accepted)
        {
            OnDeactivated();
            //Debug.Log("_currentQuestIndex:" + _currentQuestIndex);
            _dialogueTrigger.QuestAcceptanceStatus(accepted);
        }

        // private void OnTriggerEnter2D(Collider2D collision)
        // {
        //     if(collision.tag == PLAYER_TAG)
        //         _colliding = true;
        // }

        // private void OnTriggerStay2D(Collider2D collision)
        // {
        //     if(collision.tag == PLAYER_TAG && _qPressed)
        //     {
        //         OnActivated();
        //         _qPressed = false;
        //     }
        // }

        // private void OnTriggerExit2D(Collider2D collision)
        // {
        //     if(collision.tag == PLAYER_TAG)
        //     {
        //         _colliding = false;
        //         _qPressed = false;
        //         OnDeactivated();
        //     }

        // }

    }
}