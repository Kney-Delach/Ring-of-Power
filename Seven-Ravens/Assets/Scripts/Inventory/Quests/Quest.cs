using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon{

    [System.Serializable]
    public class Quest
    {
        public bool isActive; 

        public string questTitle; 

        public string questDescription; 

        public Item[] questRewards;

        public List<QuestGoal> questGoals;

    }
}
