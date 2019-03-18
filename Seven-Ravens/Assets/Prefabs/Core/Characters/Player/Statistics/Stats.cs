using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Rokemon {
    public class Stats : MonoBehaviour
    {    
        [Header("UI Components")]

        [SerializeField]
        private string _name; 
        // reference to stat display image
        private Image _displayImage; 

        // reference to stat display text
        private Text _displayText; 

        // reference to current fill amount of stat
        private float _currentFill = 1; 

        [Header("Stat Values")]        
        [SerializeField]
        private float _fillSpeed = 1;


        // reference to stat name
        private string _statName; 
        public string StatName { get { return _statName ; } set { _statName = value ; } }

        // reference to stat value 
        [SerializeField]
        private float _maxValue = 100; 
        public float MaxValue { get { return _maxValue ;}}

        // reference to current stat value 
        private float _currentValue; 

        private void Start()
        {   
            _displayImage = GameObject.FindGameObjectWithTag(name).GetComponent<Image>(); 
            _displayText = GameObject.FindGameObjectWithTag(name + "Text").GetComponent<Text>();

            _currentValue = _maxValue;
        }

        private void Update()
        {
            HandleFillBar();
        }

        // handle progress bar filling smoothly
        private void HandleFillBar()
        {
            if(_currentFill != _displayImage.fillAmount)
            {
                _displayImage.fillAmount = Mathf.Lerp(_displayImage.fillAmount, _currentFill, Time.deltaTime * _fillSpeed);
            }
        }
        // update ui components of stat
        private void UpdateUI()
        {
            _currentFill = _currentValue / _maxValue;
            _displayText.text = _currentValue + " / " + _maxValue; 
        }

        // add an amount to stat value
        public void AddValue(float amount)
        {   
            if(amount < 0)
                Debug.LogError("Stats AddValue Error: Attempting to add negative amounts to stat!");

            // verify addition of value not above max
            float tempVal = amount + _currentValue; 
            if(amount > _maxValue || tempVal > _maxValue)
            {
                _currentValue = _maxValue;
            } 
            else
            {
                _currentValue = tempVal;     
            }

            UpdateUI();
        }

        // reduce an amount from stat value 
        public void ReduceValue(float amount)
        {   
            if(amount < 0)
                Debug.LogError("Stats AddValue Error: Attempting to reduce negative amounts to stat!");
 
            // verify addition of value not below min
            float tempVal = _currentValue - amount; 
            if(tempVal < 0)
            {
                _currentValue = 0;
            } 
            else
            {
                _currentValue = tempVal;     
            }

            UpdateUI();
        }
    }
}