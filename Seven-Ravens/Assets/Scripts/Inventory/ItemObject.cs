using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon{
    [RequireComponent(typeof(Collider2D))]
    public class ItemObject : MonoBehaviour
    {
        private static int _itemIdCounter = 0;

        // reference to this item's ID
        [SerializeField]
        private int _itemId; 
        public int ItemId {get { return _itemId ; } set { _itemId = value ; } }

        private static Dictionary<int, bool> _itemIdDatabase;

        [SerializeField]
        private Item _item; 

        // reference to player tag
        private string _playerTag = "Player";

        private void Awake()
        {
            if(_itemIdDatabase == null)
                _itemIdDatabase = new Dictionary<int, bool>();
        }

        private void Start()
        {
            // if true, reloading scene
            if(_itemIdDatabase.ContainsKey(_itemId))
            {
                // if bool is true, then item has been collected already
                if(_itemIdDatabase[_itemId])
                {
                    Destroy(transform.parent.gameObject);
                }
            }
            else 
            {
                _itemIdDatabase.Add(_itemId, false);
                gameObject.GetComponent<SpriteRenderer>().sprite = _item.icon;
            }


        }

         private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == _playerTag)
            {
                ItemInventory.Instance.Add(_item);
                gameObject.SetActive(false);
                _itemIdDatabase[_itemId] = true; // set collected in database
                //Destroy(gameObject);
            }
        }

    }
}
