﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject 
    {
    
    // reference to item name
    new public string name = "New Item";
    
    // reference to item icon 
	public Sprite icon = null;

    public string description = "";

    // reference to item inventory display status
	public bool showInInventory = true;

    public bool isGem = false;

    public bool isUsable = false;

    public BrotherRefColor brotherRefColor;

	// called when the item is pressed in the inventory
	public virtual void Use ()
	{
        // TODO: use the item
        Debug.Log("Used item: " + name);
	}

	// Call this method to remove the item from inventory
	public void RemoveFromInventory ()
	{
		ItemInventory.Instance.Remove(this);
	}

    }
}