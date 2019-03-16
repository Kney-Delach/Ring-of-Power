using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon{
    public class ItemInventory : MonoBehaviour
    {
        // reference to the number of available item slots in the inventory
        [SerializeField]
        private int _itemSlots = 32; 
        public int ItemSlots {get { return _itemSlots ;} }

        // reference to current list of items in item inventory
	    private List<Item> _items = new List<Item>();
        public List<Item> Items {get { return _items ; } }
        
        // item change delegate 
        public delegate void OnItemChanged();

	    // instantiate item change observer set
        public OnItemChanged onItemChangedCallback;
        
        #region Singleton

        // reference to instance
        private static ItemInventory _instance = null;
        public static ItemInventory Instance { get { return _instance ; } }

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



        // add a new item to inventory if enough room
        public void Add (Item item)
        {
            if (item.showInInventory) {
                if (_items.Count >= _itemSlots) {
                    Debug.Log("ItemInventory Add: Not enough room in inventory!"); // TODO: Display UI notification
                    return;
                }

                _items.Add (item);

                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke(); // notify observers
            }
        }

        // remove an item from the inventory
        public void Remove (Item item)
        {
            _items.Remove(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // notify observers
        }
    }
}
