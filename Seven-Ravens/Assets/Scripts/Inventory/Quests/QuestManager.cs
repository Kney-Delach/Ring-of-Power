using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class QuestManager : MonoBehaviour
    {
        private Quest _activeQuest;

        private bool _activeQuestCompleted = false;

        private bool _assigned = false;

        #region #singleton
        // reference to instance 
        private static QuestManager _instance = null; 
        public static QuestManager Instance { get { return _instance ; } }

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


        // assign new quest to manager 
        public void AssignQuest(Quest quest)
        {   
            if(!_assigned)
            {
                _activeQuest = quest;
                _activeQuest.isActive = true;
                _activeQuestCompleted = false;
            }
            else 
            {
                // Check if player wants to disband previous quest, then remove current quest and add new one
            }
        }

        // remove assigned quest from manager
        public void RemoveQuest()
        {
            if(_assigned)
            {
                _activeQuest = null;
                _assigned = false;
                _activeQuestCompleted = false;
            }
        }

        // check status of goals of active quest
        public void CheckGoals()
        {
            if(_activeQuest != null && _assigned && !_activeQuestCompleted)
            {
                // check if completed goals
            }
            
        }

        
    }
}
