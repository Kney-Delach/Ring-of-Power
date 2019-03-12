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

        private GameObject _descriptionObject; 

        private CanvasGroup _descriptionCanvasGroup; 

        private Text _descriptionCanvasText; 

        private Image _descriptionImage; 


        // reference to current item in inventory slot
        private Item _item;

        private void Start()
        {
            _descriptionObject = GameObject.FindGameObjectWithTag("DescriptionPanel");
            _descriptionCanvasGroup = _descriptionObject.GetComponent<CanvasGroup>(); 
            _descriptionCanvasText = _descriptionObject.GetComponentInChildren<Text>(); 
            GameObject _tempImageObject = GameObject.FindGameObjectWithTag("DescriptionImage");
            _descriptionImage = _tempImageObject.GetComponent<Image>();

            _descriptionCanvasGroup.interactable = false; 
            _descriptionCanvasGroup.blocksRaycasts = false; 
            _descriptionCanvasGroup.alpha = 0;
        }
        // Add item to the slot
        public void AddItem (Item newItem)
        {
            _item = newItem;

            _itemIcon.sprite = _item.icon;
            _itemIcon.enabled = true;
           
            if(_removeItemButtonCanvasGroup != null)
            {
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
            _descriptionCanvasText.text = _item.description;
            _descriptionImage.sprite = _item.icon;         
            _descriptionCanvasGroup.alpha = 1;   
        }

        public void StopDisplayItemDescription()
        {
            _descriptionCanvasGroup.alpha = 0;   

        }
    }
}