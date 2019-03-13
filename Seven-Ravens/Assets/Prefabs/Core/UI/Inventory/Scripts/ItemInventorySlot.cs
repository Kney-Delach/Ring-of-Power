using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rokemon{
    // controls a single inventory slot
    public class ItemInventorySlot : MonoBehaviour
    {
        // reference to slot icon
        [SerializeField] 
        private Image _itemIcon;

        // reference to remove item button
        [SerializeField]
        private CanvasGroup _removeItemButtonCanvasGroup; 

        [SerializeField]
        private bool _isQuest = false;

        [SerializeField]
        private bool _isQSource = false;

        // reference to description controller instance
        private ItemDescriptionUIController _descriptionController; 
        // reference to current item in inventory slot
        private Item _item;


        private void Start()
        {
            _descriptionController = ItemDescriptionUIController.Instance;
        }
        // Add item to the slot
        public void AddItem (Item newItem)
        {
            _item = newItem;

            _itemIcon.sprite = _item.icon;
            _itemIcon.enabled = true;
           
            if(_removeItemButtonCanvasGroup != null)
            {
                _removeItemButtonCanvasGroup.blocksRaycasts = true;
                _removeItemButtonCanvasGroup.alpha = 1;
                _removeItemButtonCanvasGroup.interactable = true;
            }
        }

        // removes item from inventory slot
        public void ClearSlot ()
        {
            _item = null;

            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
           
            if(_removeItemButtonCanvasGroup != null)
            {
                _removeItemButtonCanvasGroup.interactable = false;
                _removeItemButtonCanvasGroup.alpha = 0;
                _removeItemButtonCanvasGroup.blocksRaycasts = false;
            }
        }

        // called if remove button pressed
        public void RemoveItemFromInventory ()
        {
            //ItemInventory.Instance.Remove(_item);
            _item.RemoveFromInventory();
        }

        // TODO: update to count number of items in bag, or to remove after use? 
        // use the item 
        public void UseItem ()
        {
            if (_item != null)
            {
                _item.Use();
            }
        }

        public void DisplayItemDescription()
        {   
            Debug.Log(_isQuest);
            _descriptionController.EnableCanvas(_item, _isQuest, _isQSource); 
        }

        public void StopDisplayItemDescription()
        {
            _descriptionController.DisableCanvas(); 
        }
    }
}