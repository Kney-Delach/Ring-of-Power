using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [RequireComponent(typeof(QuestDialogueTrigger))]
    public class QuestSource : MonoBehaviour
    {
        #region ID REGISTRATION
        // reference to quest source ID counter 
        private static int _itemIdCounter = 0;

        // reference to this quest source's ID
        [SerializeField]
        private int _questSourceID; 
        public int QuestSourceID {get { return _questSourceID ; } set { _questSourceID = value ; } }

        private static Dictionary<int, bool> _questSourceIdDatabase;
        #endregion
        

        private static string PLAYER_TAG = "Player";
        [SerializeField]
        private Quest[] _quests;

        [SerializeField]
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
            if(_questSourceIdDatabase == null)
                _questSourceIdDatabase = new Dictionary<int, bool>();

            QuestRequestUIController.Instance.onRequestChoiceMadeCallback += QuestAcceptanceStatus;
        }

        private void Start()
        {
            // if true, reloading scene
            if(_questSourceIdDatabase.ContainsKey(_questSourceID))
            {
                // if bool is true, then quest has been collected already
                if(_questSourceIdDatabase[_questSourceID])
                {
                    transform.gameObject.GetComponent<QuestSource>().enabled = false; 
                    transform.gameObject.GetComponent<QuestDialogueTrigger>().enabled = false;
                    transform.gameObject.GetComponent<DialogueTrigger>().enabled = true;
                }
                else
                {
                    transform.gameObject.GetComponent<DialogueTrigger>().enabled = false;
                }
            }
            else 
            {
                _questSourceIdDatabase.Add(_questSourceID, false);
                transform.gameObject.GetComponent<DialogueTrigger>().enabled = false;
            }
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
            if(_active)
            {
                if(accepted)
                {
                    RegisterAcceptedQuest();
                    SwitchQuestPanel();
                    _questSourceIdDatabase[_questSourceID] = true;
                }
                else {
                    OnDeactivated();
                }

                _questDialogueTrigger.QuestAcceptanceStatusResult(accepted);
            }

        }

        private void SwitchQuestPanel()
        {
            bool val = false;
            InventoryUIController.Instance.ActivateQuests(val);
        }

        // register accepted quest
        private void RegisterAcceptedQuest()
        {
            Debug.Log(_currentQuest);
            QuestManager.Instance.AssignQuest(_currentQuest);
        }
    }
}