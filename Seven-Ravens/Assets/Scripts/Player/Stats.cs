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
        public Image DisplayImage { set { _displayImage = value ; } }

        private Image _worldDisplayImage; 
        public Image WorldDisplayImage { set { _worldDisplayImage = value ; } }


        // reference to stat display text
        private Text _displayText; 
        public Text DisplayText { set { _displayText = value ; } }

        // reference to current fill amount of stat
        public float _currentFill = 1; 

        [Header("Stat Values")]        
        [SerializeField]
        private float _fillSpeed = 1;


        // reference to stat name
        private string _statName; 
        public string StatName { get { return _statName ; } set { _statName = value ; } }

        [SerializeField]
        private bool _isPlayer = false;

        // reference to whether or not invincibility shield is active
        private bool _shieldActive = false; 

        // reference to stat value 
        [SerializeField]
        private float _maxValue = 100; 
        public float MaxValue { get { return _maxValue ;}}

        // reference to current stat value 
        private float _currentValue; 
        public float CurrentValue { get { return _currentValue ; } }
        //public float CurrentValue { get { return _currentValue ; }}

        private void Start()
        {   
            if(_isPlayer)
            {
                _displayImage = GameObject.FindGameObjectWithTag(name).GetComponent<Image>(); 
                _displayText = GameObject.FindGameObjectWithTag(name + "Text").GetComponent<Text>();
            }


            _currentValue = _maxValue;
        }

        private void Update()
        {
            if(_isPlayer)
                HandleFillBar();
        }

        // handle progress bar filling smoothly
        public void HandleFillBar()
        {   
            if(_worldDisplayImage != null && _currentFill != _worldDisplayImage.fillAmount)
            {
                _worldDisplayImage.fillAmount = Mathf.Lerp(_worldDisplayImage.fillAmount, _currentFill, Time.deltaTime * _fillSpeed);

            }

            if(_displayImage != null && _currentFill != _displayImage.fillAmount)
            {
                _displayImage.fillAmount = Mathf.Lerp(_displayImage.fillAmount, _currentFill, Time.deltaTime * _fillSpeed);
            }
        }
        // update ui components of stat
        public void UpdateUI()
        {
            _currentFill = _currentValue / _maxValue;
            if(_displayText != null)
                _displayText.text = _currentValue.ToString("f0") + " / " + _maxValue; 
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
            if(_shieldActive)
            {
                Debug.Log("Invincible shield activated");
            }
            else 
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

        public bool CompareMaximum()
        {
            if(_currentValue < _maxValue)
                return true; 
            else 
                return false;
        }

        public void ActivateShield()
        {
            if(!_shieldActive && _isPlayer)
                _shieldActive = true; 
        }

        public void DeactivateShield()
        {
            if(_shieldActive && _isPlayer)
                _shieldActive = false; 
        }
    }
}