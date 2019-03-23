using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    
    public enum ResponseType
    {
        Cutscene, 
        Kill, 
        Trade,
        Nothing
    }
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

        #region Choice Info
        [Header("Choice Information")]
        [SerializeField]
        private Choices _choices;  

        [SerializeField]
        private int _choiceIndex = 0;

        [SerializeField]
        private ResponseType[] _responseOptions;

        [SerializeField]
        private bool _moveNpc = false;

        [SerializeField]
        private NpcController _npcToMove; 

        #endregion

        public void TriggerCommunication(ComController _controller)
        {
            if(_moveNpc && _npcToMove != null)
            {
                _moveNpc = false;
                _npcToMove.Cutscene = true;
            }
            if(_comType == ComType.Dialogue)
                ComManager.Instance.BeginCommunication(_comType, _dialogue, _controller);
            else if(_comType == ComType.Choice)
                ComManager.Instance.BeginCommunication(_comType, _dialogue, _choices, _choiceIndex, _responseOptions, _controller);
        }

        public void EndCommunication()
        {
            ComManager.Instance.EndCommunication();
        }
    
    }

}