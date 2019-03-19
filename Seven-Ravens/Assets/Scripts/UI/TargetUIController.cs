using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {

    public class TargetUIController : MonoBehaviour
    {
    
        [Header("UI Components")]
        [SerializeField]
        private CanvasGroup _targetGroup; 

        [SerializeField]
        private Image _targetImage; 

        [SerializeField]
        private Image _displayImage; 

        [SerializeField]
        private float _fillAmount; 

        [SerializeField]
        private Text _targetText;

        private GameObject _target; 

        private Stats _targetHealth; 
        
        #region SINGELTON
        // this controller's instance 
        private static TargetUIController _instance;
        public static TargetUIController Instance { get { return _instance; } }

        // initialize instance
        void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;              
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        #endregion

        private void Start()
        {
            _target = null;
            UpdateUI();

            PlayerController playerController = FindObjectOfType<PlayerController>();

            playerController.notifyTargetObservers += TargetChange;

        }

        private void Update()
        {
            if(_target != null)
                _targetHealth.HandleFillBar();
        }

        public void TargetChange(GameObject target)
        {   
            if(target == null)
            {
                if(_target != null)
                {
                    _targetHealth.DisplayImage = null;
                    _targetHealth.DisplayText = null;
                }
                _targetHealth = null;
                _target = target;

            }
            else 
            {
                _target = target;

                _targetHealth = _target.GetComponent<Stats>(); 

            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if(_target == null)
            {
                _targetGroup.alpha = 0;

            }
            else 
            {
                _targetGroup.alpha = 1;
                _targetHealth.DisplayImage = _displayImage;
                _targetHealth.DisplayText = _targetText;
                _targetHealth.UpdateUI();
            }
        }

        
    }
}