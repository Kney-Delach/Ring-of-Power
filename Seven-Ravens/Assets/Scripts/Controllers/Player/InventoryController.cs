using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class InventoryController : MonoBehaviour
    {
        // reference to inventory UI canvas group
        [SerializeField]
        private CanvasGroup _inventoryGroup; 

        // reference to item canvas group 
        [SerializeField]
        private CanvasGroup _itemCanvasGroup;
        
        // reference to rokemon canvas group 
        [SerializeField]
        private CanvasGroup _rokemonCanvasGroup; 

        // reference to quests canvas group 
        [SerializeField]
        private CanvasGroup _questCanvasGroup; 

        // reference to visibility status of inventory UI 
        private bool _active  = false;

        private bool _itemsActive = false; 
        private bool _rokemonActive = false;
        private bool _questsActive = false; 

        // reference to instance
        private static InventoryController _instance = null;
        public static InventoryController Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            _inventoryGroup.alpha = 0;
            _inventoryGroup.interactable = false;
        }
        
        // remove instance if destroyed
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I) && !_active)
                DisplayInventory();
            else if(Input.GetKeyDown(KeyCode.I) && _active)
                HideInventory();
        }

        private void DisplayInventory()
        {
            DeactivateActive();
            ActivateItems();
            _active = true;
            _inventoryGroup.alpha = 1;
            _inventoryGroup.interactable = true;
        }

        private void HideInventory()
        {

            _active = false;
            _inventoryGroup.alpha = 0;
            _inventoryGroup.interactable = false;
        }

        public void ActivateItems()
        {
            DeactivateActive();
            _itemsActive = true;
            _itemCanvasGroup.alpha = 1;
            _itemCanvasGroup.interactable = true;
        }

        public void DeactivateItems()
        {
            _itemCanvasGroup.alpha = 0;
            _itemCanvasGroup.interactable = false;
            _itemsActive = false;
        }

        public void ActivateRokemon()
        {
            DeactivateActive();
            _rokemonActive = true; 
            _rokemonCanvasGroup.alpha = 1; 
            _rokemonCanvasGroup.interactable = true;
        }

        public void DeactivateRokemon()
        {
            _rokemonCanvasGroup.alpha = 0; 
            _rokemonCanvasGroup.interactable = false;
            _rokemonActive = false; 
        }

        public void ActivateQuests()
        {
            DeactivateActive();
            _questsActive = true; 
            _questCanvasGroup.interactable = true; 
            _questCanvasGroup.alpha = 1;
        }

        public void DeactivateQuests()
        {
            _questCanvasGroup.interactable = false; 
            _questCanvasGroup.alpha = 0;
            _questsActive = false; 
        }

        private void DeactivateActive()
        {
            if(_itemsActive)
                DeactivateItems();
            else if(_rokemonActive)
            {
                DeactivateRokemon();
            }else if(_questsActive)
            {
                DeactivateQuests();
            }
        }

    }
}
