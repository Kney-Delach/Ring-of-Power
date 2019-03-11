using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    public class QuestInventoryUIController : MonoBehaviour
    {
        QuestInventory _questInventory; 

        InventoryUIController _inventoryUiController;

        
        private void Start()
        {
            _questInventory = QuestInventory.Instance;
            _questInventory.onQuestChangedCallback += UpdateUI;
            _inventoryUiController = InventoryUIController.Instance;
            _inventoryUiController.onInventoryActiveCallback += UpdateUI;
        }

        public void UpdateUI()
        {
            // TODO: Implement quest inventory slots 
        }
    }
}