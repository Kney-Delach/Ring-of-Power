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
            QuestInventorySlot[] slots = GetComponentsInChildren<QuestInventorySlot>();

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < _questInventory.Quests.Count)
                {
                    slots[i].AddItem(_questInventory.Quests[i]);
                } else
                {
                    slots[i].ClearSlot();
                }
            }

            Debug.Log("UpdatedUI");
            
        }
    }
}