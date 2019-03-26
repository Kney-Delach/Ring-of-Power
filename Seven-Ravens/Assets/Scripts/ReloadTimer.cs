using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class ReloadTimer : MonoBehaviour
    {   
        // refernece to active status of timer
        private bool _active = false;

        // reference to time left on timer
        private float _timeLeft = 0;

        [SerializeField]
        private Text _reloadText;

        private void Start()
        {
            _reloadText.enabled = false;
        }
        private void Update()
        {
            if(_active)
                UpdateTimer();
        }


        // function updating timer values
        private void UpdateTimer()
        {
            _timeLeft -= Time.deltaTime;
            if ( _timeLeft <= 0 )
            {
                DeactivateTimer();
            }
            else 
            {
                _reloadText.text = _timeLeft.ToString("F0");
            }
        }

        private void DeactivateTimer()
        {
            _reloadText.enabled = false;
            _active = false;
        }
        // function activating timer
        public void ActivateTimer(float timeLeft)
        {
            _active = true;
            _timeLeft = timeLeft;
            _reloadText.enabled = true;
            _reloadText.text = _timeLeft.ToString("F0");
        }
    }
}