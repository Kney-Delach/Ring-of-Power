using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/World-Map")]
    public class ItemMap : Item
    {

        public override void Use()
        {
            Debug.Log("Used item MAP: " + name);
        }
    }
}