using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class InventoryUIController : MonoBehaviour
    {
        [Header("Controlling Buttons")]
        [SerializeField]
        private ButtonHighlighter _itemsButton;

        [SerializeField]
        private ButtonHighlighter _rokemonButton;

        [SerializeField]
        private ButtonHighlighter _questButton;

        [SerializeField]
        private CanvasGroup _buttonGroup;

        [Header("Context Panel Canvas Groups")]
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

        [SerializeField]
        private CanvasGroup _questAllCanvasGroup; 

        [SerializeField]
        private CanvasGroup _questRequestCanvasGroup; 

        // inventory activated delegate 
        public delegate void OnInventoryActive();

	    // instantiate inventory active observer set
        public OnInventoryActive onInventoryActiveCallback;

        // reference to visibility status of inventory UI 
        private bool _active  = false;

        private bool _itemsActive = false; 
        private bool _rokemonActive = false;
        private bool _questsActive = false; 

        private bool _questRequestActive = false;
        public bool QuestRequestActive {get { return _questRequestActive ; } set { _questRequestActive = value; } }

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
            // TODO: Add option to switch between items, rokemon and quests with I,R,Q

            if(Input.GetKeyDown(KeyCode.I) && !_active && !_questRequestActive)
                DisplayInventory();
            else if(Input.GetKeyDown(KeyCode.I) && _active && !_questRequestActive)
                HideInventory();
        }

        public void DisplayInventory()
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

        public void HideInventory()
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
            _itemsButton.HighlightButton();
            _itemCanvasGroup.alpha = 1;
            _itemCanvasGroup.interactable = true;
            _itemCanvasGroup.blocksRaycasts = true;
        }

        public void DeactivateItems()
        {
            _itemsButton.UnHighlightButton();
            _itemCanvasGroup.alpha = 0;
            _itemCanvasGroup.interactable = false;
            _itemCanvasGroup.blocksRaycasts = false;
            _itemsActive = false;
        }

        public void ActivateRokemon()
        {
            DeactivateActive();
            _rokemonButton.HighlightButton();
            _rokemonActive = true; 
            _rokemonCanvasGroup.alpha = 1; 
            _rokemonCanvasGroup.interactable = true;
            _rokemonCanvasGroup.blocksRaycasts = true;
        }

        public void DeactivateRokemon()
        {
            _rokemonButton.UnHighlightButton();
            _rokemonCanvasGroup.alpha = 0; 
            _rokemonCanvasGroup.interactable = false;
            _rokemonCanvasGroup.blocksRaycasts = false;
            _rokemonActive = false; 
        }

        public void ActivateQuests(bool isRequest)
        {
            DeactivateActive();
            _questButton.HighlightButton();
            
            _questsActive = true; 
            _questCanvasGroup.interactable = true;
            _questCanvasGroup.blocksRaycasts = true; 
            _questCanvasGroup.alpha = 1;

            if(isRequest)
            {
                _questRequestCanvasGroup.alpha = 1; 
                _questRequestCanvasGroup.interactable = true; 
                _questRequestCanvasGroup.blocksRaycasts = true;
                
            }else {
                _questAllCanvasGroup.interactable = true; 
                _questAllCanvasGroup.alpha = 1; 
                _questAllCanvasGroup.blocksRaycasts = true;
            }

        }

        public void DeactivateQuests()
        {
            _questButton.UnHighlightButton();
            _questCanvasGroup.interactable = false; 
            _questCanvasGroup.alpha = 0;
            _questCanvasGroup.blocksRaycasts = false;
            _questsActive = false; 
            
            _questAllCanvasGroup.interactable = false; 
            _questAllCanvasGroup.alpha = 0; 
            _questAllCanvasGroup.blocksRaycasts = false;

            _questRequestCanvasGroup.alpha = 0; 
            _questRequestCanvasGroup.interactable = false; 
            _questRequestCanvasGroup.blocksRaycasts = false;
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

        public void DisableButtons()
        {
            _buttonGroup.interactable = false; 
        }

        public void EnableButtons()
        {
            _buttonGroup.interactable = true;
        }

    }
}
