using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class ItemsCollectedManager : MonoBehaviour
    {       
        [SerializeField]
        private Collider2D _unlockedZone;

        [SerializeField]
        private bool _zoneUnlocked = false;
        [SerializeField]
        private GameObject _gemItem;

        [SerializeField]
        private GameObject _goodItem; 

        [SerializeField]
        private GameObject _cursedItem; 

        private bool _gemCollected = false;
        private bool _itemActivated = false;
        private bool _goodItemActivated = false;
        private bool _itemCollected = false;

        private void Update()
        {
            if(!_gemItem.active)
                _gemCollected = true;
            
            if((_goodItem.activeSelf || _cursedItem.activeSelf) && !_itemActivated)
            {
                if(_goodItem.activeSelf)
                    _goodItemActivated = true; 
                else 
                    _goodItemActivated = false;


                _itemActivated = true;
                if(_unlockedZone.enabled)
                    _unlockedZone.enabled = false;
            }

            if(_itemActivated && !_itemCollected)
            {
                if(_goodItemActivated && !_goodItem.active)
                {
                    _itemCollected = true;
                }
                else if(!_goodItemActivated && !_cursedItem.active)
                {
                    _itemCollected = true;
                }
            }

            if((_gemCollected && (_itemActivated && _itemCollected)) || (_gemCollected && !_itemActivated))
            {
                _unlockedZone.enabled = true;
                _zoneUnlocked = true;
            }
            else if(_itemActivated && _gemCollected && !_itemCollected)
            {
                _unlockedZone.enabled = false;
                _zoneUnlocked = false;
            }
        }
    }
}