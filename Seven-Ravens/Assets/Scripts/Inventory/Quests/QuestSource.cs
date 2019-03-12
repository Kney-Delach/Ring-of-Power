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
        private QuestDialogueTrigger _questDialogueTrigger; 


        private void Awake()
        {
            QuestRequestUIController.Instance.onRequestChoiceMadeCallback += QuestAcceptanceStatus;
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
                
                // assigns quest ui current quest 
                QuestRequestUIController.Instance.AssignQuestUIValues(_currentQuest);

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
            if(accepted)
            {
                RegisterAcceptedQuest();
                SwitchQuestPanel();
            }
            else {
                OnDeactivated();
            }

            _questDialogueTrigger.QuestAcceptanceStatusResult(accepted);
        }

        private void SwitchQuestPanel()
        {
            bool val = false;
            InventoryUIController.Instance.ActivateQuests(val);
        }

        // register accepted quest
        private void RegisterAcceptedQuest()
        {
            QuestManager.Instance.AssignQuest(_currentQuest);
        }
    }
}