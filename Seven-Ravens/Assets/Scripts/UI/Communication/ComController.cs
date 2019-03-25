using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    public class ComController : MonoBehaviour
    {   
        [Header("Communication Trigger")]
        [SerializeField]
        private ComTrigger[] _triggers;

        private bool _singleTrigger = false; 
        
        private int _currentTriggerIndex = 0;

        [SerializeField]
        private bool _sceneStarter = false;

        [SerializeField]
        private bool _decisionTypeCheck = false;

        [SerializeField]
        private bool _eventTrigger = false;
        public bool EventTrigger { get { return _eventTrigger ; } }

        [SerializeField]
        private bool _isItem = false;

        [SerializeField]
        private bool _isDeathCom = false; 

        private bool _isCollding = false;

        private bool _isActive = false;

        private bool _itemActivated = false;
        private bool _currentComActive = false;

        private static string PLAYER_TAG = "Player";
        
        [Header("Event Communication References")]
        [SerializeField]
        private GameObject _npc;
        public GameObject Npc { get { return _npc ; } } 

        [SerializeField]
        private string _abilityName;
        public string AbilityName { get { return _abilityName ; } } 

        private bool _currentEventComplete = false;

        private bool _complete = false; 

        [SerializeField]
        private bool _isFinalPhoenixDeath = false;

        private ComController _instance;
        public ComController Instance { get { return _instance ; } }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            if(_triggers.Length == 1)
                _singleTrigger = true;
            
            if(_sceneStarter)
            {
                if(_decisionTypeCheck)
                {
                    PerformCommFromCheck(PlayerInformationController.Instance.GetRecentChoiceMade());
                }
                else 
                {
                    _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
                    gameObject.SetActive(false);
                }

            }
        }

        public void PerformCommFromCheck(ChoicesMadeType type)
        {
            switch(type)
            {
                case ChoicesMadeType.Good:
                    _triggers[0].TriggerCommunication(_instance);
                    break;
                case ChoicesMadeType.Neutral:
                    _triggers[1].TriggerCommunication(_instance);
                    break;
                case ChoicesMadeType.Bad:
                    _triggers[2].TriggerCommunication(_instance);
                    break;
                default:
                    break;
            }
        }
        public void ComEventTrigger(int triggerIndex ,bool finalTrigger)
        {
            _triggers[triggerIndex].TriggerCommunication(_instance);
            if(finalTrigger)
                gameObject.SetActive(false);

        }

        private void Update()
        {
            if(!_sceneStarter && !_eventTrigger && !_isDeathCom)
            {
                if(_isCollding)
                {
                    if(_isItem && !_itemActivated)
                    {
                        _itemActivated = true;
                        _isActive = true;
                    }else if(_isItem && _itemActivated)
                    {

                    }
                    else if(!_complete && Input.GetKeyDown(KeyCode.K))
                    {
                        _isActive = true;
                    }
                    
                    if(_isActive)
                    {

                        if(!_currentComActive) 
                        {
                            _currentComActive = true;
                            if(_currentTriggerIndex < _triggers.Length)
                                _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
                        }
                    }

                }
            }
        }

        public void TriggerComplete()
        {
            if(_eventTrigger)
            {
                _currentTriggerIndex++;
                Debug.Log(_currentTriggerIndex);
                if(_currentEventComplete)
                {
                    // PlayerController.Instance.UnfreezePlayer();
                    // gameObject.SetActive(false); // set gameobject inactive 
                }
                else if(_currentTriggerIndex < _triggers.Length) 
                {
                    Debug.Log("REACHED SIMULATION POINT 4");
                    PlayerController.Instance.FreezePlayer();
                    TriggerCommunicationEvents();
                }
                else 
                {
                    _currentEventComplete = true;
                    PlayerController.Instance.UnfreezePlayer();
                    Debug.Log("Exiting ComController");
                    gameObject.SetActive(false);
                }
            }
            else 
            {
                if(!_singleTrigger)
                {
                    _currentTriggerIndex ++;
                }
                _isActive = false;
                _currentComActive = false;
                if(_isItem && _itemActivated)
                    gameObject.SetActive(false);
            }

            // TODO: Fix
            if(_isFinalPhoenixDeath && _currentTriggerIndex == 2)
                WizardFightController.Instance.InstantiateFirebolt();
           
        }
        
        public void TriggerCompleteNoIncrement()
        {
             _isActive = false;
            _currentComActive = false;
        }
        public void TriggerCompleteCutscene()
        {
            _currentComActive = true;
            _isActive = false;
            _isCollding = false;
            _complete = true;
            gameObject.SetActive(false);
        }

        public void TriggerCommunicationEvents()
        {
            _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {   
            if(_eventTrigger &&  collision.tag == PLAYER_TAG)
            {
                TriggerCommunicationEvents();
            }
            else if(!_sceneStarter && collision.tag == PLAYER_TAG)
            {
                _isCollding = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(_eventTrigger &&  collision.tag == PLAYER_TAG)
            {

            }
            else if(!_sceneStarter && collision.tag == PLAYER_TAG) 
            {
                _currentComActive = false;
            }
        }

    }

}