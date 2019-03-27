using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon{
    [RequireComponent(typeof(Collider2D))]
    public class ItemObject : MonoBehaviour
    {
        // reference to total item ID counts 
        private static int _itemIdCounter = 0;

        // reference to this item's ID
        [SerializeField]
        private int _itemId; 
        public int ItemId {get { return _itemId ; } set { _itemId = value ; } }

        // reference storing all item IDs and whether or not they have been collected
        private static Dictionary<int, bool> _itemIdDatabase;

        // reference to this item object's item
        [SerializeField]
        private Item _item; 

        [SerializeField]
        private bool _isGem = false;

        [SerializeField]
        private Collider2D _unlockedZone;

        [SerializeField]
        private AudioController _itemPickupSFX; 

        // reference to player tag
        private static string PLAYER_TAG = "Player";

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
                else{
                    gameObject.GetComponent<SpriteRenderer>().sprite = _item.icon;
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
            if (collision.tag == PLAYER_TAG)
            {
                _itemPickupSFX.PlaySfx();
                ItemInventory.Instance.Add(_item);
                gameObject.SetActive(false);
                _itemIdDatabase[_itemId] = true; // set collected in database

                // if(_isGem && _unlockedZone != null)
                //     _unlockedZone.enabled = true;
                    
                //Destroy(gameObject);
            }
        }

    }
}
