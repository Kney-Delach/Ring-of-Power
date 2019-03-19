using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon{
    public class QuestInventory : MonoBehaviour
    {
        [SerializeField]
        private int _questSlots = 1;
        public int QuestSlots {get { return _questSlots ; } }
        
        public int QuestItemSlots {get { return _questSlots ; } }
        
        // reference to current list of quests in quest inventory
	    private List<Quest> _quests = new List<Quest>();
        public List<Quest> Quests {get { return _quests ; } }

        // quest change delegate
        public delegate void OnQuestChanged();

	    // instantiate quest change observer set
        public OnQuestChanged onQuestChangedCallback;

        #region #singleton
        // reference to instance 
        private static QuestInventory _instance = null; 
        public static QuestInventory Instance { get { return _instance ; } }

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


        public void Add(Quest quest)
        {
            if(_quests.Count > _questSlots)
            {
                Debug.Log("QuestInventory Add: Not enough room in quest inventory!");
                return;
            }

            _quests.Add(quest);

            if(onQuestChangedCallback != null)
                onQuestChangedCallback.Invoke();    // notify observers

        }


        // remove a quest from the quest inventory
        public void Remove (Quest quest)
        {
            _quests.Remove(quest);

            if (onQuestChangedCallback != null)
                onQuestChangedCallback.Invoke();    // notify observers
        }
    }
}
