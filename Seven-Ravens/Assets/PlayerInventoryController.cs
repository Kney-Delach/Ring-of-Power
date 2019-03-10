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
                _inventoryGroup.alpha = 0;
                _inventoryGroup.interactable = false;
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
    }
}
