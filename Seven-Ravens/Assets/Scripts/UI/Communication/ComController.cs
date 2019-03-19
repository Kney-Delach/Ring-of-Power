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

        private void Start()
        {
            if(_triggers.Length == 1)
                _singleTrigger = true;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                _triggers[_currentTriggerIndex].TriggerCommunication();
            }
        }
    }

}