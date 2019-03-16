using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [System.Serializable]
    public class QuestGoal
    {
        public bool collectionGoalComplete;
        public bool collectionGoal; 
        public string collectionItem; 
        public int collectionAmount; 
        public int collectedAmount;

        public bool findNpcGoalComplete; 
        public bool findNpcGoal; 
        public string npcTag;

        public bool battleGoalComplete; 
        public bool battleGoal; 
        public string battleTag;  


    }
}