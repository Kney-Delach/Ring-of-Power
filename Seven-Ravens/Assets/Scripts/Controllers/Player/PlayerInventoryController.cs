using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon
{
    public class PlayerInventoryController : MonoBehaviour
    {
        // reference to inventory UI canvas group
        private CanvasGroup _inventoryGroup; 

        // reference to visibility status of inventory UI 
        private bool _active  = false;

        // reference to instance
        private static PlayerInventoryController _instance = null;
        public static PlayerInventoryController Instance { get { return _instance; } }

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
            GameObject inventoryObject = GameObject.FindGameObjectWithTag("Inventory");
            
            _inventoryGroup = inventoryObject.GetComponent<CanvasGroup>(); 

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
    }
}
