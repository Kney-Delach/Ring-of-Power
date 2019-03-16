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
        private CanvasGroup _questSourceCanvas; 

        [SerializeField]
        private ItemInventorySlot[] _questRewardItems; 

        #endregion
        // reference to whether or not the quest has been accepted
        private bool _acceptedStatus;

        // reference to activity status of this UI 
        private bool _active = false ;
        public bool Active { get { return _active ; } }

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
            HideRequestCanvas();
            _acceptedStatus = accepted; 
            onRequestChoiceMadeCallback(_acceptedStatus);

            // TODO notify other UI they can be activated again

        }

        public void AssignQuestUIValues(Quest quest)
        {
            // TODO notify other UI they cannot be activated as quest UI active

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

            DisplayRequestCanvas();

        }

        public void HideRequestCanvas()
        {   
            _active = false;
            _questSourceCanvas.interactable = false;
            _questSourceCanvas.blocksRaycasts = false; 
            _questSourceCanvas.alpha = 0;
        }
        private void DisplayRequestCanvas()
        {   
            _active = true;
            _questSourceCanvas.interactable = true;
            _questSourceCanvas.blocksRaycasts = true; 
            _questSourceCanvas.alpha = 1;
        }

    }
}