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
        private Animator _hasteFlashAnimator;

        [SerializeField]
        private Animator _invisFlashAnimator;

        [SerializeField]
        private Animator _charmFlashAnimator;

        [SerializeField]
        private Animator _freezeFlashAnimator;

        [SerializeField]
        private Animator _unrootFlashAnimator;

        [SerializeField]
        private Animator _bubbleFlashAnimator;

        [SerializeField]
        private Image[] _reloadImages;

        [SerializeField]
        private Text[] _reloadTexts; 

        [Header("Block UI Images")]
        [SerializeField]
        private GameObject _blockInvis;

        [SerializeField]
        private GameObject _blockHaste;

        [SerializeField]
        private GameObject _blockBubble; 

        [SerializeField]
        private GameObject _blockRoot; 

        [SerializeField]
        private GameObject _blockCharm; 

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

        # region Flash Animator Controllers

        public void HideAllFlashes()
        {
            if(_fireboltFlashAnimator != null)
                _fireboltFlashAnimator.SetBool("Active", false);
            
            if(_healFlashAnimator != null)
                _healFlashAnimator.SetBool("Active", false);
            
            if(_hasteFlashAnimator != null)
                _hasteFlashAnimator.SetBool("Active", false);

            if(_invisFlashAnimator != null)
                _invisFlashAnimator.SetBool("Active", false);

            if(_charmFlashAnimator != null)
                _charmFlashAnimator.SetBool("Active", false);
            
            if(_unrootFlashAnimator != null)
                _unrootFlashAnimator.SetBool("Active", false);

            if(_bubbleFlashAnimator != null)
                _bubbleFlashAnimator.SetBool("Active", false);

            if(_freezeFlashAnimator != null)
                _freezeFlashAnimator.SetBool("Active", false);
        }

        public void ActivateFireboltFlash()
        {
            _fireboltFlashAnimator.SetBool("Active", true);
        }

        public void HideFireboltFlash()
        {
            _fireboltFlashAnimator.SetBool("Active", false);
        }

        public void ActivateHasteFlash()
        {
            _hasteFlashAnimator.SetBool("Active", true);
        }

        public void HideHasteFlash()
        {
            _hasteFlashAnimator.SetBool("Active", false);
        }

        public void ActivateRootsFlash()
        {
            _unrootFlashAnimator.SetBool("Active", true);
        }

        public void HideRootsFlash()
        {
            _unrootFlashAnimator.SetBool("Active", false);
        }

        public void ActivateBubbleFlash()
        {
            _bubbleFlashAnimator.SetBool("Active", true);
        }

        public void HideBubbleFlash()
        {
            _bubbleFlashAnimator.SetBool("Active", false);
        }
        public void ActivateHealFlash()
        {
            _healFlashAnimator.SetBool("Active", true);
        }

        public void HideHealFlash()
        {
            _healFlashAnimator.SetBool("Active", false);
        }

        public void ActivateInvisibilityFlash()
        {
            _invisFlashAnimator.SetBool("Active", true);
        }

        public void HideInvisibilityFlash()
        {
            _invisFlashAnimator.SetBool("Active", false);
        }

        public void ActivateFreezeFlash()
        {
            _freezeFlashAnimator.SetBool("Active", true);
        }

        public void HideFreezeFlash()
        {
            _freezeFlashAnimator.SetBool("Active", false);
        }

        #endregion

        #region Block Image Functions 

        public void BlockAbility(string spellName)
        {
            switch (spellName)
                {
                    case "Firebolt":                        
                        break;
                    case "Invisibility":   
                        _blockInvis.SetActive(true);                                      
                        break;
                    case "Haste":    
                        _blockHaste.SetActive(true);                 
                        break;
                    case "ProtectiveBubble":   
                        _blockBubble.SetActive(true);                 
                        break;
                    case "RemoveRoots":         
                        _blockRoot.SetActive(true);             
                        break;
                    case "WaterFreeze":                         
                        break;
                    case "Charm": 
                        _blockCharm.SetActive(true);                      
                        break;
                    case "Heal":                        
                        break;
                    default:
                        break;
                }           
        }

        #endregion
    }
}