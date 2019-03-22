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
        private bool _eventTrigger = false;

        private bool _isCollding = false;

        private bool _isActive = false;

        private bool _currentComActive = false;

        private bool _currentComComplete = false;
        private static string PLAYER_TAG = "Player";

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
                _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
                gameObject.SetActive(false);
            }
        }

        public void ComEventTrigger(bool finalTrigger)
        {
            _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
            if(finalTrigger)
                gameObject.SetActive(false);
        }

        private void Update()
        {
            if(!_sceneStarter && !_eventTrigger)
            {
                if(_isCollding)
                {
                    if(Input.GetKeyDown(KeyCode.K))
                        _isActive = true;
                    
                    if(_isActive)
                    {

                        if(!_currentComActive) 
                        {
                            _currentComActive = true;
                            _triggers[_currentTriggerIndex].TriggerCommunication(_instance);
                        }
                    }

                }
            }
        }

        public void TriggerComplete()
        {
            if(!_singleTrigger)
                _currentTriggerIndex ++;
            _isActive = false;
            _currentComActive = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!_sceneStarter && collision.tag == PLAYER_TAG)
            {
                _isCollding = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(!_sceneStarter && collision.tag == PLAYER_TAG)
            {
                _isCollding = false;
            }
        }

    }

}