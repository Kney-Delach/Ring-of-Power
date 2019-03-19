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

        // reference to currently active item inventory
        ItemInventory _itemInventory;

        // inventory activated delegate 
        public delegate void OnInventoryActive();

	    // instantiate inventory active observer set
        public OnInventoryActive onInventoryActiveCallback;

        // reference to visibility status of inventory UI 
        private bool _active  = false;
        public bool Active { get { return _active ;} }

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
            _active = false;
            _inventoryGroup.alpha = 0;
            _inventoryGroup.interactable = false;
            _inventoryGroup.blocksRaycasts = false;

            _itemInventory = ItemInventory.Instance;
            _itemInventory.onItemChangedCallback += UpdateUI;
        }       


        private void Update()
        {
            // // TODO: Add check for other interfaces active
            // if(Input.GetKeyDown(KeyCode.I) && !_active)
            //     DisplayInventory();
            // else if(Input.GetKeyDown(KeyCode.I) && _active)
            //     HideInventory();
        }

        public void DisplayInventory()
        {
            UpdateUI();
   
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

        // update inventory UI by:
        //		- Adding items
        //		- Clearing empty slots
        public void UpdateUI ()
        {
            ItemInventorySlot[] slots = GetComponentsInChildren<ItemInventorySlot>();

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < _itemInventory.Items.Count)
                {
                    slots[i].AddItem(_itemInventory.Items[i]);
                } else
                {
                    slots[i].ClearSlot();
                }
            }
        }


    }
}
