using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class QuestInventorySlot : MonoBehaviour
    {
        private Quest _quest; 
        public Quest Quest {get { return _quest ; } }
        
        [SerializeField]
        private Button _questDataButton; 
        public Button QuestDataButton { get { return _questDataButton ; } }

        [SerializeField]
        private Text _questTitleText; 
    

                // Add item to the slot
        public void AddItem (Quest newQuest)
        {
            //_questObject.SetActive(true);

            _quest = newQuest;
            _questDataButton.enabled = true;
            _questTitleText.enabled = true;
            _questTitleText.text = _quest.questTitle; 
            

            // int count = 0; 
            // for(int i = 0; i < _quest.questRewards.Length; i++)
            // {
            //     count++; 
            //     _itemRewardSlots[i].gameObject.SetActive(true);
            //     _itemRewardSlots[i].AddItem(_quest.questRewards[i]);
            // }

            // for(int c = count; c < 3; c++)
            // {
            //     _itemRewardSlots[c].gameObject.SetActive(false);
            // }


        }

                // removes item from inventory slot
        public void ClearSlot ()
        {
            _quest = null;  
            _questDataButton.enabled = false;
            _questTitleText.enabled = false;

            //_questObject.SetActive(false);

        }
    }
}
