using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class QuestRequestUIController : MonoBehaviour
    {   
        #region UI Region 

        [SerializeField]
        private Text _questTitleText; 

        [SerializeField]
        private Text _questBodyText; 

        [SerializeField]
        private ItemInventorySlot[] _questRewardItems; 

        #endregion
        // reference to whether or not the quest has been accepted
        private bool _acceptedStatus;
        public delegate void OnRequestStatus(bool accepted);
        public OnRequestStatus onRequestChoiceMadeCallback;

        #region #singleton
        // reference to instance 
        private static QuestRequestUIController _instance = null; 
        public static QuestRequestUIController Instance { get { return _instance ; } }

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

        public void QuestAcceptance(bool accepted)
        {
            _acceptedStatus = accepted; 
            onRequestChoiceMadeCallback(_acceptedStatus);
            InventoryUIController.Instance.QuestRequestActive = false; 
            InventoryUIController.Instance.EnableButtons();
        }

        public void AssignQuestUIValues(Quest quest)
        {
            InventoryUIController.Instance.QuestRequestActive = true; 
            InventoryUIController.Instance.DisableButtons(); 

            _questTitleText.text = quest.questTitle;
            _questBodyText.text = quest.questDescription;
            if(quest.questRewards.Length == 3)
            {
                for(int i = 0; i < 3; i++)
                    _questRewardItems[i].AddItem(quest.questRewards[i]);
            }else if(quest.questRewards.Length == 2)
            {
                _questRewardItems[2].gameObject.SetActive(false);
                for(int i = 0; i < 2; i++)
                    _questRewardItems[i].AddItem(quest.questRewards[i]);

            }else 
            {
                _questRewardItems[2].gameObject.SetActive(false);
                _questRewardItems[1].gameObject.SetActive(false);
                _questRewardItems[0].AddItem(quest.questRewards[0]);
            }
        }
    }
}