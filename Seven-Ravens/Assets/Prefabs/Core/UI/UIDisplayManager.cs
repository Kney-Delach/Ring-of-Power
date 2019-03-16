using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class UIDisplayManager : MonoBehaviour
    {
        InventoryUIController _inventoryUI; 
        //QuestRequestUIController _questReqUI;
        EventDialogueManager _eventDialogueManager; 
        QuestDialogueManager _questDialogueManager;
        DialogueManager _dialogueManager; 
        QuestInventoryUIController _questLogUI;

        #region Singleton 

        // reference to instance
        private static UIDisplayManager _instance = null;
        public static UIDisplayManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                _instance = this;
            }
        }

        // remove instance if destroyed
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        #endregion

        private void Start()
        {
            _eventDialogueManager = EventDialogueManager.Instance;
            _inventoryUI = InventoryUIController.Instance; 
            _questLogUI = QuestInventoryUIController.Instance;
            _questDialogueManager = QuestDialogueManager.Instance;
            _dialogueManager = DialogueManager.Instance; 
        }
        private void Update()
        {
            if(!_questDialogueManager.DialogueExited || _eventDialogueManager.Active || !_dialogueManager.DialogueExited)   // TODO: Replace this stupid naming convention
            {
                _inventoryUI.HideInventory();
                _questLogUI.HideQuestLog();
            }
            else 
            {   
                if(Input.GetKeyDown(KeyCode.I))
                {
                    if(!_inventoryUI.Active)
                    {
                        _inventoryUI.DisplayInventory();
                        _questLogUI.HideQuestLog();
                    }
                    else 
                    {                
                        _inventoryUI.HideInventory();                        
                    }
                } 
                else if(Input.GetKeyDown(KeyCode.Q))
                {
                    if(!_questLogUI.Active)
                    {
                        _questLogUI.DisplayQuestLog();
                        _inventoryUI.HideInventory();
                    }
                    else 
                    {
                        _questLogUI.HideQuestLog();                        
                    }
                }
            }
        }
    }
}
