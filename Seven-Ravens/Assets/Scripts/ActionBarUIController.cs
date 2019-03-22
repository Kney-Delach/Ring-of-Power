using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class ActionBarUIController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private Animator _fireboltFlashAnimator;

        [SerializeField]
        private Animator _healFlashAnimator;

        [SerializeField]
        private Animator _invisFlashAnimator;

        [SerializeField]
        private Animator _charmFlashAnimator;

        [SerializeField]
        private Animator _unrootFlashAnimator;
        
        [SerializeField]
        private Image[] _reloadImages;

        [SerializeField]
        private Text[] _reloadTexts; 

        #region Singleton 

        // reference to instance
        private static ActionBarUIController _instance = null;
        public static ActionBarUIController Instance { get { return _instance; } }

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

        public void ActivateFireboltFlash()
        {
            _fireboltFlashAnimator.SetBool("Active", true);
        }

        public void HideFireboltFlash()
        {
            _fireboltFlashAnimator.SetBool("Active", false);
        }

        public void ActivateHealFlash()
        {
            _healFlashAnimator.SetBool("Active", true);
        }

        public void HideHealFlash()
        {
            _healFlashAnimator.SetBool("Active", false);
        }
    }
}