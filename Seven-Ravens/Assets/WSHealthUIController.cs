using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {

    public class WSHealthUIController : MonoBehaviour
    {    
        [Header("UI Components")]
        [SerializeField]
        private Stats _health; 

        [SerializeField]
        private Image _displayImage;


        private void Start()
        {
            _health.WorldDisplayImage = _displayImage;
        }

        private void Update()
        {
            _health.HandleFillBar();
        }

          
    }
}
