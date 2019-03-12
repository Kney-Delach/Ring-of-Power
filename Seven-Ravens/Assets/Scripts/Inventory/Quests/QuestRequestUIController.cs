using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class QuestRequestUIController : MonoBehaviour
    {   
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
        }

        public void AssignQuestUIValues(Quest quest)
        {
            
        }
    }
}