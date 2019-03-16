// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Rokemon{
//     public class ItemInventoryUIController : MonoBehaviour
//     {
//         // reference to currently active inventory
//         ItemInventory _itemInventory;

//         InventoryUIController _inventoryUiController;

//         void Start ()
//         {
//             _itemInventory = ItemInventory.Instance;
//             _itemInventory.onItemChangedCallback += UpdateUI;
//             _inventoryUiController = InventoryUIController.Instance;
//             _inventoryUiController.onInventoryActiveCallback += UpdateUI;
//         }

//         // update inventory UI by:
//         //		- Adding items
//         //		- Clearing empty slots
//         public void UpdateUI ()
//         {
//             ItemInventorySlot[] slots = GetComponentsInChildren<ItemInventorySlot>();

//             for (int i = 0; i < slots.Length; i++)
//             {
//                 if (i < _itemInventory.Items.Count)
//                 {
//                     slots[i].AddItem(_itemInventory.Items[i]);
//                 } else
//                 {
//                     slots[i].ClearSlot();
//                 }
//             }
//         }
//     }
// }