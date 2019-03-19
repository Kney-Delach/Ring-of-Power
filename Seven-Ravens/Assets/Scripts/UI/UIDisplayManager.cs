using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class UIDisplayManager : MonoBehaviour
    {
        InventoryUIController _inventoryUI; 
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
            _inventoryUI = InventoryUIController.Instance; 
            if(QuestInventoryUIController.Instance != null)
                _questLogUI = QuestInventoryUIController.Instance;
        }
        private void Update()
        {
            if(!ComManager.Instance.Active) 
            {   
                if(Input.GetKeyDown(KeyCode.I))
                {
                    if(!_inventoryUI.Active)
                    {
                        _inventoryUI.DisplayInventory();
                        if(_questLogUI != null)
                            _questLogUI.HideQuestLog();
                    }
                    else 
                    {                
                        _inventoryUI.HideInventory();                        
                    }
                } 
                else if(Input.GetKeyDown(KeyCode.Q) && _questLogUI != null)
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
