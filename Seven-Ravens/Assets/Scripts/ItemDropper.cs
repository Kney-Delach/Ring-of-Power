using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class ItemDropper : MonoBehaviour
    {
        [Header("Dropable items")]
        [SerializeField]
        private GameObject _cursedItem; 

        [SerializeField]
        private GameObject _goodItem; 


        public void DropCursedItem()
        {
            _cursedItem.SetActive(true);
        }

        public void DropGoodItem()
        {
            _goodItem.SetActive(true);
        }
    }
}