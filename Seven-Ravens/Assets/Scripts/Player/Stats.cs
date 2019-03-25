using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using LevelManagement;

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

        [Header("Death Reference components")]
        [SerializeField]
        private bool _isRoot = false; 

        [SerializeField]
        private bool _isTraveller = false; 

        [SerializeField]
        private bool _isPhoenix = false; 

        [SerializeField]
        private bool _isCerberus = false;

        [SerializeField]
        private bool _isSerpent = false;

        [SerializeField]
        private bool _isWizard = false;

        [SerializeField]
        Transform _movePosition; 

        [SerializeField]
        private ComController _oneHealthComController; 

        [SerializeField]
        private ComController _deathComController;

        [SerializeField]
        private ZonerController _prevZonerController;

        [SerializeField]
        private bool _oneHealthTrigger = false; 
        public bool OneHealthTrigger { get {return _oneHealthTrigger ; } set { _oneHealthTrigger = value ; } }

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
        public float MaxValue { get { return _maxValue ; } set { _maxValue = value ; } }

        // reference to whether or not stat owner is dead
        private bool _isDead; 

        // reference to current stat value 
        private float _currentValue; 
        public float CurrentValue { get { return _currentValue ; } set { _currentValue = value ; } }
        //public float CurrentValue { get { return _currentValue ; }}

        [SerializeField]
        private bool _isDeadBunny = false;

        private void Start()
        {   
            if(_isPlayer)
            {
                _displayImage = GameObject.FindGameObjectWithTag(name).GetComponent<Image>(); 
                _displayText = GameObject.FindGameObjectWithTag(name + "Text").GetComponent<Text>();
            }


            _currentValue = _maxValue;

            if(_isDeadBunny)
            {
                _isDead = true; 
                _currentValue = 0;
                UpdateUI();
            }
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
            if(_isWizard && _oneHealthTrigger)
            {
                ProcessWizardSpared();
            }
            else 
            {
                if(_isPhoenix)
                {
                    _currentValue = _maxValue;
                    GetComponent<ItemDropper>().DropGoodItem();
                    //_shieldActive = true;
                    gameObject.tag = "Untagged";
                    ActionBarUIController.Instance.HideFireboltFlash();
                    ActionBarUIController.Instance.HideHealFlash();
                    _oneHealthComController.ComEventTrigger(1,true);
                }
                else 
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
                }

                UpdateUI();
            }

           

        }

        private void ProcessWizardSpared()
        {
            _currentValue = _maxValue;
            UpdateUI();
            Debug.Log("WIZARD HAS BEEN Spared");
            WizardFightController.Instance.ProcessWizardSparedComplete();
        }

        private void ProcessWizardDeath()
        { 
            _currentValue = 0;
            UpdateUI();
            _isDead = true;
            Debug.Log("WIZARD HAS BEEN KILLED");
            WizardFightController.Instance.ProcessWizardKilledComplete();
        }
        // reduce an amount from stat value 
        public void ReduceValue(float amount)
        {   
            if(_isWizard && _oneHealthTrigger)
            {
                ProcessWizardDeath();
               
            }
            else if(_isWizard && !_oneHealthTrigger)
            {
                _currentValue = _currentValue - amount;
                UpdateUI();
                _oneHealthTrigger = true;
                PlayerController.Instance.RemoveTarget();
                gameObject.tag = "HealableEnemy";
                gameObject.layer = 8;
                ActionBarUIController.Instance.ActivateFireboltFlash();
                ActionBarUIController.Instance.ActivateHealFlash();

            }
            else 
            {
                if(_shieldActive)
                {
                    Debug.Log("Invincible shield active");
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
                    if(_currentValue <= 0 && _oneHealthTrigger)
                    {
                        processOneHealth(); 
                        _oneHealthTrigger = false;
                        _currentValue = 1; 
                    }
                    else if (!_isDead && _currentValue <= 0)
                        ProcessDeath();

                    UpdateUI();
                }
            }
           
            
        }

        private void processOneHealth()
        {
            if(_movePosition != null)
                transform.position = _movePosition.position;
            
            if(_prevZonerController != null)
                _prevZonerController.SetInactive();
                
            _oneHealthComController.ComEventTrigger(0,false);
            ActionBarUIController.Instance.ActivateHealFlash();
            ActionBarUIController.Instance.ActivateFireboltFlash();
            
        }

        // TODO: Refractor Traveller / Serpent
        private void ProcessDeath()
        {
            _isDead = true;
            if(_isRoot)
                GetComponentInParent<Tangler>().DestroyRoot();
            if(_isPhoenix && !_isCerberus)
            {
                PlayerController.Instance.Mana.MaxValue += 20; 
                ActionBarUIController.Instance.HideFireboltFlash();
                ActionBarUIController.Instance.HideHealFlash();
                GetComponent<ItemDropper>().DropCursedItem();
                _oneHealthComController.ComEventTrigger(2,true);
            }
            if(_isCerberus && !_isPhoenix)
            {
                GetComponent<Collider2D>().enabled = false;
                PlayerController.Instance.RemoveTarget();
                gameObject.layer = 1;   
            }
            if(_isTraveller)
            {
                PlayerController.Instance.Mana.MaxValue += 20; 
                if(_deathComController != null)
                    _deathComController.Instance.TriggerCommunicationEvents();
                //ActionBarUIController.Instance.HideFireboltFlash();
                GetComponent<Collider2D>().enabled = false;
                gameObject.layer = 1;   
                PlayerController.Instance.RemoveTarget();
                ActionBarUIController.Instance.HideFireboltFlash();
                GetComponent<ItemDropper>().DropCursedItem();

                PlayerInformationController.Instance.ReplaceChoice(ChoicesMadeType.Bad);
            }

            if(_isPhoenix && _isCerberus)
            {
                WizardFightController.Instance.RemoveEnemies();

                GetComponent<Collider2D>().enabled = false;
                PlayerController.Instance.RemoveTarget();
                gameObject.layer = 1;  
                
                if(_movePosition != null)
                    transform.position = _movePosition.position;
                
                if(_deathComController != null)
                    _deathComController.Instance.TriggerCommunicationEvents();
            }

            if(_isSerpent)
            {
                PlayerController.Instance.Mana.MaxValue += 20; 

               GoodItem item = FindObjectOfType<GoodItem>();
                if(item != null)
                    item.MoveSheepBad();
                else if(_deathComController != null)
                {
                    _deathComController.Instance.TriggerCommunicationEvents();
                    PlayerInformationController.Instance.ReplaceChoice(ChoicesMadeType.Bad);
                }

                //ActionBarUIController.Instance.HideFireboltFlash();
                GetComponent<Collider2D>().enabled = false;
                gameObject.layer = 1;   
                PlayerController.Instance.RemoveTarget();
                ActionBarUIController.Instance.HideFireboltFlash();
                ActionBarUIController.Instance.HideCharmFlash();
                GetComponent<ItemDropper>().DropCursedItem();
                

  

            }
            if(_isPlayer && WizardFightController.Instance == null)
            {
                StartCoroutine(RespawnRoutine());
            } 
            else if(_isPlayer && WizardFightController.Instance != null)
            {
                PlayerController.Instance.RemoveTarget();
                PlayerController.Instance.FreezePlayer();
                _currentValue = 0;
                PlayerController.Instance.Mana.CurrentValue = 0;
                UpdateUI();
                PlayerController.Instance.Mana.UpdateUI();
                WizardFightController.Instance.ProcessGirlDeath();
            }

        }
        private IEnumerator RespawnRoutine()
        {
            ComManager.Instance.EndCommunication();
            PlayerController.Instance.RemoveTarget();
            PlayerController.Instance.FreezePlayer();
            yield return new WaitForSeconds(0.3f);
            _currentValue = _maxValue;
            PlayerController.Instance.Mana.CurrentValue = PlayerController.Instance.Mana.MaxValue;
            LevelLoader.ReloadLevel();
            UpdateUI();
            PlayerController.Instance.Mana.UpdateUI();
            _isDead = false;
            PlayerController.Instance.UnfreezePlayer();
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
            {
                _shieldActive = true; 
                _displayImage.color = Color.blue;
            }
        }

        public void DeactivateShield()
        {
            if(_shieldActive && _isPlayer)
            {            
                Debug.Log("Deactivating shield");

                _shieldActive = false; 
                _displayImage.color = Color.white;

            }
        }
    }
}