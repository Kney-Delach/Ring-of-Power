using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon { 
    public class ChoiceUIController : MonoBehaviour
    {
        // reference to choice canvas
        [SerializeField]
        private CanvasGroup _choiceCanvas; 

        // references to buttons texts
        [SerializeField]
        private Text _buttonOneText; 

        [SerializeField]
        private Text _buttonTwoText; 

        [SerializeField]
        private Text _buttonThreeText;

        // reference to third button to turn off if no choice options 
        [SerializeField]
        private Button _buttonThree;

        // reference to current choices
        private Choices _choices; 
        public Choices Choices {get {return _choices ; } set { _choices = value ; } }

        // choice made delegate containing choice chosen index
        public delegate void OnChoiceMade(int choiceIndex); 

        // instantiate choice change observer set
        public OnChoiceMade onChoiceMadeCallback;

        #region Singleton

        // reference to instance
        private static ChoiceUIController _instance = null;
        public static ChoiceUIController Instance { get { return _instance ; } }

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


        public void OnButtonSelect(int buttonIndex)
        {
            DisableCanavs();
            onChoiceMadeCallback(buttonIndex);
        }

        // displays choices on buttons
        public void DispalyChoices(Choices choices)
        {
            Debug.Log("Displaying choices");
            _choices = choices; 
            UpdateButtonText();
        }

        // updates choice canvas buttons text
        private void UpdateButtonText()
        {
            if(_choices._choicesText.Length < 3)
            {
                ActivateCanvas();
                _buttonThree.gameObject.active = false;
                _buttonThree.enabled = false;
                _buttonOneText.text = _choices._choicesText[0];
                _buttonTwoText.text = _choices._choicesText[1];
            }
            else
            {
                ActivateCanvas();
                _buttonThree.gameObject.active = true;
                _buttonThree.enabled = true;
                _buttonOneText.text = _choices._choicesText[0];
                _buttonTwoText.text = _choices._choicesText[1];
                _buttonThreeText.text = _choices._choicesText[2];
            }         
        }

        // enable choice canvas
        private void ActivateCanvas()
        {
            _choiceCanvas.alpha = 1; 
            _choiceCanvas.interactable = true; 
            _choiceCanvas.blocksRaycasts = true;
        }

        // disable choice canvas
        public void DisableCanavs()
        {
            Debug.Log("Disabling Canvas");
            _choiceCanvas.alpha = 0; 
            _choiceCanvas.interactable = false; 
            _choiceCanvas.blocksRaycasts = false;
        }


    }
}