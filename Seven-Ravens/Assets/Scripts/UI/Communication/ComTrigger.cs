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

        #region  Ending Scenario checks
        [SerializeField]
        private bool _calculateStringValues = false;
        
        [SerializeField]
        private bool _endingReplacements = false;

        [SerializeField]
        private int[] _indexesToBeReplaced;

        [SerializeField]
        private bool[] _displayColors; 

        [SerializeField]
        private bool[] _displayBrothersSaved;

        [SerializeField]
        private bool[] _displayBrothersLeft; 

        [SerializeField]
        private int[] _replacementPositions;

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

        private ComController _curController;

        #endregion

        public void TriggerCommunication(ComController controller)
        {
            if(_calculateStringValues)
                WizardFightController.Instance.CalculateBrothersText();

            if(_endingReplacements)
            {
                foreach(int index in _indexesToBeReplaced)
                {
                    if(_displayColors[index])
                    {
                        _dialogue.sentences[index] = _dialogue.sentences[index].Insert(_replacementPositions[index], WizardFightController.Instance.GetColorsText());
                    }

                    else if(_displayBrothersSaved[index])
                    {
                        _dialogue.sentences[index] =  _dialogue.sentences[index].Insert(_replacementPositions[index], WizardFightController.Instance.GetBroSavedText());
                    }

                    else if(_displayBrothersLeft[index])
                    {
                        _dialogue.sentences[index] = _dialogue.sentences[index].Insert(_replacementPositions[index], WizardFightController.Instance.GetBroNotSavedText());
                    }
                }
            }
            
            _curController = controller;
            if(_moveNpc && _npcToMove != null)
            {
                _moveNpc = false;
                _npcToMove.Cutscene = true;
            }
            if(_comType == ComType.Dialogue)
                ComManager.Instance.BeginCommunication(_comType, _dialogue, controller);
            else if(_comType == ComType.Choice)
                ComManager.Instance.BeginCommunication(_comType, _dialogue, _choices, _choiceIndex, _responseOptions, controller);
        }
        
        public void EndCommunication()
        {
            ComManager.Instance.EndCommunication();
        }
    
    }

}