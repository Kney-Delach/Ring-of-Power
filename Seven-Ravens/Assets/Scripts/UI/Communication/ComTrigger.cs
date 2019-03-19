using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    
    // initialises a new communication trigger
    public class ComTrigger : MonoBehaviour
    {   
        #region Comm Info 
        [Header("General Communication Info")]
        [SerializeField]
        private ComType _comType;

        #endregion 

        #region Dialogue Info
        [Header("Dialogue Information")]
        [SerializeField]
        private Dialogue _dialogue;  

        #endregion

        public void TriggerCommunication()
        {
            ComManager.Instance.BeginCommunication(_comType, _dialogue);
        }

        public void EndCommunication()
        {
            ComManager.Instance.EndCommunication();
        }
    
    }

}