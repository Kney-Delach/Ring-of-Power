using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class InventoryUIController : MonoBehaviour
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

        // inventory activated delegate 
        public delegate void OnInventoryActive();

	    // instantiate inventory active observer set
        public OnInventoryActive onInventoryActiveCallback;

        // reference to visibility status of inventory UI 
        private bool _active  = false;

        private bool _itemsActive = false; 
        private bool _rokemonActive = false;
        private bool _questsActive = false; 

        #region Singleton 

        // reference to instance
        private static InventoryUIController _instance = null;
        public static InventoryUIController Instance { get { return _instance; } }

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
            _inventoryGroup.alpha = 0;
            _inventoryGroup.interactable = false;
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
            if(onInventoryActiveCallback != null)
                onInventoryActiveCallback.Invoke();
            ActivateItems();
            _active = true;
            _inventoryGroup.alpha = 1;
            _inventoryGroup.interactable = true;
            _inventoryGroup.blocksRaycasts = true;
        }

        private void HideInventory()
        {

            _active = false;
            _inventoryGroup.alpha = 0;
            _inventoryGroup.interactable = false;
            _inventoryGroup.blocksRaycasts = false;
        }

        public void ActivateItems()
        {
            DeactivateActive();
            if(onInventoryActiveCallback != null)
                onInventoryActiveCallback.Invoke();
            _itemsActive = true;
            _itemCanvasGroup.alpha = 1;
            _itemCanvasGroup.interactable = true;
            _itemCanvasGroup.blocksRaycasts = true;
        }

        public void DeactivateItems()
        {
            _itemCanvasGroup.alpha = 0;
            _itemCanvasGroup.interactable = false;
            _itemCanvasGroup.blocksRaycasts = false;
            _itemsActive = false;
        }

        public void ActivateRokemon()
        {
            DeactivateActive();
            _rokemonActive = true; 
            _rokemonCanvasGroup.alpha = 1; 
            _rokemonCanvasGroup.interactable = true;
            _rokemonCanvasGroup.blocksRaycasts = true;
        }

        public void DeactivateRokemon()
        {
            _rokemonCanvasGroup.alpha = 0; 
            _rokemonCanvasGroup.interactable = false;
            _rokemonCanvasGroup.blocksRaycasts = false;
            _rokemonActive = false; 
        }

        public void ActivateQuests()
        {
            DeactivateActive();
            _questsActive = true; 
            _questCanvasGroup.interactable = true;
            _questCanvasGroup.blocksRaycasts = true; 
            _questCanvasGroup.alpha = 1;
        }

        public void DeactivateQuests()
        {
            _questCanvasGroup.interactable = false; 
            _questCanvasGroup.alpha = 0;
            _questCanvasGroup.blocksRaycasts = false;
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
