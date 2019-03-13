using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class QuestInventorySlot : MonoBehaviour
    {
        private Quest _quest; 

        [SerializeField]
        private Text _questTitleText; 
        
        [SerializeField]
        private Text _questDescriptionText; 

        [SerializeField]
        private Text _questProgressText; 

        [SerializeField]
        private CanvasGroup _removeQuestButtonCanvasGroup;


        [SerializeField]
        private ItemInventorySlot[] _itemRewardSlots;

        private void Start()
        {

        }

                // Add item to the slot
        public void AddItem (Quest newQuest)
        {
            _quest = newQuest;
           
            if(_removeQuestButtonCanvasGroup != null)
            {
                _removeQuestButtonCanvasGroup.alpha = 1;
                _removeQuestButtonCanvasGroup.interactable = true;
            }

            _questTitleText.text = _quest.questTitle; 
            _questDescriptionText.text = _quest.questDescription;
            
            // TODO: Insert progress text
            // TODO: Insert completed checkmark

            int count = 0; 
            for(int i = 0; i < _quest.questRewards.Length; i++)
            {
                count++; 
                _itemRewardSlots[i].gameObject.SetActive(true);
                _itemRewardSlots[i].AddItem(_quest.questRewards[i]);
            }

            for(int c = count; c < 3; c++)
            {
                _itemRewardSlots[c].gameObject.SetActive(false);
            }


        }

                // removes item from inventory slot
        public void ClearSlot ()
        {
            _quest = null;
           
            if(_removeQuestButtonCanvasGroup != null)
            {
                _removeQuestButtonCanvasGroup.interactable = false;
                _removeQuestButtonCanvasGroup.alpha = 0;
            }

        }
    }
}
